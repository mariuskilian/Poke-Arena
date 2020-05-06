using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static GameInfo;

public class UnitRandomAnimation : UnitAnimation {

    /*private bool isDoingRandomGesture = false;

    private void Update() { if (!isDoingRandomGesture) StartCoroutine(RandomGesture()); }

    private IEnumerator RandomGesture() {
        isDoingRandomGesture = true;
        float random = (float)RNG.NextDouble();
        yield return new WaitForSeconds((random * random * 20f) + 5f);
        if (!anim.GetBool(NonReactiveGesture)) {
            int index = RNG.Next(AvailableAnimations.Count);
            var key = new List<string>(AvailableAnimations.Keys)[index];
            TryPerformAnimation(key, false);
        }
        isDoingRandomGesture = false;
    }*/
}