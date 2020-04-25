using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable, CreateAssetMenu(fileName = "GameSettings", menuName = "Poke-Arena/Game Settings")]
public class GameSettingsSO : ScriptableObject {

    public enum Rarity { COMMON, UNCOMMON, RARE, EPIC, SECRET, LEGENDARY }

    public int maxLevel;
    public RarityInfo[] RarityInfos;

    // 2d array of all drop chances, sorted by [level, rarity] and out of 100
    public Array2D DropChances;

    // List of Prefabs of all Units of Evolution Chain 1
    public List<GameObject> BaseUnitPrefabs;

    public int GetDropChance(int level, Rarity rarity) {
        return DropChances[level, GetRarityIndex(rarity)];
    }

    public int GetRarityIndex(Rarity rarity) {
        int rarIdx = 0;
        foreach (RarityInfo ri in RarityInfos) {
            if (ri.rarity == rarity) break;
            rarIdx++;
        }
        return (rarIdx == RarityInfos.Length) ? -1 : rarIdx;
    }

}