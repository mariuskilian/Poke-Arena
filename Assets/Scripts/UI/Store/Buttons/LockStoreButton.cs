using UnityEngine;
using UnityEngine.UI;

public class LockStoreButton : MonoBehaviour {

    [SerializeField] private Text textField = null;

    public void ToggleLockStore() {
        StoreMan.Instance.ToggleLocked();
    }

    private void Update() {
        textField.text = (StoreMan.Instance.IsLocked) ? "Locked" : "Unlocked";
    }
}
