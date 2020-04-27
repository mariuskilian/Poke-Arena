using UnityEngine;
using Bolt;

public class ServerManager : GlobalEventListener {

    public static ServerManager Instance;

    public GameMan Game { get; set; }

    private void Awake() {
        if (Instance == null) Instance = this;
    }

}