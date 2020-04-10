using UnityEngine;
using System;

public class FinanceMan : MonoBehaviour {

    #region Singleton
    private static FinanceMan _instance;
    public static FinanceMan Instance {
        get {
            if (_instance == null) { //should NEVER happen
                GameObject go = new GameObject("Finance");
                go.AddComponent<FinanceMan>();
                Debug.LogWarning("Finance Manager instance was null");
            }
            return _instance;
        }
    }
    #endregion

    #region Constants
    private readonly int[] STREAK_MILESTONES = { 2, 4, 6 };
    private readonly int
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

    private void Awake() {
        _instance = this;
    }

    private void Start() {
        InitEventSubscribers();

        Coins = 50;
    }

    private void InitEventSubscribers() {
        InputMan input = InputMan.Instance;
        input.TryRerollStoreEvent += HandleTryRerollStoreEvent;
        input.TryBuyExpEvent += HandleTryBuyExpEvent;
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

    private void HandleStartOfRound() {
        Coins = Mathf.Min(MAX_COINS, Coins + Interest() + StreakBonus());
    }

    private void HandleRoundWon() {
        if (Coins < MAX_COINS) Coins++;
        Streak = Mathf.Max(Streak + 1, 1);
    }

    private void HandleRoundLost() {
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
}
