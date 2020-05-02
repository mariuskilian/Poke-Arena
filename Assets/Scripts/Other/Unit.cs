using UnityEngine;
using Bolt;

public class Unit : EntityBehaviour<IUnitState> {
    public Unit variant;
    public Unit evolution;
    public UnitProperties properties;

    public void SetActive(bool state) { 
        for (int childIdx = 0; childIdx < transform.childCount; childIdx++)
            transform.GetChild(childIdx).gameObject.SetActive(state);
    }
}