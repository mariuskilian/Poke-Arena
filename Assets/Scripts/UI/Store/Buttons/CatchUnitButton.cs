using UnityEngine;

public class CatchUnitButton : MonoBehaviour {

    private Unit unit;

    public void OnButtonClick() {
        StoreMan.Instance.BuyUnit(unit);
    }

    public void SetUnit(Unit unit) {
        this.unit = unit;
        gameObject.SetActive(true);
    }

    public bool BelongsToUnit(Unit unit) {
        return this.unit == unit;
    }

    public void DeactivateButton() {
        this.unit = null;
        gameObject.SetActive(false);
    }

}
