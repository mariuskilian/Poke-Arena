using UnityEngine;
using System;

public class FinanceMan : ManagerBehaviour{

    #region Constants
    private readonly int[] STREAK_MILESTONES = { 2, 4, 6 };
    private readonly int
        BASE_EARNING_ROUND = 5,
        REROLL_STORE_PRICE = 2,
        BUY_EXP_PRICE = 4
        ;
    private readonly int MAX_COINS = 99;
    #endregion

    #region Events
    public Action RerollStoreEvent;
    public Action BuyExpEvent;
    #endregion

    public int Coins { get; private set; }
    public int Streak { get; private set; }

    public FinanceMan() {
        Coins = 99;
    }

    private int Interest() {
        return Coins / 10;
    }

    private int StreakBonus() {
        int bonus = 0;
        int _streak = Mathf.Abs(Streak);
        foreach (int milestone in STREAK_MILESTONES)
            if (_streak >= milestone) bonus++;
        return bonus;
    }

    /*

    private void HandleStartOfPhaseEvent(RoundMan.Phase phase) {
        switch (phase) {
            case RoundMan.Phase.START:
                Coins = Mathf.Min(MAX_COINS, Coins + Interest() + StreakBonus() + BASE_EARNING_ROUND);
                break;
            default:
                break;
        }
    }

    private void HandleRoundWonEvent() {
        if (Coins < MAX_COINS) Coins++;
        Streak = Mathf.Max(Streak + 1, 1);
    }

    private void HandleRoundLostEvent() {
        Streak = Mathf.Min(Streak - 1, -1);
    }
    
    private void HandleTryRerollStoreEvent() {
        if (!UIMan.Instance.StoreActive()) return;
        if (Coins >= REROLL_STORE_PRICE) {
            Coins -= REROLL_STORE_PRICE;
            RerollStoreEvent?.Invoke();
        }
    }

    private void HandleTryBuyExpEvent() {
        LevelMan level = LevelMan.Instance;
        if (Coins >= BUY_EXP_PRICE && level.Level < level.MAX_LEVEL) {
            Coins -= BUY_EXP_PRICE;
            BuyExpEvent?.Invoke();
        }
    }

    */
}
