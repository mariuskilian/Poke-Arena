using UnityEngine;
using static GameInfo;

public class UnitContainer : MonoBehaviour {

    public int numBaseUnits { get; private set; } = 0;
    
    public bool TryAddUnit(Unit unit) {
        if (unit.properties.name != gameObject.name) return false;
        unit.transform.SetParent(transform);
        if (unit.evolutionChain == EvlChain.BASE) numBaseUnits++;
        return true;
    }

}