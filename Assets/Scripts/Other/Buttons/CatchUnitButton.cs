using UnityEngine;

public class CatchUnitButton : MonoBehaviour {

    private int storeIdx;

    public void OnButtonClick() {
        // Buy Unit
    }

    public void SetStoreIdx(int idx) {
        storeIdx = Mathf.Clamp(idx, 0, 4);
    }

    public void ActivateButton() {
        gameObject.SetActive(true);
    }

    public void DeactivateButton() {
        gameObject.SetActive(false);
    }

}