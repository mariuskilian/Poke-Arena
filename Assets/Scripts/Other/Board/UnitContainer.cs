using UnityEngine;
using System.Collections.Generic;
using static GameInfo;

public class UnitContainer : MonoBehaviour {

    public int numBaseUnits { get; private set; } = 0;
    
    public bool TryAddUnit(Unit unit) {
        if (unit.properties.name != gameObject.name) return false;
        unit.transform.SetParent(transform);
        if (unit.evolutionChain == EvlChain.BASE) numBaseUnits++;
        return true;
    }

    public List<Tile> CheckForEvolution(EvlChain evolutionChain) {
        if (evolutionChain == EvlChain.TOP) return null;
        var Tiles = new List<Tile>();
        for (int childIdx = 0; childIdx < transform.childCount; childIdx++) {
            Unit unit = transform.GetChild(childIdx).GetComponent<Unit>();
            if (unit.evolutionChain == evolutionChain) Tiles.Add(unit.CurrentTile);
        }
        if (Tiles.Count == 3) return Tiles;
        return null;
    }

}