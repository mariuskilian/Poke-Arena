using UnityEngine;
using Bolt;
using static GameInfo;

[BoltGlobalBehaviour(BoltNetworkModes.Client)]
public class ClientRoundMan : GlobalEventListener {

    public static ClientRoundMan Instance { get; private set; }
    private void Awake() { if (Instance == null) Instance = this; }

    public Round Round { get; private set; }
    public Phase CurrentPhase { get; private set; }

    public override void EntityReceived(BoltEntity entity) {
        if (!entity.StateIs<IRoundState>()) return;
        Round = entity.GetComponent<Round>();
        Round.RoundInfoUpdatedEvent += HandleRoundInfoUpdatedEvent;
    }

    private void HandleRoundInfoUpdatedEvent(RoundInfo info) {
        CurrentPhase = ArrayOfEnum<Phase>()[info.PhaseID];
    }

}