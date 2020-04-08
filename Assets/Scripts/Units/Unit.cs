using UnityEngine;

public abstract class Unit : MonoBehaviour{

    private Tile tile; //will be used later for attack and so on

    public Unit variant; //variant of unit, usually male/female. null if no variant
    public Unit evolution; //null if highest evolution

    public UnitStats baseStats;

    protected virtual void Awake() {
        gameObject.AddComponent<UnitGestureAnimation>();
        gameObject.AddComponent<UnitCarryAnimation>();
        gameObject.AddComponent<UnitStoreAnimation>();
    }

    public enum Evl_Chain { One, Two, Three }; //number represents how far down in evolution chain (one is base, two is first evo, etc.)
    public Evl_Chain evl_chain;

    public void UpdateTile(Tile tile) {
        this.tile = tile;
    }

    public bool IsUnitBenched() {
        return tile.GetTilePosition().y == -1;
    }

    public Tile GetTile() {
        return tile;
    }
}
