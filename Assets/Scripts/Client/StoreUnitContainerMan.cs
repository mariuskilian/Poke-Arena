using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;

public class StoreUnitContainerMan : GlobalEventListener {

    [SerializeField] private float
        yOffset = 0.5f,
        zOffset = 3f,
        xOffsetMax = 4f
        ;

    public static StoreUnitContainerMan Instance { get; private set; }
    private void Awake() { if (Instance == null) Instance = this; }

    public override void OnEvent(EventManClientInitializedEvent evnt) { if (evnt.RaisedBy == null) SubscribeLocalEventHandlers(); }

    private void PositionNewStore(Unit[] Units) {
        for (int idx = 0; idx < Units.Length; idx++) {
            StartCoroutine(WaitThenSpawn(Units[idx], idx));
        }
    }

    private IEnumerator WaitThenSpawn(Unit unit, int index) {
        unit.gameObject.SetActive(true);
        unit.transform.SetParent(transform);
        unit.transform.localPosition = GetUnitPosition(index);
        unit.transform.localRotation = Quaternion.Euler(0, 180, 0);
        unit.gameObject.transform.Translate(Vector3.up * 1000); //to hide unit while waiting
        float normalizedIndex = (float)index / (float)(5 - 1); // 0 <= normalizedIndex <= 1
        yield return new WaitForSeconds(normalizedIndex * 0.67f);
        unit.gameObject.transform.Translate(Vector3.down * 1000); //reshow unit
        // NewUnitInStoreEvent?.Invoke(unit, index);
    }

    //Positions unit accordingly on camera so it shows in store
    private Vector3 GetUnitPosition(int index) {
        float x = (((float)index / (float)(5 - 1)) * xOffsetMax * 2) - xOffsetMax;
        return Vector3.right * x + Vector3.up * yOffset + Vector3.forward * zOffset;
    }

    #region Local Event Handlers
    private void SubscribeLocalEventHandlers() {
        ClientEventMan clientMan = ClientEventMan.Instance;
        clientMan.NewStoreEvent += HandleNewStoreEvent;
    }

    private void HandleNewStoreEvent(Unit[] Units) { PositionNewStore(Units); }
    #endregion

}
