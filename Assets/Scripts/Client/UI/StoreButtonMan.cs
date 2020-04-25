using UnityEngine;
using Bolt;

public class StoreButtonMan : Manager {
    [SerializeField] private GameObject catchButtonTemplate = null;

    [SerializeField] private float yOffset = -25;
    [SerializeField] private int xOffsetMax = 710;

    private CatchUnitButton[] CatchUnitButtons;

    #region Singleton
    public static StoreButtonMan Instance { get; private set; }
    #endregion

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        InitStoreButtons();
    }

    private void InitStoreButtons() {
        CatchUnitButtons = new CatchUnitButton[5]; 
        for (int i = 0; i < CatchUnitButtons.Length; i++) {
            GameObject buttonObject = Instantiate(catchButtonTemplate);
            buttonObject.SetActive(false);
            buttonObject.transform.SetParent(transform);

            if (CatchUnitButtons.Length == 1) {
                buttonObject.transform.localPosition = Vector3.up * yOffset;
            } else {
                float x = 0; //float x = (((float)i / (float)(StoreMan.Instance.StoreSize - 1)) * xOffsetMax * 2) - xOffsetMax;
                buttonObject.transform.localPosition = Vector3.right * x + Vector3.up * yOffset;
            }
            buttonObject.transform.localScale = Vector3.one;
            CatchUnitButtons[i] = buttonObject.GetComponent<CatchUnitButton>();
        }
    }

    private void ActivateStoreButton(Unit unit, int index) {
        CatchUnitButtons[index].SetUnit(unit);
    }

    private void DeactivateStoreButtonWithUnitObject(Unit unit) {
        foreach (CatchUnitButton button in CatchUnitButtons) {
            if (button.BelongsToUnit(unit)) {
                button.DeactivateButton();
            }
        }
    }

    /*
    private void HandleNewUnitInStoreEvent(Unit unit, int index) {
        ActivateStoreButton(unit, index);
    }

    private void HandleUnitBoughtAndDespawnEvents(Unit unit) {
        DeactivateStoreButtonWithUnitObject(unit);
    }
    */
}
