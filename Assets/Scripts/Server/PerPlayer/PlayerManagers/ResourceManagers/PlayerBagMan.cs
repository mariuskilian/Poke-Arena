using UnityEngine;
using Bolt;
using System;
using static GameInfo;

public class PlayerBagMan : PlayerManager {

    public int PokeballCount { get; private set; } = 0;

    private void Start() { PokeballCount = 100; }

    public bool TryCatchUnit(Rarity rarity) {
        if (PokeballCount <= 0) return false;
        PokeballCount--;
        return RNG.Next(CatchChances[(int)rarity]) == 0;
    }

}