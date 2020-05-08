using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;
using System;

public class Player : EntityBehaviour<IPlayerState> {

    public BoltConnection Connection;
    public int PlayerID;

    private List<PlayerManager> PlayerMen;

    public GameObject Team;

    public override void Attached() {
        if (!BoltNetwork.IsServer) state.AddCallback("PlayerInfo", () => PlayerInfoUpdatedEvent?.Invoke(state.PlayerInfo));
    }

    public void InitPlayer(Transform parent) {
        if (!BoltNetwork.IsServer) return;
        transform.SetParent(parent);
        ResetPosition();

        AttachPlayerMen();
    }

    private void AttachPlayerMen() {
        PlayerMen = new List<PlayerManager> {
            gameObject.AddComponent<PlayerStoreMan>(),
            gameObject.AddComponent<PlayerLevelMan>(),
            gameObject.AddComponent<PlayerFinanceMan>(),
            gameObject.AddComponent<PlayerBoardMan>(),
            gameObject.AddComponent<PlayerBagMan>(),
            gameObject.AddComponent<PlayerEvolutionMan>()
        };
    }

    private void ResetPosition() {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public TMan GetPlayerMan<TMan>() where TMan : PlayerManager {
        foreach (PlayerManager pm in PlayerMen)
            if (typeof(TMan) == pm.GetType()) return pm as TMan;
            
        return null;
    }

    public Action<PlayerInfo> PlayerInfoUpdatedEvent;
}