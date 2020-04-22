using UnityEngine;
using System.Collections.Generic;

public class Unit : MonoBehaviour {

    private Tile tile; //will be used later for attack and so on

    public Unit variant; //variant of unit, usually male/female. null if no variant
    public Unit evolution; //null if highest evolution

    private List<UnitBehaviour> UnitBehaviours;

    public UnitProperties properties;

    protected virtual void Awake() {
        InitUnitBehaviours();
    }

    private void InitUnitBehaviours() {
        UnitBehaviours = new List<UnitBehaviour> {
            gameObject.AddComponent<UnitBoardAnimation>(),
            gameObject.AddComponent<UnitCarryAnimation>(),
            gameObject.AddComponent<UnitStoreAnimation>(),
            gameObject.AddComponent<UnitMovement>(),
            gameObject.AddComponent<UnitShaderEffects>(),
            gameObject.AddComponent<UnitBattleBehaviour>()
        };
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
