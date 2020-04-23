using UnityEngine;

public class LevelMan : PlayerManager {

    #region Constants
    public readonly int MAX_LEVEL = 10;
    public readonly int[] MAX_EXP = { 1, 2, 4, 6, 8, 16, 32, 40, 40, 40 };
    private readonly int EXP_PER_BUY = 4;
    #endregion

    public int Level { get; private set; }
    public int Exp { get; private set; }

    public LevelMan() {

    }

    /*
    private void HandleBuyExpEvent() {
        int maxExp = MAX_EXP[Level];
        if (Exp + EXP_PER_BUY >= maxExp) Level++;
        Exp = (Exp + EXP_PER_BUY) % maxExp;
    }
    */
}
