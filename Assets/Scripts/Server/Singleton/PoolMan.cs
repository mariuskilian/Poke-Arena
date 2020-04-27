using UnityEngine;
using System.Collections.Generic;
using static GameInfo;

[BoltGlobalBehaviour(BoltNetworkModes.Server)]
public class PoolMan : ServerManager {

    public Dictionary<string, Queue<Unit>>[] PoolsByRarity { get; private set; }

    public static GameSettings test;

    public override void OnEvent(GameLoadedEvent evnt) {
        Game = GameMan.Instance as GameMan;
        InitPools();
    }

    #region Pools
    private void InitPools() {
        // Initialize pool dictionaries
        PoolsByRarity = new Dictionary<string, Queue<Unit>>[NumRarities];
        for (int i = 0; i < NumRarities; i++) PoolsByRarity[i] = new Dictionary<string, Queue<Unit>>();

        // Initialize pools
        foreach (Unit unitPrefab in DataHolder.Instance.BaseUnitPrefabs) {
            Queue<Unit> Pool = new Queue<Unit>();
            for (int i = 0; i < Game.Settings.PoolSize[(int)unitPrefab.properties.rarity]; i++) {
                Unit unit = InstantiateUnit(unitPrefab);
                SetUnitActiveState(unit, false);
                Pool.Enqueue(unit);
            }
            PoolsByRarity[(int)unitPrefab.properties.rarity].Add(unitPrefab.properties.name, Pool);
        }
    }
    #endregion


    
    #region Spawning Random Unit
    private Unit SpawnRandomUnit(Rarity rarity) {
        Dictionary<string, Queue<Unit>> Pools = PoolsByRarity[(int)rarity];

        int numUnits = 0;
        foreach (KeyValuePair<string, Queue<Unit>> pool in Pools) numUnits += pool.Value.Count;
        
        string unitName = "";
        int ticket = RNG.Next(numUnits);
        foreach (KeyValuePair<string, Queue<Unit>> pool in Pools)
            if ((ticket -= pool.Value.Count) < 0) unitName = pool.Key;
        
        Unit unit = PoolsByRarity[(int)rarity][unitName].Dequeue();
        SetUnitActiveState(unit, false);
        return unit;
    }

    private Rarity DetermineRandomQuality(int level) {
        int ticket = RNG.Next(100);
        foreach (Rarity rarity in GameInfo.Rarities)
            if ((ticket -= Game.Settings.DropChance[level, (int)rarity]) < 0) return rarity;

        return Rarity.COMMON;
    }
    #endregion



    #region Helpers
    /// <summary>
    /// Instantiates Unit and, if the unit has a gender variant, spawns
    /// that with a 50% chance instead
    /// </summary>
    /// <param name="unit"> The Unit prefab to instantiate </param>
    /// <returns> The instantiated Unit </returns>
    private Unit InstantiateUnit(Unit unit) {
        GameObject unitObject = (unit.variant != null && RNG.Next(10) < 5) ?
            unit.variant.gameObject : unit.gameObject;

        return BoltNetwork.Instantiate(unitObject).GetComponent<Unit>();
    }

    /// <summary>
    /// Basically dactivating the whole Unit GameObject would mess with the BoltEntity
    /// so this (de)activates all the units children instead, leaving the BoltEntity
    /// as well as Unit-scripts in tact
    /// </summary>
    /// <param name="unit"> The Unit to (de)activate </param>
    /// <param name="activeState"> The state to set it to </param>
    private void SetUnitActiveState(Unit unit, bool activeState) {
        for (int childIdx = 0; childIdx < unit.transform.childCount; childIdx++) {
            unit.transform.GetChild(childIdx).gameObject.SetActive(activeState);
        }
    }
    #endregion

}