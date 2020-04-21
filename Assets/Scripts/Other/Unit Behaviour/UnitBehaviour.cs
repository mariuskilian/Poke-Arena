using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;

public class UnitBehaviour : EntityBehaviour<IUnitState> {

    protected Unit unit;

    protected void Awake() {
        unit = gameObject.GetComponent<Unit>();
    }

    protected bool IsThisUnit(Unit unit) {
        return this.unit == unit;
    }
}
