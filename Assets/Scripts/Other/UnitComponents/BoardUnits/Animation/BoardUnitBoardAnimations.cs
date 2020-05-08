using UnityEngine;
using Bolt;

public class BoardUnitBoardAnimations : BoardUnitAnimation {

    private void Start() { SubscribeLocalEventHandlers(); }

    private void SubscribeLocalEventHandlers() {
        Player player = This<BoardUnit>().Owner;
        var board = player.GetPlayerMan<PlayerBoardMan>();
        board.UnitDeselectEvent += HandleUnitPlacedEvent;
        board.UnitTeleportedEvent += HandleUnitTeleportedEvent;
    }

    private void HandleUnitPlacedEvent(BoardUnit unit) {
        if (!IsThis<BoardUnit>(unit)) return;
        This<BoardUnit>().state.Dropped();
    }

    private void HandleUnitTeleportedEvent(BoardUnit unit) {
        if (!IsThis<BoardUnit>(unit)) return;
        TryPerformGesture(Shake, true);
    }

}