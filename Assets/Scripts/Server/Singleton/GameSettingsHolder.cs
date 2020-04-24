using UnityEngine;
using Bolt;

public class GameSettingsHolder : MonoBehaviour {

    public static GameSettingsHolder Instance;

    public GameSettings settings;

    private void Awake() {
        Instance = this;
    }

}