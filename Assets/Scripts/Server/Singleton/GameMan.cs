using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;

[BoltGlobalBehaviour(BoltNetworkModes.Server)]
public class GameMan : Manager {

    public static GameMan Instance { get; private set; }
    
    public static System.Random rng = new System.Random();

    public GameSettings settings;

    public Dictionary<BoltConnection, BoltEntity> players;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        players = new Dictionary<BoltConnection, BoltEntity>();
    }

    public override void Connected(BoltConnection connection) {
        var player = BoltNetwork.Instantiate(BoltPrefabs.Player);
        player.AssignControl(connection);
        players.Add(connection, player);

        NewPlayerEvent(player);
    }

    private void NewPlayerEvent(BoltEntity player) {
        var newPlayerEvent = GameNewPlayer.Create();
        newPlayerEvent.Player = player;
        newPlayerEvent.Send();
    }

}