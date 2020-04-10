using System;
using System.Collections;
using UnityEngine;

public class StoreMan : ManagerBehaviour {

    #region Singleton
    private static StoreMan _instance;
    public static StoreMan Instance {
        get {
            if (_instance == null) { //should NEVER happen
                GameObject go = new GameObject("Store");
                go.AddComponent<StoreMan>();
                Debug.LogWarning("Store Manager instance was null");
            }
            return _instance;
        }
    }
    #endregion

    #region Constants
    public int StoreSize { get; private set; } = 5;

    [SerializeField] private float yOffset = -0.3f;
    [SerializeField] private float zOffset = 175f;
    [SerializeField] private float xOffsetMax = 2f;
    #endregion

    #region Variables
    [SerializeField] private GameObject storeUnits = null;
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

    #region Unity Methods (Awake, Start, Update)
    private void Awake() {
        _instance = this; // Singleton
    }

    private void Start() {
        InitEventSubscribers();
    }
    #endregion

    private void InitEventSubscribers() {
        FinanceMan finance = FinanceMan.Instance;
        finance.RerollStoreEvent += HandleRerollStoreEvent;
    }

    #region Load/Reload Shop
    public void InitializeStore() {
        if (CurrentStore != null) return;
        CurrentStore = new Unit[StoreSize];
        SpawnNewShop();
    }

    private void SpawnNewShop() {
        for (int index = 0; index < StoreSize; index++) {
            Unit unit = SpawnRandomUnitEvent?.Invoke();
            if (unit != null) {
                unit.transform.SetParent(storeUnits.transform);
                unit.transform.localPosition = GetUnitPosition(index);
                unit.transform.localRotation = Quaternion.Euler(0, 180, 0);
                CurrentStore[index] = unit;
                NewUnitInStoreEvent?.Invoke(unit, index);
            }
        }
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
        float x = (((float) index / (float) (StoreSize - 1)) * xOffsetMax * 2) - xOffsetMax;
        return Vector3.right * x + Vector3.up * yOffset + Vector3.forward * zOffset;
    }

    public void ToggleIsLocked() {
        IsLocked = !IsLocked;
    }
    #endregion

    #region Event Handlers
    private void HandleRerollStoreEvent() {
        DespawnCurrentStore();
        SpawnNewShop();
    }
    #endregion
}
