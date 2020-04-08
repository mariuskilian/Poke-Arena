using UnityEngine;

public class UIMan : ManagerBehaviour {
    [SerializeField] private GameObject catchButtonContainer = null;
    [SerializeField] private GameObject catchButtonTemplate = null;

    [SerializeField] private float yOffset = -25;
    [SerializeField] private int xOffsetMax = 710;

    private CatchUnitButton[] CatchUnitButtons;

    #region Singleton
    private static UIMan _instance;
    public static UIMan Instance {
        get {
            if (_instance == null) { //should NEVER happen
                GameObject go = new GameObject("User Interface");
                go.AddComponent<UIMan>();
                Debug.LogWarning("UI Manager instance was null");
            }
            return _instance;
        }
    }
    #endregion

    private void Awake() {
        _instance = this;
    }

    private void Start() {
        InitStoreButtons();
        InitEventSubscribers();
    }

    private void InitStoreButtons() {
        CatchUnitButtons = new CatchUnitButton[StoreMan.Instance.GetStoreSize()];
        for (int i = 0; i < CatchUnitButtons.Length; i++) {
            GameObject buttonObject = Instantiate(catchButtonTemplate);
            buttonObject.SetActive(false);
            buttonObject.transform.SetParent(catchButtonContainer.transform);

            if (CatchUnitButtons.Length == 1) {
                buttonObject.transform.localPosition = Vector3.up * yOffset;
            } else {
                float x = (((float) i / (float) (StoreMan.Instance.GetStoreSize() - 1)) * xOffsetMax * 2) - xOffsetMax;
                buttonObject.transform.localPosition = Vector3.right * x + Vector3.up * yOffset;
            }
            buttonObject.transform.localScale = Vector3.one;
            CatchUnitButtons[i] = buttonObject.GetComponent<CatchUnitButton>();
        }
    }

    private void InitEventSubscribers() {
        StoreMan store = StoreMan.Instance;
        store.NewUnitInStoreEvent += HandleNewUnitInStoreEvent;
        store.UnitBoughtEvent += HandleUnitBoughtAndDespawnEvents;
        store.DespawnUnitEvent += HandleUnitBoughtAndDespawnEvents;
    }

    private void ActivateStoreButton(Unit unit) {
        foreach (CatchUnitButton button in CatchUnitButtons) {
            if (!button.gameObject.activeSelf) {
                button.SetUnit(unit);
                break;
            }
        }
    }

    private void DeactivateStoreButtonWithUnitObject(Unit unit) {
        foreach (CatchUnitButton button in CatchUnitButtons) {
            if (button.DoesUnitBelongToButton(unit)) {
                button.DeactivateButton();
            }
        }
    }

    private void HandleNewUnitInStoreEvent(Unit unit) {
        ActivateStoreButton(unit);
    }

    private void HandleUnitBoughtAndDespawnEvents(Unit unit) {
        DeactivateStoreButtonWithUnitObject(unit);
    }
}
