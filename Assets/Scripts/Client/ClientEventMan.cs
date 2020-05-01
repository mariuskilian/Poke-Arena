using UnityEngine;
using Bolt;
using System;

[BoltGlobalBehaviour(BoltNetworkModes.Client)]
public class ClientEventMan : GlobalEventListener {

    public static ClientEventMan Instance { get; private set; }
    private void Awake() { if (Instance == null) Instance = this; }

    public override void SceneLoadLocalDone(string scene, Bolt.IProtocolToken token) { EventManClientInitializedEvent.Create(Bolt.GlobalTargets.OnlySelf).Send(); }

    #region Local Events
    public Action<Player> PlayerReceivedEvent;
    public Action<Unit[]> NewStoreEvent;
    #endregion

    public override void ControlOfEntityGained(BoltEntity entity) {
        if (entity.GetState<IPlayerState>() != null)
            PlayerReceivedEvent?.Invoke(entity.GetComponent<Player>());
    }

    public override void OnEvent(StoreNewStoreEvent evnt) {
        BoltLog.Info("Marius: Event received!");
        Unit[] Units = {
            BoltNetwork.FindEntity(evnt.Unit1).GetComponent<Unit>(),
            BoltNetwork.FindEntity(evnt.Unit2).GetComponent<Unit>(),
            BoltNetwork.FindEntity(evnt.Unit3).GetComponent<Unit>(),
            BoltNetwork.FindEntity(evnt.Unit4).GetComponent<Unit>(),
            BoltNetwork.FindEntity(evnt.Unit5).GetComponent<Unit>()
        };
        NewStoreEvent?.Invoke(Units);
    }

}