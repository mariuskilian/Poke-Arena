using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;

public class Player : EntityBehaviour<IPlayerState> {

    public BoltConnection Connection;
    public int PlayerID;

    public void InitPlayer(Transform parent) {
        if (!BoltNetwork.IsServer) return;
        state.SetTransforms(state.Transform, transform);
        transform.SetParent(parent);
        ResetPosition();

        var playerSpawnedEvent = PlayerSpawnedEvent.Create(Connection);
        playerSpawnedEvent.PlayerNetID = entity.NetworkId;
        playerSpawnedEvent.Send();
    }

    private void ResetPosition() {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}
