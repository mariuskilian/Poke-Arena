using UnityEngine;
using Bolt;
using System;

public static class GameInfo {

    public static System.Random RNG = new System.Random();

    public static readonly int NumLevels = 10;
    public static readonly int NumRarities = Enum.GetValues(typeof(Rarity)).Length;

    public enum Rarity { COMMON, UNCOMMON, RARE, EPIC, SECRET, LEGENDARY };
    public static readonly int[] PoolSize = { 45, 30, 25, 15, 10, 1 };

    public static readonly int[,] SpawnRate = {
        /* LVL      COM     UNC     RARE    EPIC    SECR    LEG */
        /* 01 */{   100,    0,      0,      0,      0,      0   },
        /* 02 */{   70,     30,     0,      0,      0,      0   },
        /* 03 */{   60,     35,     5,      0,      0,      0   },
        /* 04 */{   50,     35,     15,     0,      0,      0   },
        /* 05 */{   40,     35,     23,     2,      0,      0   },
        /* 06 */{   33,     30,     30,     7,      0,      0   },
        /* 07 */{   30,     30,     30,     10,     0,      0   },
        /* 08 */{   24,     30,     30,     15,     1,      0   },
        /* 09 */{   22,     30,     25,     20,     3,      0   },
        /* 10 */{   19,     25,     25,     25,     6,      0   }
    };

    public static Unit[] BaseUnitPrefabs;

    public static Rarity[] Rarities {
        get {
            Rarity[] list = new Rarity[Enum.GetValues(typeof(Rarity)).Length];
            int index = 0;
            foreach (Rarity r in Enum.GetValues(typeof(Rarity))) {
                list[index++] = r;
            }
            return list;
        }
    }

}