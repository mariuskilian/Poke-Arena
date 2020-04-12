using UnityEngine;
using System.Collections;

public class UnitStoreAnimation : UnitGestureAnimation {

    private readonly string IRIS = "Iris", FIRE = "Fire";
    private readonly string
        ALPHA_FADE = "_AlphaFade",
        TEXTURE_FADE = "_TextureFade",
        THICKNESS = "_DissolveThickness",
        SIZE = "_DissolveSize",
        COLOR = "_DissolveColor"
        ;
    private bool spawned = false;

    private new void Awake() {
        base.Awake();
        InitEventSubscribers();
    }

    protected override void StoreUpdate() {
        base.StoreUpdate();
        if (!spawned) {
            foreach (Material m in gameObject.GetComponentInChildren<Renderer>().materials) { 
                if (m.name.Contains(IRIS) || m.name.Contains(FIRE)) continue;
                float speed = 0.75f;

                m.SetFloat(ALPHA_FADE, m.GetFloat(ALPHA_FADE) + speed * Time.deltaTime);
                if (m.GetFloat(ALPHA_FADE) >= 0.6f)
                    m.SetFloat(TEXTURE_FADE, m.GetFloat(TEXTURE_FADE) + speed * Time.deltaTime);
                if (m.GetFloat(TEXTURE_FADE) >= 2f) {
                    m.SetFloat(ALPHA_FADE, 2f);
                    m.SetFloat(TEXTURE_FADE, 2f);
                    spawned = true;
                }
            }
        }
    }

    private void InitEventSubscribers() {
        StoreMan store = StoreMan.Instance;
        store.NewUnitInStoreEvent += HandleNewUnitInStoreEvent;
    }

    private IEnumerator WaitThenDrop(int index) {
        gameObject.transform.Translate(Vector3.up * 1000);
        float normalizedIndex = (float) index / (float) (StoreMan.Instance.StoreSize - 1); // 0 <= normalizedIndex <= 1
        yield return new WaitForSeconds(normalizedIndex * 0.5f);
        gameObject.transform.Translate(Vector3.down * 1000);
        foreach (Material m in gameObject.GetComponentInChildren<Renderer>().materials) {
            if (m.name == IRIS) continue;
            m.SetFloat(ALPHA_FADE, 0.4f);
            m.SetFloat(TEXTURE_FADE, 0f);
        }
        anim.SetTrigger(DROPPED_IN_STORE);
    }

    private void HandleNewUnitInStoreEvent(Unit unit, int index) {
        if (IsThisUnit(unit)) PoolMan.Instance.StartCoroutine(WaitThenDrop(index));
    }
}
