using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;

[BoltGlobalBehaviour(BoltNetworkModes.Server)]
public class GameMan : Manager {

    public static GameMan Instance { get; private set; }
    
    public static System.Random rng = new System.Random();

    public GameSettings settings;

    public Dictionary<BoltConnection, Player> players;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        players = new Dictionary<BoltConnection, Player>();
    }

    public override void Connected(BoltConnection connection) {
        var playerEntity = BoltNetwork.Instantiate(BoltPrefabs.Player);

        var newPlayerEvent = GameNewPlayer.Create();
        newPlayerEvent.Player = playerEntity;
        newPlayerEvent.Send();

        players.Add(connection, playerEntity.GetComponent<Player>());
    }

}