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
        if (IsThisUnit(unit)) {
            AvailableAnimations.TryGetValue(EXCITED, out Action<bool> excited);
            excited?.Invoke(true);
        }
    }

    private void HandleUnitSoldEvent(Unit unit) {
    }

    private void HandleEvolutionEvent(List<Tile> tiles, Unit unit) {
        if (IsThisUnit(unit)) {
            AvailableAnimations.TryGetValue(SHAKE, out Action<bool> excited);
            excited?.Invoke();
        }
    }

    private void HandleUnitTeleportEvent(Unit unit) {
        if (IsThisUnit(unit))
            if (AvailableAnimations.TryGetValue(SHAKE, out Action<bool> shake))
                shake?.Invoke(true);
    }
    #endregion
}
