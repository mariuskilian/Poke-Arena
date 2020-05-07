using UnityEngine;
using static PlayerBoardMan;

public class Tile {

    public BoardUnit CurrentUnit { get; private set; }

    public bool IsTileFilled { get { return CurrentUnit != null; } }
    public bool IsBoardTile { get { return TilePosition.y != -1; } }

    public Vector2Int TilePosition { get; private set; } // y-Component should be set to -1 if it's a bench tile
    public Vector3 LocalPosition { get; private set; }
    public Quaternion LocalRotation { get; private set; }

    public Tile(int x, int y) {
        CurrentUnit = null;
        TilePosition = new Vector2Int(x, y);
        CalculateLocalPositionAndRotation();
    }

    public BoardUnit ClearTile() { BoardUnit _unit = CurrentUnit; CurrentUnit = null; return _unit; }

    public void FillTile(BoardUnit unit) {
        if (this.CurrentUnit != null || unit == null) return;
        if (IsBoardTile) unit.gameObject.transform.SetSiblingIndex(0);
        CurrentUnit = unit;
        CurrentUnit.UpdateTile(this);
    }

    public void ResetTile() { if (IsTileFilled) FillTile(ClearTile()); }

    #region Helpers
    private void CalculateLocalPositionAndRotation() {
        Vector2 Factor = Layout.TileSize * TilePosition + Layout.TileOffset;
        Factor += (IsBoardTile) ? Layout.BoardOffsetWorld : Layout.BenchOffsetWorld;
        LocalPosition = Vector3.right * Factor.x + Vector3.forward * Factor.y;
        LocalRotation = Quaternion.Euler(0, (IsBoardTile) ? 0 : 180, 0);
    }
    #endregion

    public static void SwapTiles(Tile t1, Tile t2) {
        if (t1 == null || t2 == null) return;

        if (!t1.IsTileFilled && !t2.IsTileFilled) return;
        else if (t1.IsTileFilled && !t2.IsTileFilled) t2.FillTile(t1.ClearTile());
        else if (!t1.IsTileFilled && t2.IsTileFilled) t1.FillTile(t2.ClearTile());
        else { BoardUnit unit = t1.ClearTile(); t1.FillTile(t2.ClearTile()); t2.FillTile(unit); }
    }

}