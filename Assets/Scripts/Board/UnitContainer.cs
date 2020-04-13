using UnityEngine;
using System.Collections.Generic;

public class UnitContainer : MonoBehaviour {

    private int numBaseUnits = 1;

    private void Start() {
        StoreMan store = StoreMan.Instance;
        store.UnitBoughtEvent += HandleUnitBoughtEvent;

        BoardMan board = BoardMan.Instance;
        board.EvolutionEvent += HandleEvolutionEvent;
    }

    private void HandleUnitBoughtEvent(Unit unit) {
        if (IsInRightContainer(unit)) {
            numBaseUnits++;
        }
    }

    private bool IsInRightContainer(Unit unit) {
        return (unit.properties.name.Equals(gameObject.name));
    }

    public int GetNumBaseUnits() {
        return numBaseUnits;
    }

    public List<Tile> CheckForEvolution(Unit.Evl_Chain evl_chain) {
        List<Tile> tiles = new List<Tile>();
        for (int i = 0; i < transform.childCount; i++) {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.activeSelf) {
                Unit childUnit = child.GetComponent<Unit>();
                if (childUnit.evl_chain == evl_chain) {
                    tiles.Add(childUnit.GetTile());
                }
            }
        }
        if (tiles.Count == 3) return tiles;
        return null;
    }

    public void HandleEvolutionEvent(List<Tile> tiles, Unit unit) {
        if (tiles[0].GetUnit().properties.name != name) return;
        if (tiles[0].GetUnit().evl_chain == Unit.Evl_Chain.One) {
            numBaseUnits -= tiles.Count;
        }
    }
}
