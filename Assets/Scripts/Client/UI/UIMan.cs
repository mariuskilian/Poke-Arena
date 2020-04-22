using UnityEngine;
using Bolt;

[BoltGlobalBehaviour(BoltNetworkModes.Client)]
public class UIMan : Manager {

    #region Singleton
    public static UIMan Instance { get; private set; }
    #endregion

    #region Variables
    [SerializeField] private GameObject store = null;
    private bool forcedHidden = false; // used if a unit is selected while shop is open to hide shop until deselection
    #endregion

    private StoreMan storeMan;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        //storeMan = StoreMan.Instance;
    }

    protected override void LateStart() {
        store.SetActive(false);
    }

    /*
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
    */
}
