using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;

public class Arena : EntityBehaviour<IArenaState> {

    public GameObject Player1;
    public GameObject Player2;

    public override void Attached() {
        state.SetTransforms(state.Transform, transform);
    }

    public bool TryAddPlayer(Player player) {
        if (Player1 != null) {
            player.InitPlayer(Player1.transform);
            Player1 = null;
            return true;
        }

        if (Player2 != null) {
            player.InitPlayer(Player2.transform);
            Player2 = null;
            return true;
        }

        return false;
    }

}
