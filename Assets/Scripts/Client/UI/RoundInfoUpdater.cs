using UnityEngine;
using TMPro;
using Bolt;
using static GameInfo;

public class RoundInfoUpdater : GlobalEventListener {

    [SerializeField] private TextMeshProUGUI
        timeText = null,
        roundText = null
        ;

    public override void EntityReceived(BoltEntity entity) {
        if (entity.StateIs<IRoundState>()) entity.GetComponent<Round>().RoundInfoUpdatedEvent += HandleRoundInfoUpdatedEvent;
    }

    private void HandleRoundInfoUpdatedEvent(RoundInfo info) {
        timeText.text = info.Time.ToString();
        roundText.text = "Stage " + info.Stage + "-" + info.Round + ", " + ArrayOfEnum<Phase>()[info.PhaseID].ToString();
    }
}