using System;
using System.Collections.Generic;
using UnityEngine;
using Bolt;
using static GameSettings;

[BoltGlobalBehaviour(BoltNetworkModes.Server)]
public class PoolMan : Bolt.GlobalEventListener {

    #region Singleton
    public static PoolMan Instance { get; private set; }
    #endregion

    #region Constants
    #endregion

    #region Variables
    #endregion

    #region Containers
    private GameSettings settings;

    private Dictionary<string, Queue<Unit>>[] PoolsByQuality;
    #endregion

    #region Unity Methods
    private void Awake() {
        Instance = this; // Singleton
    }

    private void Start() {
        InitializePools();
        settings = GameMan.Instance.settings;
    }
    #endregion

    #region Initialisation
    private void InitializePools() {
        // Init. the dictionary-array
        PoolsByQuality = new Dictionary<string, Queue<Unit>>[settings.RarityInfos.Length];
        for(int i = 0; i < PoolsByQuality.Length; i++) {
            PoolsByQuality[i] = new Dictionary<string, Queue<Unit>>();
        }

        // Create Pools
        foreach (GameObject prefab in settings.BaseUnitPrefabs) {
            Unit unit = prefab.GetComponent<Unit>();
            int rarIdx = settings.GetRarityIndex(unit.properties.rarity);

            // Fill the Pool
            Queue<Unit> Pool = new Queue<Unit>();
            for (int i = 0; i < settings.RarityInfos[rarIdx].poolSize; i++) {
                Unit _unit = InstantiateUnit(unit);
                _unit.gameObject.SetActive(false);
                Pool.Enqueue(_unit);
            }

            // Add the Pool at the appropriate index
            PoolsByQuality[rarIdx].Add(unit.properties.name, Pool);
        }
    }

    private void InitEventSubscribers() {
        StoreMan store = StoreMan.Instance;
        store.SpawnRandomUnitEvent += HandleSpawnRandomUnitEvent;
        store.DespawnUnitEvent += HandleDespawnUnitEvent;
    }
    #endregion

    #region Spawn Random Unit for Store
    // Determine which Quality Pokemon to Spawn with Ticket System, depending on DROP_CHANCE Table
    // only runs more than once if all pools of selected quality are empty.
    // in such a case it will refind random quality until it found a non-empty pool.
    // It will not search again for those qualities it has already deemed to have empty pools
    private Rarity DetermineRandomQuality(int level, List<Rarity> QualitiesToConsider) {
        Rarity rarity = Rarity.COMMON;

        //calculate number of tickets to generate
        int maxTicketNumber = 0;
        foreach (Rarity _rarity in QualitiesToConsider) {
            maxTicketNumber += settings.GetDropChance(level, _rarity);
        }

        if (maxTicketNumber == 0) {
            Debug.LogWarning("ALL Unit Pools are currently Empty");
            return rarity;
        }

        //choose winner of lottery
        int ticket = GameMan.rng.Next(maxTicketNumber);
        foreach (Rarity _rarity in QualitiesToConsider) {
            if ((ticket -= settings.GetDropChance(level, _rarity)) < 0) {
                rarity = _rarity;
                break;
            }
        }

        //check if those pools contain at least 1 unit, otherwise find new quality and exclude current
        int numUnits = 0;
        foreach (KeyValuePair<string, Queue<Unit>> entry in PoolsByQuality[settings.GetRarityIndex(rarity)]) {
            numUnits += entry.Value.Count;
        }
        if (numUnits > 0) {
            return rarity;
        } else {
            QualitiesToConsider.Remove(rarity);
            return DetermineRandomQuality(level, QualitiesToConsider);
        }
    }

    private Unit SpawnRandomUnit(Rarity rarity) {
        string unitToSpawn = DetermineRandomUnit(rarity);
        Unit unit = PoolsByQuality[settings.GetRarityIndex(rarity)][unitToSpawn].Dequeue();
        unit.gameObject.SetActive(true);
        return unit;
    }

    // Determine which Pokemon to spawn with Ticket system, depending on Queue Size
    private string DetermineRandomUnit(Rarity rarity) {

        int numUnits = 0; //total number of individual (!) pokemon of this rarity
        Dictionary<string, Queue<Unit>> pools;

        pools = PoolsByQuality[settings.GetRarityIndex(rarity)];
        foreach (KeyValuePair<string, Queue<Unit>> entry in pools) {
            numUnits += entry.Value.Count;
        }

        int spawnNumber = GameMan.rng.Next(numUnits);
        foreach (KeyValuePair<string, Queue<Unit>> entry in pools) {
            if ((spawnNumber -= entry.Value.Count) < 0) {
                return entry.Key;
            }
        }
        Debug.LogWarning("Ticket number was greater than total Queue Sizes added up");
        return null;
    }
    #endregion

    #region Pools
    public void ReturnToPool(Unit unit) /*TODO: When returning an Evolution, return all used base models instead, then despawn evolution*/ {
        Dictionary<string, Queue<Unit>> Pools = PoolsByQuality[settings.GetRarityIndex(unit.properties.rarity)];
        if (!Pools.ContainsKey(unit.properties.name)) {
            Debug.LogWarning("Dictionary does not contain tag " + unit.properties.name);
            return;
        }
        Pools[unit.properties.name].Enqueue(unit);
    }
    #endregion

    #region Helper Methods
    //Instantiate new Pokemon, with 50% chance to spawn its variant if it exists
    public Unit InstantiateUnit(Unit unit) {
        GameObject unitObject = (unit.variant != null && GameMan.rng.Next(10) < 5) ?
            unit.variant.gameObject : unit.gameObject;

        var unitEntity = BoltNetwork.Instantiate(unitObject);

        return unitEntity.GetComponent<Unit>();
    }
    #endregion

    #region Handle Incoming Events
    //determine random quality depending on level, then random unit depending on pools, then dequeue unit, activate it, set transform and attach to store
    private Unit HandleSpawnRandomUnitEvent() {
        List<Rarity> allRarities = new List<Rarity>();
        foreach (Rarity q in Enum.GetValues(typeof(Rarity))) {
            if (q != Rarity.LEGENDARY) allRarities.Add(q);
        }
        return SpawnRandomUnit(DetermineRandomQuality(LevelMan.Instance.Level, allRarities));
    }

    //deactivate unit, remove parent, queue unit
    private void HandleDespawnUnitEvent(Unit unit) {
        unit.gameObject.SetActive(false);
        unit.transform.SetParent(null);
        ReturnToPool(unit);
    }
    #endregion
}
