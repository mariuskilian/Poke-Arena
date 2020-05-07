using UnityEngine;
using Bolt;

public class PlayerManager : GlobalEventListener {
    protected Player player;

    protected void Awake() { player = GetComponent<Player>(); }

    public bool IsThisPlayer(BoltConnection connection) {
        return player.Connection == connection;
    }
}