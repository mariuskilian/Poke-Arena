using UnityEngine;
using Bolt;
using System.Collections.Generic;

[BoltGlobalBehaviour(BoltNetworkModes.Server)]
public class GameMan : GlobalEventListener {

    public static GameMan Instance { get; private set; }

    public GameSettings Settings { get; private set; }
    public GameMode Mode { get; private set; }

    private void Awake() {
        if (Instance == null) Instance = this;
    }

    public override void SceneLoadLocalDone(string scene, IProtocolToken token) {
        Settings = DataHolder.Instance.GameSettings[0];
        Mode = DataHolder.Instance.GameModes[0];
        GameLoadedEvent.Create().Send();
    }

}