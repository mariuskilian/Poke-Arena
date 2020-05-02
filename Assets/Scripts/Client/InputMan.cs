using UnityEngine;
using Bolt;
using System;

[BoltGlobalBehaviour(BoltNetworkModes.Client)]
public class InputMan : MonoBehaviour {

    public static InputMan Instance { get; private set; }
    private void Awake() { if (Instance == null) Instance = this; }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.D)) TryRerollStoreEvent?.Invoke();
    }

    #region Local Events
    public Action TryRerollStoreEvent;
    #endregion
}