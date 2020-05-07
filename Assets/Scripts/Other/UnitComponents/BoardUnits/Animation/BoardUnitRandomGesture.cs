using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static GameInfo;

public class BoardUnitRandomGesture : BoardUnitAnimation {

    private bool isDoingRandomGesture = false;

    private void Update() { if (!isDoingRandomGesture) StartCoroutine(RandomGesture()); }

    private IEnumerator RandomGesture() {
        isDoingRandomGesture = true;
        float random = (float)RNG.NextDouble();
        yield return new WaitForSeconds((random * random * 20f) + 5f);
        if (!animator.GetBool(NonReactive)) {
            int index = RNG.Next(Gestures.Count);
            var key = new List<string>(Gestures.Keys)[index];
            TryPerformGesture(key, false);
        }
        isDoingRandomGesture = false;
    }
}