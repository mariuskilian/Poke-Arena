using System;
using System.Collections;
using UnityEngine;

public class StoreMan : PlayerManager {

    #region Variables
    #endregion

    #region Helper Variables
    public bool IsLocked { get; private set; }
    #endregion

    #region Containers
    public Unit[] CurrentStore { get; private set; }
    #endregion

    #region Events
    public Func<Unit> SpawnRandomUnitEvent;
    public Func<Unit, bool> BuyRequestEvent;
    public Action<Unit> UnitBoughtEvent;
    public Action<Unit> DespawnUnitEvent;
    #endregion

    protected override void LateStart() {
        InitializeEventHandlers();
        InitializeStore();
    }

    private void InitializeEventHandlers() {
        FinanceMan finance = player.GetManager<FinanceMan>() as FinanceMan;
        finance.RerollStoreEvent += HandleRerollStoreEvent;
    }

    #region Load/Reload Shop
    private void InitializeStore() {
        if (CurrentStore != null) return;
        CurrentStore = new Unit[5];
        SpawnNewStore();
    }

    private void RespawnStore() {
        DespawnCurrentStore();
        SpawnNewStore();
    }

    private void SpawnNewStore() {
        var newStoreEvent = StoreNewStoreEvent.Create(player.connection);
        Unit[] Units = new Unit[5];
        for (int index = 0; index < 5; index++) {
            Unit unit = SpawnRandomUnitEvent?.Invoke();
            if (unit != null) {
                CurrentStore[index] = unit;
                Units[index] = unit;
                Debug.Log("Marius1 " + unit.entity);
                Debug.Log("Marius2 " + Units[index]);
            }
        }
        newStoreEvent.Unit0 = Units[0].entity;
        newStoreEvent.Unit1 = Units[1].entity;
        newStoreEvent.Unit2 = Units[2].entity;
        newStoreEvent.Unit3 = Units[3].entity;
        newStoreEvent.Unit4 = Units[4].entity;
        Debug.Log("Marius3 " + newStoreEvent.Unit0 + Units[0]);
        newStoreEvent.Send();
    }

    private void DespawnCurrentStore() {
        for (int i = 0; i < 5; i++) {
            if (CurrentStore[i] != null) {
                DespawnUnitEvent?.Invoke(CurrentStore[i]);
                CurrentStore[i] = null;
            }
        }
    }
    #endregion

    #region Buying a Unit
    // requests to buy a unit, then buys it if allowed
    public void BuyUnit(Unit unit) {
        if (BuyRequestEvent != null) {
            if (BuyRequestEvent(unit)) {
                UnitBoughtEvent?.Invoke(unit);
                for (int i = 0; i < 5; i++) {
                    if (CurrentStore[i] == unit) CurrentStore[i] = null;
                }
            }
        }
    }
    #endregion

    #region Helper Methods

    public void ToggleLocked() {
        IsLocked = !IsLocked;
    }
    #endregion

    #region Event Handlers
    private void HandleRerollStoreEvent() {
        RespawnStore();
    }

    private void HandleStartOfRoundEvent() {
        if (!IsLocked) RespawnStore();
        IsLocked = false;
    }

    private void HandleToggleLockStoreEvent() {
        IsLocked = !IsLocked;
    }
    #endregion
}
