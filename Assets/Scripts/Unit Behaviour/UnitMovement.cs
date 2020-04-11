using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : UnitBehaviour {

    public enum MoveType { NONE, WALK, RUN }
    public MoveType CurrentMoveType { get; private set; }

    private readonly float
        WALK_SPEED = 1f,
        RUN_SPEED = 3f
        ;

    private void Start() {
        CurrentMoveType = MoveType.NONE;
        InitEventSubscribers();
    }

    private void Update() {
        if (CurrentMoveType == MoveType.NONE) return;

        Vector3 targetPos = unit.GetTile().GetWorldPosition();

        if (Vector3.Equals(gameObject.transform.position, targetPos)) {
            CurrentMoveType = MoveType.NONE;
            return;
        }

        float moveSpeed = 0f;
        if (CurrentMoveType == MoveType.WALK) moveSpeed = WALK_SPEED;
        if (CurrentMoveType == MoveType.RUN) moveSpeed = RUN_SPEED;

        gameObject.transform.LookAt(targetPos);
        gameObject.transform.position = 
            Vector3.MoveTowards(gameObject.transform.position, targetPos, moveSpeed * Time.deltaTime);
    }

    private void InitEventSubscribers() {
        BoardMan board = BoardMan.Instance;
        board.UnitTeleportEvent += HandleUnitTeleportEvent;
    }

    private void HandleUnitTeleportEvent(Unit unit, Tile fromTile) {
        if (IsThisUnit(unit) && fromTile.IsBoardTile() && unit.GetTile().IsBoardTile()) {
            gameObject.transform.position = fromTile.GetWorldPosition();
            CurrentMoveType = MoveType.RUN;
        }
    }
}
