using UnityEngine;
using TMPro;
using Bolt;
using static GameInfo;

public class RoundInfoUpdater : GlobalEventListener {

    [SerializeField] private TextMeshProUGUI
        timeText = null,
        roundText = null
        ;

    private void Start() { timeText.text = "0"; roundText.text = "Loading Game"; }
    
    public override void OnEvent(ClientEventManInitializedEvent evnt) { SubscribeLocalEventHandlers(); }

    private void SubscribeLocalEventHandlers() {
        var global = ClientGlobalEventMan.Instance;
        global.GameStartEvent += HandleGameStartEvent;
    }

    private void HandleGameStartEvent() {
        ClientRoundMan.Instance.Round.RoundInfoUpdatedEvent += HandleRoundInfoUpdatedEvent;
    }

    private void HandleRoundInfoUpdatedEvent(RoundInfo info) {
        timeText.text = info.Time.ToString();
        roundText.text = "Stage " + info.Stage + "-" + info.Round + ", " + ArrayOfEnum<Phase>()[info.PhaseID].ToString();
    }
}