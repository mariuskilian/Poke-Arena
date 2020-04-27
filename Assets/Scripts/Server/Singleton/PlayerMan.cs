using UnityEngine;
using Bolt;

[BoltGlobalBehaviour(BoltNetworkModes.Server)]
public class PlayerMan : GlobalEventListener {

    public static PlayerMan Instance { get; private set; }

    public Player[] Players { get; private set; }

    private void Awake() {
        if (Instance == null) Instance = this;
    }

    public override void OnEvent(GameLoadedEvent evnt) {
        Players = new Player[GameMan.Instance.Mode.NumPlayers];
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