using UnityEngine;
using TMPro;
using Bolt;
using static GameInfo;

public class PlayerInfoUpdater : GlobalEventListener {

    [SerializeField]private TextMeshProUGUI
        coinText = null,
        levelText = null,
        expText = null
        ;

    public override void OnEvent(ClientEventManInitializedEvent evnt) { SubscribeLocalEventHandlers(); }

    private void SubscribeLocalEventHandlers() {
        var global = ClientGlobalEventMan.Instance;
        global.PlayerReceivedEvent += HandlePlayerReceivedEvent;
    }

    private void HandlePlayerReceivedEvent(Player player) {
        player.PlayerInfoUpdatedEvent += HandlePlayerInfoUpdatedEvent;
    }

    private void HandlePlayerInfoUpdatedEvent(PlayerInfo info) {
        coinText.text = info.Coins.ToString();
        int usrLvl = info.Progression.Level + 1;
        levelText.text = "Level: " + usrLvl;
        if (info.Progression.Level < MaxLevel) expText.text = "Exp: " + info.Progression.Exp + "/" + ExpUntilNextLevel[usrLvl - 1];
    }

}