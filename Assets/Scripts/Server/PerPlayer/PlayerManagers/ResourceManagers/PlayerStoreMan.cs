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
            UnitDespawnEvent?.Invoke(storeUnit);
        }
    }

    private void CatchUnit(int storeIdx) {
        StoreUnit storeUnit = ActiveStore[storeIdx];
        ActiveStore[storeIdx] = null;

        UnitCaughtEvent?.Invoke(storeUnit);
        var unitCaughtEvent = StoreUnitCaughtEvent.Create(player.Connection);
        unitCaughtEvent.StoreIdx = storeIdx;
        unitCaughtEvent.Send();
    }

    
    
    #region Local Events
    public Action<StoreUnit> UnitDespawnEvent;
    public Action<StoreUnit> UnitCaughtEvent;
    #endregion

    #region Local Event Handlers
    private void SubscribeLocalEventHandlers() {
        GameMan.Instance.StartGameEvent += HandleAllPlayersLoadedEvent;

        var finance = player.GetPlayerMan<PlayerFinanceMan>();
        finance.RerollStoreEvent += HandleRerollStoreEvent;
    }

    private void HandleAllPlayersLoadedEvent() { SpawnNewStore(); }
    private void HandleRerollStoreEvent() { DespawnActiveStore(); SpawnNewStore(); }
    private void HandleUnitCaughtAndSpawnedEvent(BoardUnit unit) {  }
    #endregion

    #region Global Event Handlers
    public override void OnEvent(ClientTryCatchUnitEvent evnt) {
        if (!IsThisPlayer(evnt.RaisedBy)) return;
        StoreUnit storeUnit = ActiveStore[evnt.StoreIdx];
        if (storeUnit == null) return;
        if (!player.GetPlayerMan<PlayerBoardMan>().CanSpawnUnit(storeUnit.boardUnit)) return;
        if (!player.GetPlayerMan<PlayerBagMan>().TryCatchUnit(storeUnit.boardUnit.properties.rarity)) return;
        CatchUnit(evnt.StoreIdx);
    }
    #endregion
}