using UnityEngine;

public class DataHolder : MonoBehaviour {

    public static DataHolder Instance { get; private set; }

    public GameMode[] GameModes;

    public GameSettings[] GameSettings;

    public Unit[] BaseUnitPrefabs;

    private void Awake() {
        if (Instance == null) Instance = this;
    }

}