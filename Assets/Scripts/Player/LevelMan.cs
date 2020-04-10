using UnityEngine;

public class LevelMan : MonoBehaviour {

    #region Singleton
    private static LevelMan _instance;
    public static LevelMan Instance {
        get {
            if (_instance == null) {
                GameObject go = new GameObject("Level");
                go.AddComponent<LevelMan>();
                Debug.LogWarning("Level Manager instance was null");
            }
            return _instance;
        }
    }
    #endregion

    #region Constants
    public readonly int MAX_LEVEL = 10;
    public readonly int[] MAX_EXP = { 1, 2, 4, 6, 8, 16, 32, 40, 40, 40 };
    private readonly int EXP_PER_BUY = 4;
    #endregion

    public int Level { get; private set; }
    public int Exp { get; private set; }

    private void Awake() {
        _instance = this;
    }

    void Start() {
        InitEventSubscribers();
    }

    private void InitEventSubscribers() {
        FinanceMan finance = FinanceMan.Instance;
        finance.BuyExpEvent += HandleBuyExpEvent;
    }

    private void HandleBuyExpEvent() {
        int maxExp = MAX_EXP[Level];
        if (Exp + EXP_PER_BUY >= maxExp) Level++;
        Exp = (Exp + EXP_PER_BUY) % maxExp;
    }
}
