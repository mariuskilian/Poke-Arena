using UnityEngine;
using Bolt;
using System;

public static class GameInfo {

    public static System.Random RNG = new System.Random();

    public enum Rarity { COMMON, UNCOMMON, RARE, EPIC, SECRET, LEGENDARY };
    public static readonly int[] CatchChances = { 1, 2, 3, 4, 5, 10 };

    public enum EvlChain { BASE, MID, TOP }

    // Finance
    public static readonly int[] StreakMilestones = new int[] { 2, 4, 6 };
    public const int
        BaseEarningRound = 5,
        PriceRerollStore = 2,
        PriceBuyExp = 4,
        MaxCoins = 99
        ;

    // Level
    public static readonly int[] ExpUntilNextLevel = { 1, 1, 2, 4, 8, 16, 24, 32, 40 };
    public static readonly int ExpPerBuy = 4;

    public static readonly int NumLevels = ExpUntilNextLevel.Length + 1;
    public static readonly int MaxLevel = NumLevels - 1;
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