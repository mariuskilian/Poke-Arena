using UnityEngine;
using Bolt;

public class StoreMan : PlayerManager {

    public const int StoreSize = 5;

    public Unit[] ActiveStore { get; private set; }
    public bool IsLocked { get; private set; }

    public void Start() {
        ActiveStore = new Unit[StoreSize];
        SubscribeLocalEventHandlers();
    }

    private void SpawnNewStore() {
        int level = player.GetPlayerMan<LevelMan>().Level;
        for (int idx = 0; idx < StoreSize; idx++) {
            Unit unit = PoolMan.Instance.SpawnRandomUnit(level);
            ActiveStore[idx] = unit;
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

            var despawnUnitEvent = StoreDespawnUnitEvent.Create(player.Connection);
            despawnUnitEvent.Unit = ActiveStore[i].entity.NetworkId;
            despawnUnitEvent.Send();

            ActiveStore[i] = null;
        }
    }

    #region Local Event Handlers
    private void SubscribeLocalEventHandlers() {
        GameMan.Instance.AllPlayersLoadedEvent += HandleAllPlayersLoadedEvent;
    }

    private void HandleAllPlayersLoadedEvent() { SpawnNewStore(); }
    #endregion
}