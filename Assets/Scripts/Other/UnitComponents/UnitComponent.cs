using UnityEngine;

public class UnitComponent : MonoBehaviour {

    private BoardUnit unit;
    private StoreUnit storeUnit;

    protected T This<T>() where T : MonoBehaviour {
        if (unit != null && typeof(T) == typeof(BoardUnit)) return unit as T;
        if (storeUnit != null && typeof(T) == typeof(StoreUnit)) return storeUnit as T;
        return null;
    }

    protected void Awake() { unit = gameObject.GetComponent<BoardUnit>(); storeUnit = gameObject.GetComponent<StoreUnit>(); }

    protected bool IsThis<T>(T tUnit) where T : MonoBehaviour { return This<T>() == tUnit; }

}