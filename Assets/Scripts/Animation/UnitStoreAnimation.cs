using UnityEngine;
using System.Collections;

public class UnitStoreAnimation : UnitAnimation {

    void Start() {
        InitEventSubscribers();
    }

    private void InitEventSubscribers() {
        StoreMan store = StoreMan.Instance;
        store.NewUnitInStoreEvent += HandleNewUnitInStoreEvent;
    }

    private IEnumerator WaitThenDrop() {
        gameObject.transform.Translate(Vector3.up * 1000);
        yield return new WaitForSeconds((float) GameMan.random.NextDouble() / 3f);
        gameObject.transform.Translate(Vector3.down * 1000);
        anim.SetTrigger(TRIGGER_DROPPED_IN_STORE);
    }

    private void HandleNewUnitInStoreEvent(Unit unit) {
        if (IsThisUnit(unit)) GameMan.Instance.StartCoroutine(WaitThenDrop());
    }
}
