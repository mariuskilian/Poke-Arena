using UnityEngine;
using System.Collections;
using System;
using Bolt;

public class UIMan : GlobalEventListener {

    public static UIMan Instance { get; private set; }
    private void Awake() { if (Instance == null) Instance = this; }

    [SerializeField] private GameObject store = null;
    [SerializeField] private Camera storeCam = null;
    private bool forcedHidden = false; // Used if a unit is selected while shop is open, to hide shop until deselection

    private void SetStoreActive(bool active) { store.SetActive(active); storeCam.enabled = active; }

    public override void OnEvent(ClientEventManInitializedEvent evnt) { SubscribeLocalEventHandlers(); }

    private void SubscribeLocalEventHandlers() {
        SelectionMan selection = SelectionMan.Instance;
        selection.UnitSelectEvent += HandleUnitSelectEvent;
        selection.UnitDeselectEvent += HandleUnitDeselectEvent;

        InputMan input = InputMan.Instance;
        input.ToggleStoreEvent += HandleToggleStoreEvent;
    }

    private void HandleToggleStoreEvent() { SetStoreActive(!store.activeSelf); }
    private void HandleUnitSelectEvent(Unit _) { if (store.activeSelf) SetStoreActive(!(forcedHidden = true)); }
    private void HandleUnitDeselectEvent(Unit _u, Vector3 _v, bool _b) { if (forcedHidden) SetStoreActive(!(forcedHidden = false)); }

}