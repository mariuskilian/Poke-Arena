using UnityEngine;
using System;

public class InputMan : ManagerBehaviour {

    #region Singleton
    private static InputMan _instance;
    public static InputMan Instance {
        get {
            if (_instance == null) {
                GameObject go = new GameObject("Input Manager");
                go.AddComponent<InputMan>();
                Debug.LogWarning("Input Manager instance was null");
            }
            return _instance;
        }
    }
    #endregion

    #region Key Bindings and Events
    private readonly KeyCode hideShowShop = KeyCode.Space;
    public Action ToggleStoreEvent;

    private readonly KeyCode rerollShop = KeyCode.D;
    public Action TryRerollStoreEvent;

    private readonly KeyCode buyExp = KeyCode.F;
    public Action TryBuyExpEvent;

    private readonly KeyCode sellUnit = KeyCode.E;
    public Action SellUnitEvent;

    private readonly KeyCode benchUnbenchUnit = KeyCode.W;
    public Action BenchUnbenchUnitEvent;

    private readonly KeyCode showScoreboard = KeyCode.Tab;
    public Action ShowScoreboardEvent;
    public Action HideScoreboardEvent;

    private readonly KeyCode showHideMenu = KeyCode.Escape;
    public Action ShowHideMenuEvent;
    #endregion

    private void Awake() {
        _instance = this; //Singleton
    }

    private new void Update() {
        CheckForInput();
    }

    private void CheckForInput() {
        if (Input.GetKeyDown(hideShowShop)) ToggleStoreEvent?.Invoke();

        if (Input.GetKeyDown(rerollShop)) TryRerollStoreEvent?.Invoke();

        if (Input.GetKeyDown(buyExp)) TryBuyExpEvent?.Invoke();

        if (Input.GetKeyDown(sellUnit)) SellUnitEvent?.Invoke();

        if (Input.GetKeyDown(benchUnbenchUnit)) BenchUnbenchUnitEvent?.Invoke();

        if (Input.GetKeyDown(showScoreboard)) ShowScoreboardEvent?.Invoke();
        if (Input.GetKeyUp(showScoreboard)) ShowScoreboardEvent?.Invoke();

        if (Input.GetKeyDown(showHideMenu)) ShowHideMenuEvent?.Invoke();
    }
}
