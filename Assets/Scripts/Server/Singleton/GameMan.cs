using UnityEngine;
using Bolt;
using System.Collections.Generic;
using System;

[BoltGlobalBehaviour(BoltNetworkModes.Server)]
public class GameMan : GlobalEventListener {

    public static GameMan Instance { get; private set; }

    #region Local Events
    public Action GameLoadedEvent;
    #endregion

    // General
    public GameSettings Settings { get; private set; }
    public GameMode Mode { get; private set; }

    // Arenas
    public Arena[] Arenas { get; private set; }
    private readonly int ArenaSize = 20;
    private readonly int SpaceBetweenArenas = 1;

    // Players
    private Queue<Player> PlayerQueue = new Queue<Player>(); // Used while Players Array has not yet been init.ed
    public Player[] Players { get; private set; }

    private void Awake() {
        if (Instance == null) Instance = this;
    }

    public override void SceneLoadLocalDone(string scene, IProtocolToken token) {
        Settings = DataHolder.Instance.GameSettings[0];
        Mode = DataHolder.Instance.GameModes[0];
        InitArenas();
        InitPlayers();
        GameLoadedEvent?.Invoke();
    }

    public override void SceneLoadRemoteDone(BoltConnection connection, IProtocolToken token) {
        InitPlayer(connection);
    }

    private void InitArenas() {
        Arenas = new Arena[Mode.NumArenas];
        var arenaLayout = Mode.arenaLayout;
        for (int i = 0; i < arenaLayout.Length; i++) {
            for (int j = 0; j < arenaLayout[i].Length; j++) {
                if (!arenaLayout[i, j].active) continue;
                
                Vector3 pos = (Vector3.right * i + Vector3.forward * j) * (ArenaSize + SpaceBetweenArenas);
                var arenaEntity = BoltNetwork.Instantiate(BoltPrefabs.Arena, pos, Quaternion.identity);
                Arena arena = arenaEntity.GetComponent<Arena>();
                arena.Players = (arenaLayout[i, j].shared) ? new Player[2] : new Player[1];

                for (int k = 0; k < Arenas.Length; k++) if (Arenas[k] == null) { Arenas[k] = arena; break; }
            }
        }
    }

    private void InitPlayers() {
        Players = new Player[Mode.NumPlayers];
        ClearQueue();
    }

    private void ClearQueue() {
        // Add all Players from Queue to the Players array
        for (int i = 0; i < Players.Length && PlayerQueue.Count > 0; i++)
            if (Players[i] == null) Players[i] = PlayerQueue.Dequeue();

        // If players array was full, clear the remaining Queue
        while (PlayerQueue.Count > 0) {
            var player = PlayerQueue.Dequeue();
            player.Connection.Disconnect();
            BoltNetwork.Destroy(player.gameObject);
        }
    }

    private void InitPlayer(BoltConnection connection) {
        // Init player and add to PlayerQueue
        var playerEntity = BoltNetwork.Instantiate(BoltPrefabs.Player);
        Player player = playerEntity.GetComponent<Player>();
        player.Connection = connection;
        foreach (Arena a in Arenas) if (a.TryAddPlayer(player)) break;
        PlayerQueue.Enqueue(player);
        // If Players array is initialized, clear the queue
        if (Players != null) ClearQueue();
    }

}