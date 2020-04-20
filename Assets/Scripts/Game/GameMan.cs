using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;

[BoltGlobalBehaviour(BoltNetworkModes.Server)]
public class GameMan : Bolt.GlobalEventListener {

    public static GameMan Instance { get; private set; }
    
    public static System.Random rng = new System.Random();

    public GameSettings settings;

    private void Awake() {
        Instance = this;
    }

}