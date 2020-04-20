using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable, CreateAssetMenu(fileName = "GameSettings", menuName = "Poke-Arena/Game Settings")]
public class GameSettings : ScriptableObject {
    public int maxLevel;

    public RarityInfo[] Rarities;

    // 2d array of all drop chances, sorted by [level, rarity] and out of 100
    public Array2D DropChances;

    // List of Prefabs of all Units of Evolution Chain 1
    public List<GameObject> BaseUnitPrefabs;

}