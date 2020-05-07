using UnityEngine;
using System.Collections;
using System;
using Bolt;

public class UIMan : GlobalEventListener {

    public static UIMan Instance { get; private set; }
    private void Awake() { if (Instance == null) Instance = this; }

    public bool IsStoreActive { get { return store.activeSelf; } }

    [SerializeField] private GameObject store = null;
    [SerializeField] private Camera storeCam = null;    
    [SerializeField] private GameObject CatchButtonTemplate = null;
    [SerializeField] private int xOffsetMax = 520;

    private bool forcedHidden = false; // Used if a unit is selected while shop is open, to hide shop until deselection
    private CatchUnitButton[] CatchUnitButtons = new CatchUnitButton[PlayerStoreMan.StoreSize];

    private void Start() { SetStoreActive(false); InitStoreButtons(); }

    private void SetStoreActive(bool active) { store.SetActive(active); storeCam.enabled = active; }
    
    private void InitStoreButtons() {
        for (int idx = 0; idx < CatchUnitButtons.Length; idx++) {
            GameObject buttonGO = Instantiate(CatchButtonTemplate);
            buttonGO.transform.SetParent(store.transform);

            float xOffset = (((float)idx / (float)(PlayerStoreMan.StoreSize - 1)) * xOffsetMax * 2) - xOffsetMax;
            buttonGO.transform.position = CatchButtonTemplate.transform.position;
            Vector3 pos = buttonGO.transform.localPosition;
            buttonGO.transform.localPosition = new Vector3(xOffset, pos.y, pos.z);
            buttonGO.transform.localScale = Vector3.one;

            CatchUnitButtons[idx] = buttonGO.GetComponent<CatchUnitButton>();
            CatchUnitButtons[idx].SetStoreIdx(idx);
            DeactivateCatchButton(idx);
        }
    }

    private void ActivateCatchButton(int idx) { CatchUnitButtons[idx].gameObject.SetActive(true); }
    private void DeactivateCatchButton(int idx) { CatchUnitButtons[idx].gameObject.SetActive(false); }

    public override void OnEvent(ClientEventManInitializedEvent evnt) { SubscribeLocalEventHandlers(); }

    public Action<int> TryCatchUnitEvent;

    private void SubscribeLocalEventHandlers() {
        var global = ClientGlobalEventMan.Instance;
        global.GameStartEvent += HandleGameStartEvent;
        global.UnitCaughtEvent += HandleUnitCaughtEvent;
        global.NewStoreEvent += HandleNewStoreEvent;

        var selection = SelectionMan.Instance;
        selection.UnitSelectEvent += HandleUnitSelectEvent;
        selection.UnitDeselectEvent += HandleUnitDeselectEvent;

        var input = InputMan.Instance;
        input.ToggleStoreEvent += HandleToggleStoreEvent;

        var store = ClientStoreMan.Instance;
        store.UnitArrivedInStoreEvent += HandleUnitArrivedInStoreEvent;
    }

    private void HandleToggleStoreEvent() { SetStoreActive(!store.activeSelf); }
    private void HandleUnitSelectEvent(BoardUnit _) { if (store.activeSelf) SetStoreActive(!(forcedHidden = true)); }
    private void HandleUnitDeselectEvent(BoardUnit _u, Vector3 _v, bool _b) { if (forcedHidden) SetStoreActive(!(forcedHidden = false)); }
    private void HandleGameStartEvent() { SetStoreActive(true); }
    private void HandleUnitArrivedInStoreEvent(StoreUnit _, int storeIdx) { ActivateCatchButton(storeIdx); }
    private void HandleUnitCaughtEvent(int storeIdx) { DeactivateCatchButton(storeIdx); }
    private void HandleNewStoreEvent(StoreUnit[] _) { for (int idx = 0; idx < PlayerStoreMan.StoreSize; idx++) DeactivateCatchButton(idx); }

}