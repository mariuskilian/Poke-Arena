using UnityEngine;

public class ToggleShopButton : MonoBehaviour {

    public GameObject store; //Store to hide/show
    public GameObject textObject; //button to change text of

    private bool isStoreVisible;

    private void Start() {
        isStoreVisible = store.activeSelf;
        UpdateButtonText();
    }

    //toggles visibility of the shop
    public void OnClick() {
        isStoreVisible = !isStoreVisible;
        store.SetActive(isStoreVisible);
        UpdateButtonText();
    }

    private void UpdateButtonText() {
        string text;
        if (isStoreVisible) text = "Hide Shop";
        else text = "Show Shop";
        textObject.GetComponent<UnityEngine.UI.Text>().text = text;
    }
}
