using UnityEngine;
using Bolt;
using System;

public static class GameInfo {

    public static System.Random RNG = new System.Random();

    public enum Rarity { COMMON, UNCOMMON, RARE, EPIC, SECRET, LEGENDARY };
    public enum EvlChain { BASE, MID, TOP }

    public static readonly int NumLevels = 10;
    public static readonly int NumRarities = Enum.GetValues(typeof(Rarity)).Length;

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