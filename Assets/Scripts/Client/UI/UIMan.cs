using UnityEngine;
using System.Collections;
using System;

public class UIMan : Manager {

    #region Singleton
    public static UIMan Instance { get; private set; }
    #endregion

    #region Constants
    [SerializeField] private float yOffset = 0.378f;
    [SerializeField] private float zOffset = 3f;
    [SerializeField] private float xOffsetMax = 1.8f;
    #endregion

    #region Variables
    [SerializeField] private GameObject
        store = null,
        storeUnitContainer = null
        ;
    private bool forcedHidden = false; // used if a unit is selected while shop is open to hide shop until deselection
    #endregion

    #region Events
    public Action<Unit, int> NewUnitInStoreEvent;
    #endregion

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        InitEventSubscribers();
    }

    protected override void LateStart() {
        store.SetActive(false);
    }

    private void InitEventSubscribers() {
        SelectionMan selection = SelectionMan.Instance;
        selection.UnitSelectEvent += HandleUnitSelectEvent;
        selection.UnitDeselectEvent += HandleUnitDeselectEvent;

        InputMan input = InputMan.Instance;
        input.ToggleStoreEvent += HandleToggleStoreEvent;
    }

    public override void OnEvent(StoreNewStoreEvent evnt) {
        BoltEntity[] UnitEntities = {evnt.Unit0, evnt.Unit1, evnt.Unit2, evnt.Unit3, evnt.Unit4};
        for (int i = 0; i < UnitEntities.Length; i++) {
            StartCoroutine(WaitThenSpawn(UnitEntities[i].GetComponent<Unit>(), i));
        }
    }

    private IEnumerator WaitThenSpawn(Unit unit, int index) {
        unit.gameObject.SetActive(true);
        unit.transform.SetParent(storeUnitContainer.transform);
        unit.transform.localPosition = GetUnitPosition(index);
        unit.transform.localRotation = Quaternion.Euler(0, 180, 0);
        unit.gameObject.transform.Translate(Vector3.up * 1000); //to hide unit while waiting
        float normalizedIndex = (float)index / (float)(5 - 1); // 0 <= normalizedIndex <= 1
        yield return new WaitForSeconds(normalizedIndex * 0.67f);
        unit.gameObject.transform.Translate(Vector3.down * 1000); //reshow unit
        NewUnitInStoreEvent?.Invoke(unit, index);
    }

    //Positions unit accordingly on camera so it shows in store
    private Vector3 GetUnitPosition(int index) {
        float x = (((float)index / (float)(5 - 1)) * xOffsetMax * 2) - xOffsetMax;
        return Vector3.right * x + Vector3.up * yOffset + Vector3.forward * zOffset;
    }
    
    #region Event Handlers
    private void HandleToggleStoreEvent() {
        store.SetActive(!store.activeSelf);
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

    public bool StoreActive() {
        return store.activeSelf;
    }
    #endregion
    
}
