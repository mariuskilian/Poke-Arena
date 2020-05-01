using UnityEngine;
using Bolt;
using System.Collections.Generic;
using System;
using System.Collections;

[BoltGlobalBehaviour(BoltNetworkModes.Server)]
public class GameMan : GlobalEventListener {

    public static GameMan Instance { get; private set; }
    private void Awake() { if (Instance == null) Instance = this; }

    #region Local Events
    public Action GameLoadedEvent;
    public Action AllPlayersLoadedEvent;
    #endregion

    // General
    public GameSettings Settings { get; private set; }
    public GameMode Mode { get; private set; }
    public int LoadStatus { get; private set; }

    // Arenas
    public Arena[] Arenas { get; private set; }
    private readonly int ArenaSize = 20;
    private readonly int SpaceBetweenArenas = 1;

    // Players
    public Player[] Players { get; private set; }
    private Queue<BoltConnection> ConnectionQueue = new Queue<BoltConnection>(); // Used while Players Array has not yet been init.ed

    private void Start() { SubscribeLocalEventHandlers(); }

    public override void SceneLoadLocalDone(string scene, IProtocolToken token) {
        Settings = DataHolder.Instance.GameSettings[0];
        Mode = DataHolder.Instance.GameModes[0];
        InitArenas();
        InitPlayers();
        GameLoadedEvent?.Invoke();
    }

    public override void SceneLoadRemoteDone(BoltConnection connection, IProtocolToken token) {
        ConnectionQueue.Enqueue(connection);
        if (Players != null) ClearQueue();
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
        CheckAllPlayersLoaded();
    }

    private void ClearQueue() {
        // Add all Players from Queue to the Players array
        for (int i = 0; i < Players.Length && ConnectionQueue.Count > 0; i++)
            if (Players[i] == null) Players[i] = InitPlayer(ConnectionQueue.Dequeue());

        // If players array was full, clear the remaining Queue
        while (ConnectionQueue.Count > 0) ConnectionQueue.Dequeue().Disconnect();
    }

    private Player InitPlayer(BoltConnection connection) {
        // Init player
        var playerEntity = BoltNetwork.Instantiate(BoltPrefabs.Player);
        playerEntity.AssignControl(connection);
        Player player = playerEntity.GetComponent<Player>();
        player.Connection = connection;
        foreach (Arena a in Arenas) if (a.TryAddPlayer(player)) break;
        return player;
    }



    private IEnumerator CheckAllPlayersLoaded() {
        LoadStatus = 0;

        // First check, whether all players already connected
        while (true) {
            bool allPlayersConnected = true;
            foreach (Player p in Players) if (p == null) allPlayersConnected = false;
            if (allPlayersConnected) break;
            yield return new WaitForSeconds(0.1f);
        }

        var NewEntities = new Dictionary<NetworkId, BoltEntity>();
        var LoadedEntities = new Dictionary<NetworkId, BoltEntity>();

        // Then check, whether each Player, Arena, Unit, etc. already exist on all players remote connections
        while (true) {

            foreach (var entity in BoltNetwork.Entities) { // Check each entity against each connection
                var key = entity.NetworkId;
                if (LoadedEntities.ContainsKey(key)) continue; // Entity loaded on all connections

                if (NewEntities.TryGetValue(key, out var _entity)) { // Entity not yet loaded on all connections
                    bool existsOnAllRemote = true;
                    foreach (var connection in BoltNetwork.Connections) { // Check entity against all remote connections
                        if (connection.ExistsOnRemote(_entity) != ExistsResult.Yes) { existsOnAllRemote = false; break; }
                    }
                    if (existsOnAllRemote) LoadedEntities.Add(key, _entity);
                } else NewEntities.Add(key, entity); // Entity just loaded on server, hasn't yet been checked against all connections
                    
                if (LoadedEntities.ContainsKey(key)) NewEntities.Remove(key);
            }

            LoadStatus = (LoadedEntities.Count * 100) / (LoadedEntities.Count + NewEntities.Count);

            Debug.Log(LoadStatus);

            if (NewEntities.Count == 0) break; // All players are connected and all entities are loaded

            yield return new WaitForSeconds(0.1f);
        }

        // Fully loaded game
        AllPlayersLoadedEvent?.Invoke();
    }


    #region Local Event Handlers
    private void SubscribeLocalEventHandlers() {
        PoolMan.Instance.PoolsInitDoneEvent += HandlePoolsInitDoneEvent;
    }

    private void HandlePoolsInitDoneEvent() { Debug.Log("Marius: Got it!"); StartCoroutine(CheckAllPlayersLoaded()); }
    #endregion
}