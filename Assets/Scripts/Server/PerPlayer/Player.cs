using UnityEngine;
using System.Collections.Generic;

public class Player : Bolt.EntityBehaviour<IPlayerState> {

    private List<Manager> Managers;

    public BoltConnection connection;

    public GameObject
        storeUnitContainer,
        team
        ;

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
        state.SetTransforms(state.Transform, transform);
    }

    public Manager GetManager<M>() {
        foreach (Manager m in Managers) if (typeof(M) == m.GetType()) return m;
        return null;
    }

    public void SetPosition() {
        int rotation = 180 * (state.PlayerID % 2);
        transform.localRotation = Quaternion.Euler(0, rotation, 0);
    }

}