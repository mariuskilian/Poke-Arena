﻿using UnityEngine;
using System;
using System.Collections.Generic;

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

    #region Events
    public Action
        ToggleStoreEvent,
        TryRerollStoreEvent,
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
        _instance = this; //Singleton
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
            { KeyCode.D, () => TryRerollStoreEvent?.Invoke() },
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
