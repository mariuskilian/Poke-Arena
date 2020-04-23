using UnityEngine;
using TMPro;
using Bolt;

public class PlayerInfoUpdater : GlobalEventListener {

    private IPlayerState playerState;

    [SerializeField] private TextMeshProUGUI
        coinText = null,
        levelText = null,
        expText = null
        ;

    public override void ControlOfEntityGained(BoltEntity entity) {
        IPlayerState state = entity.GetState<IPlayerState>();
        if (state == null) return;

        playerState = state;

        playerState.AddCallback("Coins", UpdateCoins);
        playerState.AddCallback("Exp", UpdateLevelAndExp);
    }

    private void UpdateCoins() {
        coinText.text = playerState.Coins.ToString();
    }

    private void UpdateLevelAndExp() {
        int usrLvl = playerState.Level + 1;
        levelText.text = "Level: " + usrLvl;
        expText.text = "EXP: " + playerState.Exp + "/" + playerState.MaxExp;
    }
}
