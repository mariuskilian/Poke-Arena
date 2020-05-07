using UnityEngine;
using Bolt;
using System;

public class Round : EntityBehaviour<IRoundState> { 

    public override void Attached() {
        if (!BoltNetwork.IsServer) state.AddCallback("RoundInfo", () => RoundInfoUpdatedEvent?.Invoke(state.RoundInfo));
    }

    public Action<RoundInfo> RoundInfoUpdatedEvent;

}