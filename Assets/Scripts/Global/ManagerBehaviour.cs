using Bolt;

public abstract class Manager : GlobalEventListener {

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
