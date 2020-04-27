using UnityEngine;
using Bolt;

[BoltGlobalBehaviour(BoltNetworkModes.Server)]
public class PlayerMan : ServerManager {

    public Player[] Players { get; private set; }

    public override void OnEvent(GameLoadedEvent evnt) {
        Game = GameMan.Instance as GameMan;
        Players = new Player[Game.Mode.NumPlayers];
    }

    public override void Connected(BoltConnection connection) {
        var playerEntity = BoltNetwork.Instantiate(BoltPrefabs.Player);
        Player player = playerEntity.GetComponent<Player>();
        player.Connection = connection;
        player.InitPlayer();
        for (int i = 0; i < Players.Length; i++)
            if (!Players[i]) { Players[i] = player; break; }
    }

}