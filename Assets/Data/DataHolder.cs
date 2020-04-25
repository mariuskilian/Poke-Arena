using UnityEngine;

public class DataHolder : MonoBehaviour {

    public static DataHolder Instance { get; private set; }

    public GameMode[] gameModes;

    public GameSettings[] gameSettings;

    private void Awake() {
        if (Instance == null) Instance = this;
    }

}