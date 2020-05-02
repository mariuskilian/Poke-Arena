using UnityEngine;
using Bolt;
using System;
using System.Collections;
using System.Collections.Generic;
using static GameInfo;

[BoltGlobalBehaviour(BoltNetworkModes.Server)]
public class PoolMan : GlobalEventListener {

    public static PoolMan Instance { get; private set; }
    private void Awake() { if (Instance == null) Instance = this; }

    public Dictionary<string, UnitPool>[] PoolsByRarity { get; private set; }

    public static GameSettings test;

    #region Local Events
    public Action PoolsInitDoneEvent;
    #endregion

    private void Start() { SubscribeLocalEventHandlers(); }

    #region Pools
    private void InitPools() {
        // Initialize pool dictionaries
        PoolsByRarity = new Dictionary<string, UnitPool>[NumRarities];
        for (int i = 0; i < NumRarities; i++) PoolsByRarity[i] = new Dictionary<string, UnitPool>();

        // Initialize pools
        foreach (Unit unitPrefab in DataHolder.Instance.BaseUnitPrefabs) {
            Rarity rarity = unitPrefab.properties.rarity;
            UnitPool pool = new UnitPool(unitPrefab, GameMan.Instance.Settings.PoolSize[(int)rarity]);
            PoolsByRarity[(int)rarity].Add(unitPrefab.properties.name, pool);
        }

        PoolsInitDoneEvent?.Invoke();

    }
    #endregion



    #region Spawning Random Unit
    public Unit SpawnRandomUnit(int level) {
        Rarity rarity = DetermineRandomQuality(level);
        Dictionary<string, UnitPool> Pools = PoolsByRarity[(int)rarity];

        int numUnits = 0;
        foreach (KeyValuePair<string, UnitPool> pool in Pools) numUnits += pool.Value.Count;

        string unitName = "";
        int ticket = RNG.Next(numUnits);
        foreach (KeyValuePair<string, UnitPool> pair in Pools)
            if ((ticket -= pair.Value.Count) < 0) { unitName = pair.Key; break; }

        Unit unit = PoolsByRarity[(int)rarity][unitName].Dequeue();
        return unit;
    }

    private Rarity DetermineRandomQuality(int level) {
        int ticket = RNG.Next(100);
        foreach (Rarity rarity in GameInfo.Rarities)
            if ((ticket -= GameMan.Instance.Settings.DropChance[level, (int)rarity]) < 0) return rarity;

        return Rarity.COMMON;
    }
    #endregion



    #region Helpers
    private Unit InstantiateUnit(Unit unit) { return BoltNetwork.Instantiate(unit.gameObject).GetComponent<Unit>(); }

    private void FreezeAllStoreEntities() { foreach (var Pools in PoolsByRarity) foreach (var Pair in Pools) Pair.Value.FreezeAllEntities(); }
    #endregion



    #region Local Event Handlers 
    private void SubscribeLocalEventHandlers() {
        GameMan game = GameMan.Instance;
        game.GameLoadedEvent += HandleGameLoadedEvent;
        game.AllPlayersLoadedEvent += HandleAllPlayersLoadedEvent;
    }

    private void HandleGameLoadedEvent() { InitPools(); }
    private void HandleAllPlayersLoadedEvent() { FreezeAllStoreEntities(); }
    #endregion

}

public class UnitPool {

    public int Count { get; private set; }

    private Queue<Unit> storeUnits;
    private Queue<Unit> storeUnitsVariants;
    
    public UnitPool(Unit unit, int poolSize) {
        Count = poolSize;

        storeUnits = new Queue<Unit>();
        storeUnitsVariants = (unit.variant == null) ? null : new Queue<Unit>();

        for (int i = 0; i < StoreMan.StoreSize; i++) {
            var unitEntity = BoltNetwork.Instantiate(unit.gameObject);
            storeUnits.Enqueue(unitEntity.GetComponent<Unit>());
            if (unit.variant == null) continue;
            storeUnitsVariants.Enqueue(BoltNetwork.Instantiate(unit.variant.gameObject).GetComponent<Unit>());
        }
    }

    public Unit Dequeue() {
        if (Count == 0) return null;

        Unit unit;
        if (storeUnitsVariants != null && RNG.Next(10) < 5) {
            unit = storeUnitsVariants.Dequeue();
            storeUnitsVariants.Enqueue(unit);
        } else {
            unit = storeUnits.Dequeue();
            storeUnits.Enqueue(unit);
        }

        Count--;
        return unit;
    }

    public void FreezeAllEntities() {
        Unit unit;
        for (int i = 0; i < StoreMan.StoreSize; i++) {
            unit = storeUnits.Dequeue();
            unit.entity.Freeze(true);
            storeUnits.Enqueue(unit);

            if (unit.variant == null) continue;

            unit = storeUnitsVariants.Dequeue();
            unit.entity.Freeze(true);
            storeUnitsVariants.Enqueue(unit);
        }
    }

    public void Enueue() { Count++; }
}