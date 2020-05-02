using UnityEngine;
using System;

public class StoreButtonMan : MonoBehaviour {

    public static StoreButtonMan Instance { get; private set; }
    private void Awake() { if (Instance == null) Instance = this; }

    #region Local Events
    public Action<int> TryBuyUnitEvent;
    #endregion

    [SerializeField] private GameObject CatchButtonTemplate = null;
    [SerializeField] private int xOffsetMax = 520;

    private CatchUnitButton[] CatchUnitButtons = new CatchUnitButton[PlayerStoreMan.StoreSize];

    private void Start() { InitStoreButtons(); SubscribeLocalEventHandlers(); }

    private void InitStoreButtons() {
        for (int idx = 0; idx < CatchUnitButtons.Length; idx++) {
            GameObject buttonGO = Instantiate(CatchButtonTemplate);
            buttonGO.transform.SetParent(transform);

            float xOffset = (((float)idx / (float)(PlayerStoreMan.StoreSize-1)) * xOffsetMax * 2) - xOffsetMax;
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
        ClientStoreMan.Instance.UnitArrivedInStoreEvent += HandleUnitArrivedInStoreEvent;
    }

    private void HandleUnitArrivedInStoreEvent(int idx) { ActivateStoreButton(idx); }
    #endregion

}