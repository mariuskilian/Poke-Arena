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

    public Dictionary<string, StoreUnitPool>[] PoolsByRarity { get; private set; }

    public static GameSettings test;

    #region Local Events
    public Action PoolsInitDoneEvent;
    #endregion

    private void Start() { SubscribeLocalEventHandlers(); }

    #region Pools
    private void InitPools() {
        // Initialize pool dictionaries
        PoolsByRarity = new Dictionary<string, StoreUnitPool>[NumRarities];
        for (int i = 0; i < NumRarities; i++) PoolsByRarity[i] = new Dictionary<string, StoreUnitPool>();

        // Initialize pools
        foreach (StoreUnit storeUnitPrefab in DataHolder.Instance.StoreUnitPrefabs) {
            Rarity rarity = storeUnitPrefab.unit.properties.rarity;
            StoreUnitPool pool = new StoreUnitPool(storeUnitPrefab, GameMan.Instance.Settings.PoolSize[(int)rarity]);
            PoolsByRarity[(int)rarity].Add(storeUnitPrefab.unit.properties.name, pool);
        }

        PoolsInitDoneEvent?.Invoke();

    }
    #endregion



    #region Spawning Random Unit
    public StoreUnit SpawnRandomUnit(int level) {
        Rarity rarity = DetermineRandomQuality(level);
        Dictionary<string, StoreUnitPool> Pools = PoolsByRarity[(int)rarity];

        int numUnits = 0;
        foreach (KeyValuePair<string, StoreUnitPool> pool in Pools) numUnits += pool.Value.Count;

        string unitName = "";
        int ticket = RNG.Next(numUnits);
        foreach (KeyValuePair<string, StoreUnitPool> pair in Pools)
            if ((ticket -= pair.Value.Count) < 0) { unitName = pair.Key; break; }

        StoreUnit storeUnit = PoolsByRarity[(int)rarity][unitName].Dequeue();
        return storeUnit;
    }

    private Rarity DetermineRandomQuality(int level) {
        int ticket = RNG.Next(100);
        foreach (Rarity rarity in GameInfo.Rarities)
            if ((ticket -= GameMan.Instance.Settings.DropChance[level, (int)rarity]) < 0) return rarity;

        return Rarity.COMMON;
    }
    #endregion



    #region Helpers
    private StoreUnit InstantiateUnit(StoreUnit storeUnit) { return BoltNetwork.Instantiate(storeUnit.gameObject).GetComponent<StoreUnit>(); }

    private void FreezeAllStoreEntities() { foreach (var Pools in PoolsByRarity) foreach (var Pair in Pools) Pair.Value.FreezeAllEntities(); }

    private void EnqueueUnit(StoreUnit storeUnit) { PoolsByRarity[(int)storeUnit.unit.properties.rarity][storeUnit.unit.properties.name].Enqueue(); }
    #endregion



    #region Local Event Handlers 
    private void SubscribeLocalEventHandlers() {
        GameMan game = GameMan.Instance;
        game.GameLoadedEvent += HandleGameLoadedEvent;
        game.AllPlayersLoadedEvent += HandleAllPlayersLoadedEvent;
    }

    private void SubscribePlayerEventHandlers() {
        foreach (Player p in GameMan.Instance.Players) {
            PlayerStoreMan store = p.GetPlayerMan<PlayerStoreMan>();
            store.DespawnUnitEvent += HandleDespawnUnitEvent;
        }
    }

    private void HandleGameLoadedEvent() { InitPools(); }
    private void HandleAllPlayersLoadedEvent() { FreezeAllStoreEntities(); SubscribePlayerEventHandlers(); }
    private void HandleDespawnUnitEvent(StoreUnit storeUnit) { EnqueueUnit(storeUnit); }
    #endregion

}

public class StoreUnitPool {

    public int Count { get; private set; }

    private Queue<StoreUnit> storeUnits;
    private Queue<StoreUnit> storeUnitsVariants;
    
    public StoreUnitPool(StoreUnit storeUnit, int poolSize) {
        Count = poolSize;

        storeUnits = new Queue<StoreUnit>();
        storeUnitsVariants = (storeUnit.variant == null) ? null : new Queue<StoreUnit>();

        for (int i = 0; i < PlayerStoreMan.StoreSize; i++) {
            var storeUnitCopy = BoltNetwork.Instantiate(storeUnit.gameObject).GetComponent<StoreUnit>();
            storeUnitCopy.ResetUnitPosition();
            storeUnits.Enqueue(storeUnitCopy);

            if (storeUnit.variant == null) continue;

            storeUnitCopy = BoltNetwork.Instantiate(storeUnit.variant.gameObject).GetComponent<StoreUnit>();
            storeUnitCopy.ResetUnitPosition();
            storeUnitsVariants.Enqueue(storeUnitCopy);
        }
    }

    public StoreUnit Dequeue() {
        if (Count == 0) return null;

        StoreUnit storeUnit;
        if (storeUnitsVariants != null && RNG.Next(10) < 5) {
            storeUnit = storeUnitsVariants.Dequeue();
            storeUnitsVariants.Enqueue(storeUnit);
        } else {
            storeUnit = storeUnits.Dequeue();
            storeUnits.Enqueue(storeUnit);
        }

        Count--;
        return storeUnit;
    }

    public void FreezeAllEntities() {
        StoreUnit storeUnit;
        for (int i = 0; i < PlayerStoreMan.StoreSize; i++) {
            storeUnit = storeUnits.Dequeue();
            storeUnit.entity.Freeze(true);
            storeUnits.Enqueue(storeUnit);

            if (storeUnit.variant == null) continue;

            storeUnit = storeUnitsVariants.Dequeue();
            storeUnit.entity.Freeze(true);
            storeUnitsVariants.Enqueue(storeUnit);
        }
    }

    public void Enqueue() { Count++; }
}