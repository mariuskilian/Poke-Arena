using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;

[BoltGlobalBehaviour(BoltNetworkModes.Server)]
public class GameMan : Manager {

    public static GameMan Instance { get; private set; }
    
    public static System.Random rng = new System.Random();

    public List<BoltEntity> players;

    private void Awake() {
        Instance = this;
    }

    public override void SceneLoadLocalDone(string scene, Bolt.IProtocolToken token) {
        players = new List<BoltEntity>();
    }

    public override void Connected(BoltConnection connection) {
        var player = BoltNetwork.Instantiate(BoltPrefabs.Player);
        player.AssignControl(connection);
        player.GetState<IPlayerState>().PlayerID = players.Count;
        player.GetComponent<Player>().connection = connection;
        player.GetComponent<Player>().SetPosition();
        players.Add(player);

        NewPlayerEvent(player);
    }

    private void NewPlayerEvent(BoltEntity player) {
        var newPlayerEvent = GameNewPlayerEvent.Create();
        newPlayerEvent.Player = player;
        newPlayerEvent.Send();
    }

}