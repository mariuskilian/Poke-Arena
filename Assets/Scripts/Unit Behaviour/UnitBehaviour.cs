using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBehaviour : MonoBehaviour
{

    protected Unit unit;

    protected void Awake() {
        unit = gameObject.GetComponent<Unit>();
    }

    protected bool IsThisUnit(Unit unit) {
        return this.unit == unit;
    }
}
