using UnityEngine;
using System;
using System.Collections.Generic;
using static GameInfo;

public class UnitContainer : MonoBehaviour {

    private List<BoardUnit> ListOfUnits {
        get {
            List<BoardUnit> Units = new List<BoardUnit>();
            for (int childIdx = 0; childIdx < transform.childCount; childIdx++)
                Units.Add(transform.GetChild(childIdx).GetComponent<BoardUnit>());
            return Units;
        }
    }

    private int[] NumUnitsPerEvlChain {
        get {
            int[] result = new int[ArrayOfEnum<EvlChain>().Length];
            foreach (var u in ListOfUnits) result[(int)u.evolutionChain]++;
            return result;
        }
    }
    
    public bool IsReadyForEvolve(EvlChain evolutionChain) {
        if (evolutionChain == EvlChain.TOP) return false;
        return NumUnitsPerEvlChain[(int)evolutionChain] == 2;
    }
    
    public bool TryAddUnit(BoardUnit unit) {
        if (unit.properties.name != gameObject.name) return false;
        unit.transform.SetParent(transform);
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

    public void RemoveUnit(BoardUnit unit) { unit.transform.parent = null; }

}