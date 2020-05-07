using UnityEngine;
using Bolt;
using System;

[BoltGlobalBehaviour(BoltNetworkModes.Client)]
public class ClientGlobalEventMan : GlobalEventListener {

    #region General
    public static ClientGlobalEventMan Instance { get; private set; }
    private void Awake() { if (Instance == null) Instance = this; }

    public override void SceneLoadLocalDone(string scene, Bolt.IProtocolToken token) {
        ClientEventManInitializedEvent.Create(Bolt.GlobalTargets.OnlySelf).Send();
        SubscribeLocalEventHandlers();
    }
    #endregion

    #region Receiving Global Events
    public Action<Player> PlayerReceivedEvent;
    public Action GameStartEvent;
    public Action<StoreUnit[]> NewStoreEvent;
    public Action<int> UnitCaughtEvent;

    public override void ControlOfEntityGained(BoltEntity entity) {
        if (entity.StateIs<IPlayerState>())
            PlayerReceivedEvent?.Invoke(entity.GetComponent<Player>());
    }

    public override void OnEvent(GameStartEvent evnt) { GameStartEvent?.Invoke(); }

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

    public override void OnEvent(StoreUnitCaughtEvent evnt) { UnitCaughtEvent?.Invoke(evnt.StoreIdx); }
    #endregion

    #region Sending Global Events
    private void SubscribeLocalEventHandlers() {
        var UI = UIMan.Instance;
        UI.TryCatchUnitEvent += HandleTryCatchUnitEvent;

        var input = InputMan.Instance;
        input.TryRerollStoreEvent += HandleTryRerollStoreEvent;
        input.TryBuyExpEvent += HandleTryBuyExpEvent;

        var select = SelectionMan.Instance;
        select.UnitDeselectEvent += HandleUnitDeselectEvent;
    }

    private void HandleTryCatchUnitEvent(int idx) {
        var evnt = ClientTryCatchUnitEvent.Create(GlobalTargets.OnlyServer);
        evnt.StoreIdx = idx;
        evnt.Send();
    }

    private void HandleTryRerollStoreEvent() { ClientTryRerollStoreEvent.Create(GlobalTargets.OnlyServer).Send(); }

    private void HandleTryBuyExpEvent() { ClientTryBuyExpEvent.Create(GlobalTargets.OnlyServer).Send(); }

    private void HandleUnitDeselectEvent(BoardUnit unit, Vector3 clickPos, bool clickedBoard) {
        var evnt = ClientUnitDeselectEvent.Create(GlobalTargets.OnlyServer);
        evnt.Unit = unit.entity.NetworkId;
        evnt.ClickPosition = clickPos;
        evnt.ClickedBoard = clickedBoard;
        evnt.Send();
    }
    #endregion

}