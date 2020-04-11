using UnityEngine;

public class UIMan : ManagerBehaviour {

    #region Singleton
    private static UIMan _instance;
    public static UIMan Instance {
        get {
            if (_instance == null) { //should NEVER happen
                GameObject go = new GameObject("UI");
                go.AddComponent<UIMan>();
                Debug.LogWarning("UI Manager instance was null");
            }
            return _instance;
        }
    }
    #endregion

    #region Variables
    [SerializeField] private GameObject store = null;
    private bool forcedHidden = false; // used if a unit is selected while shop is open to hide shop until deselection
    #endregion

    private StoreMan storeMan;

    private void Awake() {
        _instance = this;
    }

    private void Start() {
        storeMan = StoreMan.Instance;
        InitEventSubscribers();
        Debug.Log("UI Subscribed");
    }
    
    protected override void LateStart() {
        store.SetActive(false);
    }

    private void InitEventSubscribers() {
        InputMan input = InputMan.Instance;
        input.ToggleStoreEvent += HandleToggleStoreEvent;

        BoardMan board = BoardMan.Instance;
        board.UnitSelectEvent += HandleUnitSelectEvent;
        board.UnitDeselectEvent += HandleUnitDeselectEvent;
    }

    #region Event Handlers
    private void HandleToggleStoreEvent() {
        store.SetActive(!store.activeSelf);
        if (storeMan.CurrentStore == null) storeMan.InitializeStore();
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
