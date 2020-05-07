using UnityEngine;
using System.Collections;
using static GameInfo;

public class PlayerLevelMan : PlayerManager {

    public int Level {
        get { return player.state.PlayerInfo.Progression.Level; }
        private set { player.state.PlayerInfo.Progression.Level = Mathf.Clamp(value, 0, MaxLevel); }
    }
    public int Exp {
        get { return player.state.PlayerInfo.Progression.Exp; }
        private set { player.state.PlayerInfo.Progression.Exp = value; }
    }

    private void Start() { Level = 1; Exp = 1; Level = 0; Exp = 0; }

    public override void OnEvent(ClientTryBuyExpEvent evnt) {
        if (!IsThisPlayer(evnt.RaisedBy)) return;
        if (Level == MaxLevel) return;
        if (!player.GetPlayerMan<PlayerFinanceMan>().TryBuyExp()) return;
        AddExp(ExpPerBuy);
    }

    // System to add exp one at a time
    private void AddExp(int amount) {
        int expQueueSize = _expQueue;
        _expQueue += amount;
        if (expQueueSize == 0) { StopAllCoroutines(); StartCoroutine(AddExp()); }
    }
    private int _expQueue = 0;
    private IEnumerator AddExp() {
        while (_expQueue > 0) {
            Exp++;
            if (Exp >= ExpUntilNextLevel[Level]) Exp -= ExpUntilNextLevel[Level++];
            yield return new WaitForSeconds(0.05f);
            _expQueue--;
        }
    }

}