using UnityEngine;

public class ServerManager : MonoBehaviour {

    public static ServerManager Instance;

    private void Awake() {
        if (Instance == null) Instance = this;
    }

}
