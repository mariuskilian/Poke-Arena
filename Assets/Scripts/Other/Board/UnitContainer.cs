using UnityEngine;
using System.Collections.Generic;
using static GameInfo;

public class UnitContainer : MonoBehaviour {

    private int[] numBaseUnits = { 0, 0};
    
    public bool IsReadyForEvolve(EvlChain evolutionChain) {
        if (evolutionChain == EvlChain.TOP) return false;
        return numBaseUnits[(int)evolutionChain] == 2;
    }
    
    public bool TryAddUnit(BoardUnit unit) {
        if (unit.properties.name != gameObject.name) return false;
        unit.transform.SetParent(transform);
        if (unit.evolutionChain != EvlChain.TOP)
            numBaseUnits[(int)unit.evolutionChain]++;
        return true;
    }

    public List<Tile> TryGetEvolvingUnits(EvlChain evolutionChain) {
        if (evolutionChain == EvlChain.TOP) return null;
        var Tiles = new List<Tile>();
        for (int childIdx = 0; childIdx < transform.childCount; childIdx++) {
            BoardUnit unit = transform.GetChild(childIdx).GetComponent<BoardUnit>();
            if (unit.evolutionChain == evolutionChain) Tiles.Add(unit.CurrentTile);
        }
        if (Tiles.Count == 3) return Tiles;
        return null;
    }

}