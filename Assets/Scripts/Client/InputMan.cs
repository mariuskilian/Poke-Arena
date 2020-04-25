using UnityEngine;
using System;
using System.Collections.Generic;
using Bolt;

public class InputMan : Manager {

    #region Singleton
    public static InputMan Instance { get; private set; }
    #endregion

    #region Events
    public Action
        ToggleStoreEvent,
        TryBuyExpEvent,
        SellUnitEvent,
        BenchUnbenchUnitEvent,
        ToggleLockStoreEvent,
        ShowScoreboardEvent,
        HideScoreboardEvent,
        ToggleMenuEvent
        ;

    #endregion

    #region Containers
    private Dictionary<KeyCode, Action> KeyDownBindings, KeyUpBindings;
    #endregion

    private void Awake() {
        Instance = this; //Singleton
    }

    private void Start() {
        InitKeyBindings();
    }

    private new void Update() {
        CheckForInput();
    }

    private void InitKeyBindings() {
        KeyDownBindings = new Dictionary<KeyCode, Action> { //need to store references to actions!!! not copys!!
            { KeyCode.Space, () => ToggleStoreEvent?.Invoke() },
            { KeyCode.D, () => { if (UIMan.Instance.StoreActive()) InputTryRerollStoreEvent.Create().Send(); } },
            { KeyCode.F, () => TryBuyExpEvent?.Invoke() },
            { KeyCode.E, () => SellUnitEvent?.Invoke() },
            { KeyCode.W, () => BenchUnbenchUnitEvent?.Invoke() },
            { KeyCode.L, () => ToggleLockStoreEvent?.Invoke() },
            { KeyCode.Tab, () => ShowScoreboardEvent?.Invoke() },
            { KeyCode.Escape, () => ToggleMenuEvent?.Invoke() }
        };
        KeyUpBindings = new Dictionary<KeyCode, Action> {
            { KeyCode.Tab, HideScoreboardEvent }
        };
    }

    private void CheckForInput() {
        foreach (KeyCode key in KeyDownBindings.Keys) {
            if (Input.GetKeyDown(key)) KeyDownBindings[key]();
        }
        foreach (KeyCode key in KeyUpBindings.Keys) {
            if (Input.GetKeyUp(key)) KeyUpBindings[key]();
        }
    }
}
