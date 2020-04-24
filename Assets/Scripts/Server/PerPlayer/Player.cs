using UnityEngine;
using System.Collections.Generic;

public class Player : Bolt.EntityBehaviour<IPlayerState> {

    private List<Manager> Managers;

    public BoltConnection connection;

    private void Awake() {
        if (!BoltNetwork.IsServer) return;

        Managers = new List<Manager> {
            gameObject.AddComponent<FinanceMan>(),
            gameObject.AddComponent<LevelMan>(),
            gameObject.AddComponent<StoreMan>(),
            gameObject.AddComponent<BoardMan>()
        };
    }

    public override void Attached() {

    }

    public Manager GetManager<M>() {
        foreach (Manager m in Managers) if (typeof(M) == m.GetType()) return m;
        return null;
    }

    public bool IsThisPlayer(BoltConnection connection) {
        return this.connection == connection;
    }

}