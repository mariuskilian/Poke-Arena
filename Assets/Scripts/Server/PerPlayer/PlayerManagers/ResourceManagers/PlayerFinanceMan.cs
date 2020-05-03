using UnityEngine;
using System;

public class PlayerFinanceMan : PlayerManager {

    private static readonly int[] StreakMilestones = new int[] { 2, 4, 6 };
    private const int
        BaseEarningRound = 5,
        PriceRerollStore = 2,
        PriceBuyExp = 4,
        MaxCoins = 99
        ;
    
    public int Coins { get; private set; }
    public int Streak { get; private set; }

    private bool CanRerollStore { get { return Coins >= PriceRerollStore; } }
    private bool CanBuyExp { get { return Coins >= PriceBuyExp; } }

    public void Start() { Coins = 99; } // DEBUG

    private int Interest() { return Coins / 10; }

    private int StreakBonus() {
        int bonus = 0;
        int streak = Mathf.Abs(Streak);
        foreach (int milestone in StreakMilestones) if (streak >= milestone) bonus++;
        return bonus;
    }



    #region Local Events
    public Action RerollStoreEvent;
    public Action BuyExpEvent;
    #endregion

    #region Global Event Handlers
    public override void OnEvent(ClientTryRerollStoreEvent evnt) {
        if (!IsThisPlayer(evnt.RaisedBy)) return;
        if (CanRerollStore) { Coins -= PriceRerollStore; RerollStoreEvent.Invoke(); }
    }
    #endregion

}