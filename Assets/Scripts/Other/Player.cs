using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;

public class Player : EntityBehaviour<IPlayerState> {

    public BoltConnection Connection;
    public int PlayerID;

    public void InitPlayer(Transform targetTransform) {
        state.SetTransforms(state.Transform, transform);
        SetPosition(targetTransform);
    }

    private void SetPosition(Transform targetTransform) {
        transform.position = targetTransform.position;
        transform.rotation = targetTransform.rotation;
    }
}
