using UnityEngine;
using static PlayerBoardMan;

public class Tile {

    public Unit CurrentUnit { get; private set; }

    public bool IsTileFilled { get { return CurrentUnit != null; } }
    public bool IsBoardTile { get { return TilePosition.y != -1; } }

    public Vector2Int TilePosition { get; private set; } // y-Component should be set to -1 if it's a bench tile
    public Vector3 LocalPosition { get; private set; }

    public Tile(int x, int y) {
        CurrentUnit = null;
        TilePosition = new Vector2Int(x, y);
        LocalPosition = CalculateLocalPosition();
    }

    public Unit ClearTile() { Unit _unit = CurrentUnit; CurrentUnit = null; return _unit; }

    public void FillTile(Unit unit) {
        if (this.CurrentUnit != null) return;
        if (IsBoardTile) unit.gameObject.transform.SetSiblingIndex(0);
        this.CurrentUnit = unit;
        this.CurrentUnit.UpdateTile(this);
        UpdateUnitTransform();
    }

    public void ResetTile() { if (CurrentUnit != null) FillTile(ClearTile()); }

    #region Helpers
    private Vector3 CalculateLocalPosition() {
        Vector2 Factor;
        if (IsBoardTile) Factor = Layout.BoardTileSize * TilePosition + Layout.BoardTileOffset + Layout.BoardOffset;
        else Factor = Layout.BenchTileSize * TilePosition + Layout.BenchTileOffset + Layout.BenchOffset;
        return Vector3.right * Factor.x + Vector3.forward * Factor.y;
    }

    private void UpdateUnitTransform() {
        CurrentUnit.transform.localPosition = LocalPosition;
        CurrentUnit.transform.localRotation = Quaternion.Euler(0f, (IsBoardTile) ? 0f : 180f, 0f);
    }
    #endregion

    public static void SwapTiles(Tile t1, Tile t2) {
        if (t1 == null || t2 == null) return;

        if (!t1.IsTileFilled && !t2.IsTileFilled) return;
        else if (t1.IsTileFilled && !t2.IsTileFilled) t2.FillTile(t1.ClearTile());
        else if (!t1.IsTileFilled && t2.IsTileFilled) t1.FillTile(t2.ClearTile());
        else { Unit unit = t1.ClearTile(); t1.FillTile(t2.ClearTile()); t2.FillTile(unit); }
    }

}