using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameInfo;

public class UnitShaderEffects : UnitComponent {

    private readonly string Iris = "Iris";

    protected List<Material> Materials;

    private new void Awake() {
        base.Awake();
        Materials = new List<Material>();
        foreach (var m in gameObject.GetComponentInChildren<Renderer>().materials) {
            if (!m.name.Contains(Iris)) Materials.Add(m);
        }
    }

}