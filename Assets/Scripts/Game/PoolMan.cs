using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PoolMan : ManagerBehaviour {

    #region Singleton
    private static PoolMan _instance;
    public static PoolMan Instance {
        get {
            if (_instance == null) { //should NEVER happen
                GameObject go = new GameObject("Game Manager");
                go.AddComponent<PoolMan>();
                Debug.LogWarning("Game Manager instance was null");
            }
            return _instance;
        }
    }
    #endregion

    #region Constants
    public enum QUALITY { COMMON, UNCOMMON, RARE, EPIC, LEGENDARY }
    public readonly int[] POOL_SIZE = { 45, 30, 25, 15, 10 };
    public readonly float[,] DROP_CHANCE = { //[level, rarity]
        /********   COM     UNCOM   RARE    EPIC    LEGEN   */
        /*LV 01*/ { 1f,     0f,     0f,     0f,     0f      },
        /*LV 02*/ { 0.7f,   0.3f,   0f,     0f,     0f      },
        /*LV 03*/ { 0.6f,   0.35f,  0.05f,  0f,     0f      },
        /*LV 04*/ { 0.5f,   0.35f,  0.15f,  0f,     0f      },
        /*LV 05*/ { 0.4f,   0.35f,  0.23f,  0.02f,  0f      },
        /*LV 06*/ { 0.33f,  0.3f,   0.3f,   0.07f,  0f      },
        /*LV 07*/ { 0.3f,   0.3f,   0.3f,   0.1f,   0f      },
        /*LV 08*/ { 0.24f,  0.3f,   0.3f,   0.15f,  0.01f   },
        /*LV 09*/ { 0.22f,  0.3f,   0.25f,  0.2f,   0.03f   },
        /*LV 10*/ { 0.19f,  0.25f,  0.25f,  0.25f,  0.06f   }
    };
    #endregion

    #region Variables
    public static System.Random random = new System.Random();
    #endregion

    #region Containers
    public List<GameObject> baseUnitPrefabs;
    public Dictionary<string, Queue<Unit>>[] poolsByQuality;
    #endregion

    #region Unity Methods
    private void Awake() {
        _instance = this; // Singleton
    }

    private void Start() {
        InitializePools();
        InitEventSubscribers();
    }
    #endregion

    #region Initialisation
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
    private QUALITY DetermineRandomQuality(int level, List<QUALITY> QualitiesToConsider) {
        QUALITY quality = QUALITY.COMMON;

        //calculate number of tickets to generate
        int maxQualityNumber = 0;
        foreach (QUALITY _quality in QualitiesToConsider) {
            maxQualityNumber += (int) (DROP_CHANCE[level, (int) _quality] * 100);
        }

        if (maxQualityNumber == 0) {
            Debug.LogWarning("ALL Unit Pools are currently Empty");
            return quality;
        }

        //choose winner of lottery
        int qualityNumber = random.Next(0, maxQualityNumber);
        foreach (QUALITY _quality in QualitiesToConsider) {
            if ((qualityNumber -= (int) (100 * DROP_CHANCE[level, (int) _quality])) < 0) {
                quality = _quality;
                break;
            }
        }

        //check if those pools contain at least 1 unit, otherwise find new quality and exclude current
        int numUnits = 0;
        foreach (KeyValuePair<string, Queue<Unit>> entry in poolsByQuality[(int) quality]) {
            numUnits += entry.Value.Count;
        }
        if (numUnits > 0) {
            return quality;
        } else {
            QualitiesToConsider.Remove(quality);
            return DetermineRandomQuality(level, QualitiesToConsider);
        }
    }

    private Unit SpawnRandomUnit(QUALITY quality) {
        string unitToSpawn = DetermineRandomUnit(quality);
        Unit unit = poolsByQuality[(int) quality][unitToSpawn].Dequeue();
        unit.gameObject.SetActive(true);
        return unit;
    }

    // Determine which Pokemon to spawn with Ticket system, depending on Queue Size
    private string DetermineRandomUnit(QUALITY quality) {

        int numUnits = 0; //total number of individual (!) pokemon of this rarity
        Dictionary<string, Queue<Unit>> pools;

        pools = poolsByQuality[(int) quality];
        foreach (KeyValuePair<string, Queue<Unit>> entry in pools) {
            numUnits += entry.Value.Count;
        }

        int spawnNumber = random.Next(0, numUnits);
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
        Dictionary<string, Queue<Unit>> pools = poolsByQuality[(int) unit.baseStats.quality];
        if (!pools.ContainsKey(unit.baseStats.name)) {
            Debug.LogWarning("Dictionary does not contain tag " + unit.baseStats.name);
            return;
        }
        pools[unit.baseStats.name].Enqueue(unit);
    }

    private void InitializePools() {
        poolsByQuality = new Dictionary<string, Queue<Unit>>[Enum.GetNames(typeof(QUALITY)).Length];
        foreach (QUALITY quality in Enum.GetValues(typeof(QUALITY))) {
            poolsByQuality[(int) quality] = new Dictionary<string, Queue<Unit>>();
        }

        //create Pools
        foreach (GameObject prefab in baseUnitPrefabs) {
            Unit unit = prefab.GetComponent<Unit>();
            UnitStats stats = unit.baseStats;

            //fill the pool
            Queue<Unit> unitPool = new Queue<Unit>();
            for (int i = 0; i < POOL_SIZE[(int) stats.quality]; i++) {
                Unit _unit = InstantiateUnit(unit);
                _unit.gameObject.SetActive(false);
                unitPool.Enqueue(_unit);
            }
            poolsByQuality[(int) stats.quality].Add(unit.baseStats.name, unitPool);
        }
    }
    #endregion

    #region Helper Methods
    //Instantiate new Pokemon, with 50% chance to spawn its variant if it exists
    public Unit InstantiateUnit(Unit unit) {
        GameObject unitObject = (unit.variant != null && random.Next(0, 10) < 5) ?
            Instantiate(unit.variant.gameObject) : Instantiate(unit.gameObject);

        return unitObject.GetComponent<Unit>(); ;
    }
    #endregion

    #region Handle Incoming Events
    //determine random quality depending on level, then random unit depending on pools, then dequeue unit, activate it, set transform and attach to store
    private Unit HandleSpawnRandomUnitEvent() {
        List<QUALITY> allQualities = new List<QUALITY>();
        foreach (QUALITY q in Enum.GetValues(typeof(QUALITY))) {
            allQualities.Add(q);
        }
        return SpawnRandomUnit(DetermineRandomQuality(LevelMan.Instance.Level, allQualities));
    }

    //deactivate unit, remove parent, queue unit
    private void HandleDespawnUnitEvent(Unit unit) {
        unit.gameObject.SetActive(false);
        unit.transform.SetParent(null);
        ReturnToPool(unit);
    }
    #endregion
}
