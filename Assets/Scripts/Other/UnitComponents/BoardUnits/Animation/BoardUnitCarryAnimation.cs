using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BoardUnitCarryAnimation : BoardUnitAnimation {

    private void Start() { SubscribeLocalEventHandlers(); }

    private void SubscribeLocalEventHandlers() {
        if (!BoltNetwork.IsClient) return;
        var selection = SelectionMan.Instance;
        selection.UnitSelectEvent += HandleUnitSelectEvent;
    }

    private void HandleUnitSelectEvent(BoardUnit unit) {
        if (!IsThis<BoardUnit>(unit)) return;
        animator.SetTrigger(PickedUp);
    }

}