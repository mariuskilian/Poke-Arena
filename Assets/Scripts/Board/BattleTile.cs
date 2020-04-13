using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTile : Tile {

    public bool IsAlliedUnit { get; private set; }

    public BattleTile(int x, int y) : base(x, y) { }

    public void FillTile(Unit unit, bool isAlliedUnit = true) {
        base.FillTile(unit);
        IsAlliedUnit = isAlliedUnit;
    }

    public static void SwapTiles(BattleTile t1, BattleTile t2) {
        Tile.SwapTiles(t1, t2);
        bool tmpAllied = t1.IsAlliedUnit;
        t1.IsAlliedUnit = t2.IsAlliedUnit;
        t2.IsAlliedUnit = tmpAllied;
    }
}
