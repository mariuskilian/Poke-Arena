using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;

public class Player : EntityBehaviour<IPlayerState> {

    public BoltConnection Connection;
    public int PlayerID;

    public void InitPlayer() {
        state.SetTransforms(state.Transform, transform);
        SetPosition();
    }

    private void SetPosition() {
        int factor = (true) ? PlayerID : PlayerID / 2;
        int yRotation = (PlayerID % 2 == 0) ? 0 : 180;
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

}
