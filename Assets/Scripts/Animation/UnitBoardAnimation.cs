using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UnitBoardAnimation : UnitGestureAnimation {

    #region Variables
    Coroutine randomGestureHandle;
    #endregion

    #region Unity Methods
    private new void Awake() {
        base.Awake();
        InitEventSubscribers();
    }

    protected override void BoardUpdate() {
        base.BoardUpdate();
        PerformRandomGesture();
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

    #region Event Handlers
    private void HandleUnitBoughtEvent(Unit unit) {
        if (IsThisUnit(unit)) TryPerformAnimation(EXCITED, true);
    }

    private void HandleUnitSoldEvent(Unit unit) {
    }

    private void HandleEvolutionEvent(List<Tile> tiles, Unit unit) {
        if (IsThisUnit(unit)) TryPerformAnimation(SHAKE, true);
    }

    private void HandleUnitTeleportEvent(Unit unit) {
        if (IsThisUnit(unit)) TryPerformAnimation(SHAKE, true);
    }
    #endregion
}
