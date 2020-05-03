using UnityEngine;
using Bolt;
using System;

[BoltGlobalBehaviour(BoltNetworkModes.Client)]
public class ClientGlobalEventMan : GlobalEventListener {

    #region General
    public static ClientGlobalEventMan Instance { get; private set; }
    private void Awake() { if (Instance == null) Instance = this; }

    public override void SceneLoadLocalDone(string scene, Bolt.IProtocolToken token) {
        EventManClientInitializedEvent.Create(Bolt.GlobalTargets.OnlySelf).Send();
        SubscribeLocalEventHandlers();
    }
    #endregion

    #region Receiving Global Events
    public Action<Player> PlayerReceivedEvent;
    public Action<StoreUnit[]> NewStoreEvent;

    public override void ControlOfEntityGained(BoltEntity entity) {
        if (entity.GetState<IPlayerState>() != null)
            PlayerReceivedEvent?.Invoke(entity.GetComponent<Player>());
    }

    public override void OnEvent(StoreNewStoreEvent evnt) {
        StoreUnit[] Units = {
            BoltNetwork.FindEntity(evnt.Unit1).GetComponent<StoreUnit>(),
            BoltNetwork.FindEntity(evnt.Unit2).GetComponent<StoreUnit>(),
            BoltNetwork.FindEntity(evnt.Unit3).GetComponent<StoreUnit>(),
            BoltNetwork.FindEntity(evnt.Unit4).GetComponent<StoreUnit>(),
            BoltNetwork.FindEntity(evnt.Unit5).GetComponent<StoreUnit>()
        };
        NewStoreEvent?.Invoke(Units);
    }
    #endregion

    #region Sending Global Events
    private void SubscribeLocalEventHandlers() {
        StoreButtonMan.Instance.TryBuyUnitEvent += HandleTryBuyUnitEvent;

        InputMan input = InputMan.Instance;
        input.TryRerollStoreEvent += HandleTryRerollStoreEvent;
    }

    private void HandleTryBuyUnitEvent(int idx) {
        var tryBuyUnitEvent = ClientTryBuyUnitEvent.Create(GlobalTargets.OnlyServer);
        tryBuyUnitEvent.StoreIdx = idx;
        tryBuyUnitEvent.Send();
    }

    private void HandleTryRerollStoreEvent() { ClientTryRerollStoreEvent.Create(GlobalTargets.OnlyServer).Send(); }
    #endregion

}