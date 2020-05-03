using UnityEngine;
using System;
using Bolt;

public class StoreButtonMan : GlobalEventListener {

    public static StoreButtonMan Instance { get; private set; }
    private void Awake() { if (Instance == null) Instance = this; }

    #region Local Events
    public Action<int> TryCatchUnitEvent;
    #endregion

    [SerializeField] private GameObject CatchButtonTemplate = null;
    [SerializeField] private int xOffsetMax = 520;

    private CatchUnitButton[] CatchUnitButtons = new CatchUnitButton[PlayerStoreMan.StoreSize];

    private void Start() { InitStoreButtons(); }

    public override void OnEvent(EventManClientInitializedEvent evnt) { SubscribeLocalEventHandlers(); }

    private void InitStoreButtons() {
        for (int idx = 0; idx < CatchUnitButtons.Length; idx++) {
            GameObject buttonGO = Instantiate(CatchButtonTemplate);
            buttonGO.transform.SetParent(transform);

            float xOffset = (((float)idx / (float)(PlayerStoreMan.StoreSize - 1)) * xOffsetMax * 2) - xOffsetMax;
            buttonGO.transform.localPosition = Vector3.right * xOffset;
            buttonGO.transform.localScale = Vector3.one;

            CatchUnitButtons[idx] = buttonGO.GetComponent<CatchUnitButton>();
            CatchUnitButtons[idx].SetStoreIdx(idx);
            DeactivateStoreButton(idx);
        }
    }

    private void ActivateStoreButton(int idx) { CatchUnitButtons[idx].gameObject.SetActive(true); }

    private void DeactivateStoreButton(int idx) { CatchUnitButtons[idx].gameObject.SetActive(false); }

    #region Local Event Handlers
    private void SubscribeLocalEventHandlers() {
        ClientGlobalEventMan global = ClientGlobalEventMan.Instance;
        global.UnitCaughtEvent += HandleUnitCaughtEvent;
        global.NewStoreEvent += HandleNewStoreEvent;

        ClientStoreMan store = ClientStoreMan.Instance;
        store.UnitArrivedInStoreEvent += HandleUnitArrivedInStoreEvent;
    }

    private void HandleUnitArrivedInStoreEvent(int storeIdx) { ActivateStoreButton(storeIdx); }
    private void HandleUnitCaughtEvent(int storeIdx) { DeactivateStoreButton(storeIdx); }
    private void HandleNewStoreEvent(StoreUnit[] _) { for (int idx = 0; idx < PlayerStoreMan.StoreSize; idx++) DeactivateStoreButton(idx); }
    #endregion

}