using System;
using System.Collections;
using UnityEngine;

public class StoreMan : ManagerBehaviour {

    #region Constants
    public int StoreSize { get; private set; } = 5;

    [SerializeField] private float yOffset = 0.378f;
    [SerializeField] private float zOffset = 3f;
    [SerializeField] private float xOffsetMax = 1.8f;
    #endregion

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
    public Action<Unit, int> NewUnitInStoreEvent;
    public Func<Unit, bool> BuyRequestEvent;
    public Action<Unit> UnitBoughtEvent;
    public Action<Unit> DespawnUnitEvent;
    #endregion

    #region Unity Methods
    #endregion

    #region Load/Reload Shop
    public void InitializeStore() {
        if (CurrentStore != null) return;
        CurrentStore = new Unit[StoreSize];
        SpawnNewStore();
    }

    private void RespawnStore() {
        DespawnCurrentStore();
        SpawnNewStore();
    }

    private void SpawnNewStore() {
        for (int index = 0; index < StoreSize; index++) {
            Unit unit = SpawnRandomUnitEvent?.Invoke();
            if (unit != null) {
                StartCoroutine(WaitThenSpawn(unit, index));
            }
        }
    }

    private IEnumerator WaitThenSpawn(Unit unit, int index) {
        unit.transform.SetParent(transform);
        unit.transform.localPosition = GetUnitPosition(index);
        unit.transform.localRotation = Quaternion.Euler(0, 180, 0);
        CurrentStore[index] = unit;
        unit.gameObject.transform.Translate(Vector3.up * 1000); //to hide unit while waiting
        float normalizedIndex = (float)index / (float)(StoreSize - 1); // 0 <= normalizedIndex <= 1
        yield return new WaitForSeconds(normalizedIndex * 0.67f);
        unit.gameObject.transform.Translate(Vector3.down * 1000); //reshow unit
        NewUnitInStoreEvent?.Invoke(unit, index);
    }

    private void DespawnCurrentStore() {
        for (int i = 0; i < StoreSize; i++) {
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
                for (int i = 0; i < StoreSize; i++) {
                    if (CurrentStore[i] == unit) CurrentStore[i] = null;
                }
            }
        }
    }
    #endregion

    #region Helper Methods
    //Positions unit accordingly on camera so it shows in store
    private Vector3 GetUnitPosition(int index) {
        if (StoreSize == 1) {
            return Vector3.forward * zOffset;
        }
        float x = (((float)index / (float)(StoreSize - 1)) * xOffsetMax * 2) - xOffsetMax;
        return Vector3.right * x + Vector3.up * yOffset + Vector3.forward * zOffset;
    }

    public void ToggleLocked() {
        IsLocked = !IsLocked;
    }
    #endregion

    /*
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
    */
}
