using UnityEngine;
using Bolt;
using System;

public class PlayerStoreMan : PlayerManager {

    public const int StoreSize = 5;

    public StoreUnit[] ActiveStore { get; private set; }
    public bool IsLocked { get; private set; }

    private void Start() { ActiveStore = new StoreUnit[StoreSize]; SubscribeLocalEventHandlers(); }

    private void SpawnNewStore() {
        int level = player.GetPlayerMan<PlayerLevelMan>().Level;
        for (int idx = 0; idx < StoreSize; idx++) {
            StoreUnit storeUnit = PoolMan.Instance.SpawnRandomUnit(level);
            ActiveStore[idx] = storeUnit;
        }

        var newStoreEvent = StoreNewStoreEvent.Create(player.Connection);
        newStoreEvent.Unit1 = ActiveStore[0].entity.NetworkId;
        newStoreEvent.Unit2 = ActiveStore[1].entity.NetworkId;
        newStoreEvent.Unit3 = ActiveStore[2].entity.NetworkId;
        newStoreEvent.Unit4 = ActiveStore[3].entity.NetworkId;
        newStoreEvent.Unit5 = ActiveStore[4].entity.NetworkId;
        newStoreEvent.Send();
    }

    private void DespawnActiveStore() {
        for (int i = 0; i < StoreSize; i++) {
            if (ActiveStore[i] == null) continue;
            StoreUnit storeUnit = ActiveStore[i];
            ActiveStore[i] = null;
            DespawnUnitEvent?.Invoke(storeUnit);
        }
    }

    
    
    #region Local Events
    public Action<StoreUnit> TryBuyUnitEvent;
    public Action<StoreUnit> DespawnUnitEvent;
    #endregion

    #region Local Event Handlers
    private void SubscribeLocalEventHandlers() {
        GameMan.Instance.AllPlayersLoadedEvent += HandleAllPlayersLoadedEvent;
        player.GetPlayerMan<PlayerFinanceMan>().RerollStoreEvent += HandleRerollStoreEvent;
    }

    private void HandleAllPlayersLoadedEvent() { SpawnNewStore(); }
    private void HandleRerollStoreEvent() { DespawnActiveStore(); SpawnNewStore(); }
    #endregion

    #region Global Event Handlers
    public override void OnEvent(ClientTryBuyUnitEvent evnt) {
        if (!IsThisPlayer(evnt.RaisedBy)) return;
        TryBuyUnitEvent?.Invoke(ActiveStore[evnt.StoreIdx]);
    }
    #endregion
}