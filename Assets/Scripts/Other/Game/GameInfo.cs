using UnityEngine;
using Bolt;
using System;
using System.Collections.Generic;

public static class GameInfo {

    public static System.Random RNG = new System.Random();

    public static T[] ArrayOfEnum<T>() where T : Enum {
        T[] list = new T[Enum.GetValues(typeof(T)).Length];
        int idx = 0;
        foreach (T t in Enum.GetValues(typeof(T))) list[idx++] = t;
        return list;
    }

    // Units
    public enum Rarity { COMMON, UNCOMMON, RARE, EPIC, SECRET, LEGENDARY };
    public static readonly int[] CatchChances = { 1, 2, 3, 4, 5, 10 };
    public enum EvlChain { BASE, MID, TOP }

    // Round
    public enum RoundType { Player, NPC, Trainer };
    public enum Phase { Start, Setup, Prepare, Battle, Overtime, End }
    public static readonly int[] PhaseTimes = { 1, 30, 5, 30, 10, 3 };
    public static readonly RoundType[]
        FirstStage = { RoundType.NPC, RoundType.NPC, RoundType.NPC, RoundType.Trainer },
        Stage = { RoundType.NPC, RoundType.Player, RoundType.Player, RoundType.Player, RoundType.Player, RoundType.Player, RoundType.Trainer }
        ;

    // Finance
    public static readonly int[] StreakMilestones = new int[] { 2, 4, 6 };
    public const int BaseEarningRound = 5, PriceRerollStore = 2, PriceBuyExp = 4, MaxCoins = 99;

    // Level
    public static readonly int[] ExpUntilNextLevel = { 1, 1, 2, 4, 8, 16, 24, 32, 40 };
    public static readonly int ExpPerBuy = 4;

    // General
    public static readonly int NumLevels = ExpUntilNextLevel.Length + 1;
    public static readonly int MaxLevel = NumLevels - 1;
    public static readonly int NumRarities = Enum.GetValues(typeof(Rarity)).Length;

}