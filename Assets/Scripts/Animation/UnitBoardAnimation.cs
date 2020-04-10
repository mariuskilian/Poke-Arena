using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UnitBoardAnimation : UnitGestureAnimation {

    #region Variables
    bool isRunning = false;
    Vector3 currentPos;
    Vector3 newPos;
    #endregion

    #region Unity Methods
    private new void Awake() {
        base.Awake();
        InitEventSubscribers();
    }

    protected override void BoardUpdate() {
        base.BoardUpdate();
        PerformRandomGesture();
        if (isRunning) {
            float speed = 3 * Time.deltaTime;
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, newPos, speed);
            if (Vector3.Equals(gameObject.transform.position, newPos)) {
                isRunning = false;
                gameObject.GetComponent<Unit>().GetTile().UpdateUnitTransform();
                anim.SetBool("Running", false);
            }
        }
    }
    #endregion

    #region Initialisation
    private void InitEventSubscribers() {
        BoardMan board = BoardMan.Instance;
        board.UnitTeleportEvent += HandleUnitTeleportEvent;
        board.EvolutionEvent += HandleEvolutionEvent;
        StoreMan store = StoreMan.Instance;
        store.UnitBoughtEvent += HandleUnitBoughtEvent;
    }
    #endregion

    private void TriggerUnitRunAnimation(Vector3 from, Vector3 to) {
        currentPos = from;
        newPos = to;
        gameObject.transform.position = from;
        gameObject.transform.LookAt(to);
        anim.SetBool("Running", true);
        isRunning = true;
    }

    #region Event Handlers
    private void HandleUnitBoughtEvent(Unit unit) {
        if (IsThisUnit(unit)) TryPerformAnimation(EXCITED, true);
    }

    private void HandleUnitSoldEvent(Unit unit) {
    }

    private void HandleEvolutionEvent(List<Tile> tiles, Unit unit) {
        if (IsThisUnit(unit)) TryPerformAnimation(SHAKE, true);
    }

    private void HandleUnitTeleportEvent(Unit unit, Tile fromTile, Tile toTile) {
        if (IsThisUnit(unit)) {
            if (fromTile.IsBoardTile() && toTile.IsBoardTile())
                TriggerUnitRunAnimation(fromTile.GetWorldPosition(), toTile.GetWorldPosition());
            else TryPerformAnimation(SHAKE, true);
        }
    }
    #endregion
}
