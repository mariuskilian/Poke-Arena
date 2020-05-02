using UnityEngine;

public class CatchUnitButton : MonoBehaviour {

    private int storeIdx;

    public void OnButtonClick() {
        StoreButtonMan.Instance.TryBuyUnitEvent?.Invoke(storeIdx);
    }

    public void SetStoreIdx(int idx) {
        storeIdx = Mathf.Clamp(idx, 0, PlayerStoreMan.StoreSize - 1);
    }

    public void ActivateButton() {
        gameObject.SetActive(true);
    }

    public void DeactivateButton() {
        gameObject.SetActive(false);
    }

}