using UnityEngine;

public abstract class ManagerBehaviour : MonoBehaviour {

    private bool lateStart = false;

    protected void Update() {
        if (!lateStart) {
            lateStart = true;
            LateStart();
        }
    }

    protected virtual void LateStart() {
    }
}
