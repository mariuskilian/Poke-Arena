using UnityEngine;
using System;
using static GameInfo;

public class PlayerFinanceMan : PlayerManager {

    public int Coins {
        get { return player.state.PlayerInfo.Coins; }
        private set { player.state.PlayerInfo.Coins = value; }
    }
    public int Streak { get; private set; }

    private bool CanRerollStore { get { return Coins >= PriceRerollStore; } }
    private bool CanBuyExp { get { return Coins >= PriceBuyExp; } }

    public void Start() { Coins = -1; Coins = 99; } // DEBUG

    private int Interest() { return Coins / 10; }

    private int StreakBonus() {
        int bonus = 0;
        int streak = Mathf.Abs(Streak);
        foreach (int milestone in StreakMilestones) if (streak >= milestone) bonus++;
        return bonus;
    }

    public bool TryBuyExp() {
        if (CanBuyExp) { Coins -= PriceBuyExp; return true; }
        return false;
    }

    #region Local Events
    public Action RerollStoreEvent;
    #endregion

    #region Global Event Handlers
    public override void OnEvent(ClientTryRerollStoreEvent evnt) {
        if (!IsThisPlayer(evnt.RaisedBy)) return;
        if (CanRerollStore) { Coins -= PriceRerollStore; RerollStoreEvent?.Invoke(); }
    }
    #endregion

}