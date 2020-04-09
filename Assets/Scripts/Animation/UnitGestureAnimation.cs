using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitGestureAnimation : UnitAnimation {

    #region Variables
    private bool isDoingRandomGesture = false;
    #endregion

    protected void PerformRandomGesture() {
        if (!isDoingRandomGesture) RandomGesture();
    }

    protected sealed override void Update() {
        if (unit.IsInStore())
            StoreUpdate();
        else
            BoardUpdate();
    }

    protected virtual void StoreUpdate() {
    }
    protected virtual void BoardUpdate() {
    }

    private IEnumerator RandomGesture() {
        isDoingRandomGesture = true;
        float random = (float) GameMan.random.NextDouble();
        yield return new WaitForSeconds((random * random * 10f) + 5f);
        if (!anim.GetBool(TYPE_NON_REACTIVE_GESTURE)) {
            if (GameMan.random.Next(0, 10) < 5) {
                int index = GameMan.random.Next(0, AvailableAnimations.Count);
                List<string> Keys = new List<string>(AvailableAnimations.Keys);
                string key = Keys[index];
                TryPerformAnimation(key, false);
            }
            isDoingRandomGesture = false;
        }
    }
}
