using UnityEngine;
using Bolt;
using System;
using System.Collections.Generic;

public class MPPoolMan : GlobalEventListener {

    #region Singleton
    public static MPPoolMan Instance { get; private set; }
    #endregion

    #region Constants
    public enum RARITY { COMMON, UNCOMMON, RARE, EPIC, SECRET, LEGENDARY };
    public readonly int[] POOL_SIZE = { 45, 30, 25, 15, 10, 1 };
    public readonly float[,] DROP_CHANCE = { //[level, rarity]
        /********   COM     UNCOM   RARE    EPIC    SECRET  LEGEN */
        /*LV 01*/ { 1f,     0f,     0f,     0f,     0f,     0f  },
        /*LV 02*/ { 0.7f,   0.3f,   0f,     0f,     0f,     0f  },
        /*LV 03*/ { 0.6f,   0.35f,  0.05f,  0f,     0f,     0f  },
        /*LV 04*/ { 0.5f,   0.35f,  0.15f,  0f,     0f,     0f  },
        /*LV 05*/ { 0.4f,   0.35f,  0.23f,  0.02f,  0f,     0f  },
        /*LV 06*/ { 0.33f,  0.3f,   0.3f,   0.07f,  0f,     0f  },
        /*LV 07*/ { 0.3f,   0.3f,   0.3f,   0.1f,   0f,     0f  },
        /*LV 08*/ { 0.24f,  0.3f,   0.3f,   0.15f,  0.01f,  0f  },
        /*LV 09*/ { 0.22f,  0.3f,   0.25f,  0.2f,   0.03f,  0f  },
        /*LV 10*/ { 0.19f,  0.25f,  0.25f,  0.25f,  0.06f,  0f  }
    }; // excludes Legen. - Drop chance handled seperately
    #endregion

    #region Variables
    #endregion
    
    #region Containers
    public List<GameObject> BaseUnitPrefabs;

    private Dictionary<string, Queue<MPUnit>>[] PoolsByRarity;
    #endregion

    #region Unity Methods
    private void Awake() {
        Instance = this;
    }

    private void Start() {
        if (BoltNetwork.IsServer) InitializePools();
    }
    #endregion

    #region Initialization
    private void InitializePools(){
        // Init. the dictionary-array
        int numRarities = Enum.GetNames(typeof(RARITY)).Length;
        PoolsByRarity = new Dictionary<string, Queue<MPUnit>>[numRarities];
        foreach (RARITY rarity in Enum.GetValues(typeof(RARITY)))
            PoolsByRarity[(int) rarity] = new Dictionary<string, Queue<MPUnit>>();
        
        // Create Pools
        foreach (GameObject prefab in BaseUnitPrefabs) {
            MPUnit unit = prefab.GetComponent<MPUnit>();
            MPUnitProperties properties = unit.properties;

            Queue<MPUnit> Pool = new Queue<MPUnit>();
            for (int i = 0; i < POOL_SIZE[(int) properties.quality]; i++) {

            }
        }
    }
    #endregion
}