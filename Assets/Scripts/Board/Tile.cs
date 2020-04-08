using UnityEngine;

public class Tile {

    private Unit unit;
    private readonly Vector2Int tilePosition; //y component should be set to -1 if its a bench tile
    private readonly Vector3 worldPosition;

    public Tile(int x, int y) {
        unit = null;
        tilePosition = new Vector2Int(x, y);
        worldPosition = CalculateWorldPosition();
    }

    #region Tile Updates
    public Unit ClearTile() {
        if (unit == null) {
            Debug.LogWarning("Tile was already Cleared");
            return null;
        }
        Unit _unit = unit;
        unit = null;
        return _unit;
    }

    public void FillTile(Unit unit) {
        if (this.unit != null) {
            Debug.LogWarning("Tile already contains a Unit");
            return;
        }
        unit.UpdateTile(this);
        if (IsBoardTile()) unit.gameObject.transform.SetSiblingIndex(0);
        this.unit = unit;
        UpdateUnitTransform();
    }
    #endregion

    #region Helper Methods
    private Vector3 CalculateWorldPosition() {
        //Bench Tile
        if (!IsBoardTile()) {
            float xOffset = (BoardMan.BOARD_WIDTH - BoardMan.BENCH_SIZE) / 2f;
            return Vector3.right * (BoardMan.TILE_SIZE * tilePosition.x + BoardMan.TILE_OFFSET + xOffset)
                + Vector3.up + Vector3.forward * (BoardMan.BENCH_Y + BoardMan.TILE_OFFSET);
        }

        //Board Tile
        else {
            return Vector3.right * (BoardMan.TILE_SIZE * tilePosition.x + BoardMan.TILE_OFFSET)
                + Vector3.forward * (BoardMan.TILE_SIZE * tilePosition.y + BoardMan.TILE_OFFSET);
        }
    }

    private void UpdateUnitTransform() {
        unit.gameObject.transform.position = worldPosition;
        float facing = (tilePosition.y == -1) ? 180f : 0f;
        unit.gameObject.transform.rotation = Quaternion.Euler(0f,facing,0f);
    }
    #endregion

    #region Getter Methods
    public Vector2Int GetTilePosition() {
        return tilePosition;
    }

    public Vector3 GetWorldPosition() {
        return worldPosition;
    }

    public Unit GetUnit() {
        return unit;
    }

    public bool IsTileFilled() {
        return unit != null;
    }

    public bool IsBoardTile() {
        return tilePosition.y != -1;
    }
    #endregion

    public static void SwapTiles(Tile t1, Tile t2) {
        if (t1 == null || t2 == null) {
            Debug.LogWarning("One or more of the tiles was null");
            return;
        }
        if (!t1.IsTileFilled() && !t2.IsTileFilled()) return;
        else if (t1.IsTileFilled() && !t2.IsTileFilled()) t2.FillTile(t1.ClearTile());
        else if (!t1.IsTileFilled() && t2.IsTileFilled()) t1.FillTile(t2.ClearTile());
        else {
            Unit unit = t1.ClearTile();
            t1.FillTile(t2.ClearTile());
            t2.FillTile(unit);
        }
    }

    public void ResetTile() {
        if (unit != null) FillTile(ClearTile());
    }
}
