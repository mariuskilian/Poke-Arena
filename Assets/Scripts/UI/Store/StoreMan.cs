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
    [SerializeField] private int storeSize = 5;
    [SerializeField] private float yOffset = -0.3f;
    [SerializeField] private float zOffset = 175f;
    [SerializeField] private float xOffsetMax = 2f;
    #endregion

    #region Variables
    [SerializeField] private GameObject store = null;
    [SerializeField] private GameObject storeUnits = null;
    private bool forcedHidden = false; // used if a unit is selected while shop is open to hide shop until deselection
    #endregion

    #region Containers
    private Unit[] currentStore;
    #endregion

    #region Events
    public Func<Unit> SpawnRandomUnitEvent;
    public Action<Unit> NewUnitInStoreEvent;
    public Func<Unit, bool> BuyRequestEvent;
    public Action<Unit> UnitBoughtEvent;
    public Action<Unit> DespawnUnitEvent;
    #endregion

    #region Unity Methods (Awake, Start, Update)
    private void Awake() {
        _instance = this; // Singleton
        currentStore = new Unit[storeSize];
    }

    private void Start() {
        InitEventSubscribers();
    }
    
    protected override void LateStart() {
        SpawnNewShop();
    }
    #endregion

    private void InitEventSubscribers() {
        InputMan input = InputMan.Instance;
        input.HideShowShopEvent += HandleHideShowShopEvent;
        input.RerollShopEvent += HandleRerollShopEvent;

        BoardMan board = BoardMan.Instance;
        board.UnitSelectEvent += HandleUnitSelectEvent;
        board.UnitDeselectEvent += HandleUnitDeselectEvent;
    }

    #region Load/Reload Shop
    private void SpawnNewShop() {
        for (int index = 0; index < storeSize; index++) {
            Unit unit = SpawnRandomUnitEvent?.Invoke();
            if (unit != null) {
                unit.transform.SetParent(storeUnits.transform);
                unit.transform.localPosition = GetUnitPosition(index);
                unit.transform.localRotation = Quaternion.Euler(0, 180, 0);
                currentStore[index] = unit;
                NewUnitInStoreEvent?.Invoke(unit);
            }
        }
    }

    private void DespawnCurrentStore() {
        for (int i = 0; i < storeSize; i++) {
            if (currentStore[i] != null) {
                DespawnUnitEvent?.Invoke(currentStore[i]);
                currentStore[i] = null;
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
                for (int i = 0; i < storeSize; i++) {
                    if (currentStore[i] == unit) currentStore[i] = null;
                }
            }
        }
    }
    #endregion

    #region Helper Methods
    //Positions unit accordingly on camera so it shows in store
    private Vector3 GetUnitPosition(int index) {
        if (storeSize == 1) {
            return Vector3.forward * zOffset;
        }
        float x = (((float) index / (float) (storeSize - 1)) * xOffsetMax * 2) - xOffsetMax;
        return Vector3.right * x + Vector3.up * yOffset + Vector3.forward * zOffset;
    }
    #endregion

    #region Event Handlers
    private void HandleHideShowShopEvent() {
        store.SetActive(!store.activeSelf);
    }

    private void HandleRerollShopEvent() {
        DespawnCurrentStore();
        SpawnNewShop();
    }

    private void HandleUnitSelectEvent(Unit unit) {
        if (store.activeSelf) {
            forcedHidden = true;
            store.SetActive(false);
        }
    }

    private void HandleUnitDeselectEvent(Unit unit) {
        if (forcedHidden) {
            forcedHidden = false;
            store.SetActive(true);
        }
    }
    #endregion

    public int GetStoreSize() {
        return storeSize;
    }
}
