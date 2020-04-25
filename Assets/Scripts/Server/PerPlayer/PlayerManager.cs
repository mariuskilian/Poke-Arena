using UnityEngine;

// Intended for those Managers that run only on the server, but have one instance per player
public class PlayerManager : Manager {

    protected Player player;

    private void Awake() {
        player = gameObject.GetComponent<Player>();
    }

    public bool IsThisPlayer(BoltConnection connection) {
        return player.connection == connection;
    }

}