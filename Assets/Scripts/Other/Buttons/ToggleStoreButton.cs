using UnityEngine;
using UnityEngine.UI;

public class ToggleStoreButton : MonoBehaviour {

    [SerializeField] private GameObject store = null; //Store to hide/show
    [SerializeField] private Text textField = null; //button to change text of

    //toggles visibility of the shop
    public void ToggleStore() {
        store.SetActive(!store.activeSelf);
    }

    private void Update() {
        textField.text = (store.activeSelf) ? "Hide Store" : "Show Store";
    }
}
