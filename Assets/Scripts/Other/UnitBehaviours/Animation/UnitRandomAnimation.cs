using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static GameInfo;

public class UnitRandomAnimation : UnitAnimation {

    private bool isDoingRandomGesture = false;

    private void Update() { if (!isDoingRandomGesture) StartCoroutine(RandomGesture()); }

    private IEnumerator RandomGesture() {
        isDoingRandomGesture = true;
        float random = (float)RNG.NextDouble();
        yield return new WaitForSeconds((random * random * 20f) + 5f);
        if (!animator.GetBool(NonReactive)) {
            int index = RNG.Next(AvailableGestures.Count);
            var key = new List<string>(AvailableGestures.Keys)[index];
            TryPerformGesture(key, false);
        }
        isDoingRandomGesture = false;
    }
}