using UnityEngine;
using System.Collections.Generic;
using Bolt;

public class Unit : EntityBehaviour<IUnitState> {

    private Tile tile; //current tile the unit is standing on

    public Unit variant; //variant of unit, usually male/female. null if no variant
    public Unit evolution; //null if highest evolution

    private List<UnitBehaviour> UnitBehaviours;

    public UnitProperties properties;

    private void Awake() {
        if (BoltNetwork.IsServer) {
            InitUnitServerBehaviours();
        }
        if (BoltNetwork.IsClient && entity.HasControl) {
            InitUnitClientBehaviours();
            AddCollisionPlane();
        }
    }

    private void InitUnitServerBehaviours() {
        UnitBehaviours = new List<UnitBehaviour> {
            gameObject.AddComponent<UnitBoardAnimation>(),
            gameObject.AddComponent<UnitCarryAnimation>(),
            gameObject.AddComponent<UnitStoreAnimation>(),
            gameObject.AddComponent<UnitMovement>(),
            gameObject.AddComponent<UnitSelection>(),
            gameObject.AddComponent<UnitShaderEffects>(),
            gameObject.AddComponent<UnitBattleBehaviour>()
        };
    }

    private void InitUnitClientBehaviours() {
        UnitBehaviours = new List<UnitBehaviour> {
            gameObject.AddComponent<UnitMovement>(),
            gameObject.AddComponent<UnitSelection>()
        };
    }

    private void AddCollisionPlane() {
        BoxCollider collisionPlane = gameObject.AddComponent<BoxCollider>();
        collisionPlane.size = new Vector3(0.9f, 0f, 0.9f);
    }

    public enum Evl_Chain { One, Two, Three }; //number represents how far down in evolution chain (one is base, two is first evo, etc.)
    public Evl_Chain evl_chain;

    public void UpdateTile(Tile tile) {
        this.tile = tile;
    }

    public bool IsBenched() {
        return tile.GetTilePosition().y == -1;
    }

    public bool IsInStore() {
        return tile == null;
    }

    public Tile GetTile() {
        return tile;
    }

    public UnitBehaviour GetUnitBehaviour<UB>() {
        foreach (UnitBehaviour ub in UnitBehaviours) if (typeof(UB) == ub.GetType()) return ub;
        return null;
    }
}
