using UnityEngine;
using System.Collections;

public class UnitStoreAnimation : UnitGestureAnimation {

    private new void Awake() {
        base.Awake();
        InitEventSubscribers();
    }

    protected override void StoreUpdate() {
        base.StoreUpdate();
    }

    private void InitEventSubscribers() {
        StoreMan store = StoreMan.Instance;
        store.NewUnitInStoreEvent += HandleNewUnitInStoreEvent;
    }

    private IEnumerator WaitThenDrop(int index) {
        gameObject.transform.Translate(Vector3.up * 1000);
        float normalizedIndex = (float) index / (float) (StoreMan.Instance.StoreSize - 1); // 0 <= normalizedIndex <= 1
        yield return new WaitForSeconds(normalizedIndex * 0.3f);
        gameObject.transform.Translate(Vector3.down * 1000);

        anim.SetTrigger(DROPPED_IN_STORE);
    }

    private void HandleNewUnitInStoreEvent(Unit unit, int index) {
        if (IsThisUnit(unit)) PoolMan.Instance.StartCoroutine(WaitThenDrop(index));
    }
}
