using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Bolt;

public class ClientStoreMan : GlobalEventListener {

    public static ClientStoreMan Instance { get; private set; }
    private void Awake() { if (Instance == null) Instance = this; }

    #region Local Events
    public Action<int> UnitArrivedInStoreEvent;
    #endregion

    [SerializeField] private float
        yOffset = 0.3f,
        zOffset = 2f,
        xOffsetMax = 1.8f
        ;

    private StoreUnit[] StoreUnits = new StoreUnit[PlayerStoreMan.StoreSize];

    public override void OnEvent(EventManClientInitializedEvent evnt) { if (evnt.RaisedBy == null) SubscribeLocalEventHandlers(); }

    private void SpawnNewStore(StoreUnit[] Units) {
        for (int idx = 0; idx < Units.Length; idx++) {
            StoreUnits[idx] = Units[idx];
            StartCoroutine(WaitThenSpawn(Units[idx], idx));
        }
    }

    private void DespawnStore() {
        StopAllCoroutines();
        for (int idx = 0; idx < StoreUnits.Length; idx++) {
            StoreUnit storeUnit = StoreUnits[idx];
            if (storeUnit == null) continue;
            storeUnit.ResetUnitPosition();
        }
    }

    private IEnumerator WaitThenSpawn(StoreUnit storeUnit, int index) {
        storeUnit.SetActive(true);
        storeUnit.transform.SetParent(transform);
        storeUnit.transform.localPosition = GetUnitPosition(index);
        storeUnit.transform.localRotation = Quaternion.Euler(0, 180, 0);
        storeUnit.gameObject.transform.Translate(Vector3.up * 10); //to hide unit while waiting
        float normalizedIndex = (float)index / (float)(PlayerStoreMan.StoreSize - 1); // 0 <= normalizedIndex <= 1
        yield return new WaitForSeconds(normalizedIndex * 0.67f);
        storeUnit.gameObject.transform.Translate(Vector3.down * 10); //reshow unit
        UnitArrivedInStoreEvent?.Invoke(index);
    }

    //Positions unit accordingly on camera so it shows in store
    private Vector3 GetUnitPosition(int index) {
        float x = (((float)index / (float)(5 - 1)) * xOffsetMax * 2) - xOffsetMax;
        return Vector3.right * x + Vector3.up * yOffset + Vector3.forward * zOffset;
    }

    #region Local Event Handlers
    private void SubscribeLocalEventHandlers() {
        ClientGlobalEventMan clientMan = ClientGlobalEventMan.Instance;
        clientMan.NewStoreEvent += HandleNewStoreEvent;
    }

    private void HandleNewStoreEvent(StoreUnit[] Units) { DespawnStore(); SpawnNewStore(Units); }
    #endregion

}
