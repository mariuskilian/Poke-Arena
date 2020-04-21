using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;

[BoltGlobalBehaviour(BoltNetworkModes.Server)]
public class GameMan : ManagerBehaviour {

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
        players.Add(connection, new Player());
    }

}