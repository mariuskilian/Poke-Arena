using UnityEngine;

public class UnitComponent : MonoBehaviour {

    private BoardUnit boardUnit;
    private StoreUnit storeUnit;

    protected T This<T>() where T : MonoBehaviour {
        if (boardUnit != null && typeof(T) == typeof(BoardUnit)) return boardUnit as T;
        if (storeUnit != null && typeof(T) == typeof(StoreUnit)) return storeUnit as T;
        return null;
    }

    protected void Awake() { boardUnit = gameObject.GetComponent<BoardUnit>(); storeUnit = gameObject.GetComponent<StoreUnit>(); }

    protected bool IsThis<T>(T tUnit) where T : MonoBehaviour { return This<T>() == tUnit; }

}