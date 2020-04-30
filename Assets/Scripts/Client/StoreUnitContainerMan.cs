using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;

public class StoreUnitContainerMan : GlobalEventListener {

    public static StoreUnitContainerMan Instance { get; private set; }

    private void Awake() { if (Instance == null) Instance = this; }

}
