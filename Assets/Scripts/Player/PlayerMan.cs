using UnityEngine;
using Bolt;

[BoltGlobalBehaviour(BoltNetworkModes.Client)]
public class PlayerMan : GlobalEventListener {

    public static PlayerMan Instance;

    private int _playerID = -1; //can only be set once, through PlayerID
    public int PlayerID {
        get { return _playerID; }
        set {
            if (_playerID == -1) _playerID = value;
            else Debug.LogWarning("Player ID has already been set");
        }
    }

    private void Awake() {
        Instance = this;
    }
}