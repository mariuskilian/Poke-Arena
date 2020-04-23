using UnityEngine;
using Bolt;

public class UIMan : Manager {

    #region Singleton
    public static UIMan Instance { get; private set; }
    #endregion

    #region Variables
    [SerializeField] private GameObject store = null;
    private bool forcedHidden = false; // used if a unit is selected while shop is open to hide shop until deselection
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
