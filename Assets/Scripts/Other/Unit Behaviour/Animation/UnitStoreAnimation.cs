using UnityEngine;
using System.Collections;

public class UnitStoreAnimation : UnitGestureAnimation {

    private new void Awake() {
        base.Awake();
    }

    protected override void StoreUpdate() {
        base.StoreUpdate();
    }
    
    // private void HandleNewUnitInStoreEvent(Unit unit, int index) {
    //     if (IsThisUnit(unit)) anim.SetTrigger(DROPPED_IN_STORE);
    // }
}
