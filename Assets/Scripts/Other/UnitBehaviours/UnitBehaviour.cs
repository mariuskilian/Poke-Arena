using UnityEngine;
using Bolt;

public class UnitBehaviour : MonoBehaviour {

    protected Unit unit;
    protected void Awake() { unit = gameObject.GetComponent<Unit>(); }

    protected bool IsThisUnit(Unit unit) { return this.unit == unit; }

}