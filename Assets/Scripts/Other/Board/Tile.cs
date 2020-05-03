using UnityEngine;
using static PlayerBoardMan;

public class Tile {

    public Unit unit { get; private set; }

    public bool IsTileFilled { get { return unit != null; } }
    public bool IsBoardTile { get { return TilePosition.y != -1; } }

    public Vector2Int TilePosition { get; private set; } // y-Component should be set to -1 if it's a bench tile
    public Vector3 LocalPosition { get; private set; }

    public Tile(int x, int y) {
        unit = null;
        TilePosition = new Vector2Int(x, y);
        LocalPosition = CalculateLocalPosition();
    }

    public Unit ClearTile() { Unit _unit = unit; unit = null; return _unit; }

    public void FillTile(Unit unit) {
        if (this.unit != null) return;
        if (IsBoardTile) unit.gameObject.transform.SetSiblingIndex(0);
        this.unit = unit;
        this.unit.UpdateTile(this);
        UpdateUnitTransform();
    }

    public void ResetTile() { if (unit != null) FillTile(ClearTile()); }

    #region Helpers
    private Vector3 CalculateLocalPosition() {
        float xFactor = ((IsBoardTile) ? 0 : BenchXOffset) + TileSize * TilePosition.x + TileOffset;
        float zFactor = ((IsBoardTile) ? TileSize * TilePosition.y : BenchYOffset) + TileOffset;
        return Vector3.right * xFactor + Vector3.forward * zFactor;
    }

    private void UpdateUnitTransform() {
        unit.transform.localPosition = LocalPosition;
        unit.transform.localRotation = Quaternion.Euler(0f, (IsBoardTile) ? 0f : 180f, 0f);
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