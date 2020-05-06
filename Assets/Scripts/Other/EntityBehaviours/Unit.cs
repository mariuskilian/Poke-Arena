using UnityEngine;
using Bolt;
using System.Collections.Generic;
using static GameInfo;

public class Unit : EntityBehaviour<IUnitState> {

    public Player Owner { get; private set; }

    public Tile CurrentTile { get; private set; }

    public Unit evolution;
    public EvlChain evolutionChain;
    public UnitProperties properties;

    private List<UnitBehaviour> UnitBehaviours;

    public override void Attached() {
        state.SetAnimator(GetComponent<Animator>());
        state.Animator.applyRootMotion = entity.IsOwner;

        state.AddCallback("Position", () => transform.position = state.Position);
        state.AddCallback("Rotation", () => transform.rotation = state.Rotation);

        if (BoltNetwork.IsClient && entity.HasControl) AddCollisionPlane();

        if (entity.IsOwner) InitUnitServerBehaviours();
        
    }

    private void InitUnitServerBehaviours() {
        UnitBehaviours = new List<UnitBehaviour> {
            gameObject.AddComponent<UnitRandomAnimation>()
        };
    }

    public void UpdateTile(Tile tile) { CurrentTile = tile; SetPositionAndRotation(tile.LocalPosition, tile.LocalRotation, true); }
    public void SetOwner(Player player) { if (Owner == null) Owner = player; }

    private void AddCollisionPlane() {
        BoxCollider collisionPlane = gameObject.AddComponent<BoxCollider>();
        float unitScale = .9f / transform.localScale.x;
        Vector2 tileSize = DataHolder.Instance.ArenaLayouts[0].TileSize;
        collisionPlane.size = new Vector3(tileSize.x * unitScale, 0, tileSize.y * unitScale);
    }

    public void SetPositionAndRotation(Vector3 position, Quaternion rotation, bool local) {
        if (!entity.IsOwner) return;

        if (local) { transform.localPosition = position; transform.localRotation = rotation; }
        else { transform.position = position; transform.rotation = rotation; }

        state.Position = transform.position - Vector3.up;
        state.Position = transform.position;
        state.Rotation = Quaternion.Euler(Vector3.up) * transform.rotation;
        state.Rotation = transform.rotation;
    }

}