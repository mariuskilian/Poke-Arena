using UnityEngine;

[CreateAssetMenu(fileName = "ArenaLayout", menuName = "Poke-Arena/Arena Layout")]
public class ArenaLayout : ScriptableObject {

    public Array2DBool board = new Array2DBool(10, 10);
    public int BenchSizeTiles = 10;

    public bool
        EqualDimensions,
        PointSymmetric,
        YSymmetric,
        BenchCentered,
        DefaultYOffset
        ;

    public enum Fill { STRETCH, FIT_WIDTH, FIT_HEIGHT, FIT }
    public Fill fillMode;

    private readonly Vector2 ArenaSizeWorld = new Vector2(20, 20);

    public Vector2Int BoardSizeTiles {
        get { return new Vector2Int(board.Length, board[0].Length); }
    }

    public Vector2 BoardSizeWorld { get { return BoardSizeTiles * TileSize; } }

    public Vector2 BenchSizeWorld { get { return new Vector2(BenchSizeTiles, 1f) * TileSize; } }

    public Vector2 TileSize {
        get {
            float width = 10f / (float)BoardSizeTiles.x;
            float height = 10f / (float)BoardSizeTiles.y;
            float factor = 1f;
            if (fillMode != Fill.STRETCH) {
                if (fillMode == Fill.FIT_HEIGHT || width > height) {
                    width = height;
                    factor = width * BoardSizeTiles.x / 20f;
                } else if (fillMode == Fill.FIT_WIDTH || width < height) {
                    height = width;
                    factor = height * BoardSizeTiles.y / 20f;
                }
            }
            if (factor > 1f) {
                width = width / factor;
                height = height / factor;
            }
            return new Vector2(width, height);
        }
    }

    public Vector2 TileOffset { get { return 0.5f * TileSize; } }

    public Vector2 BoardOffsetWorld { get { return 0.5f * (ArenaSizeWorld - BoardSizeWorld); } }

    private float _benchXOffset = 0f;
    private float _benchYOffset = -1f;
    public Vector2 BenchOffsetTiles {
        get {
            float xOffset = (!BenchCentered) ? _benchXOffset : (float)(BoardSizeWorld.x - BenchSizeWorld.x) / 2f;
            float yOffset = (!DefaultYOffset) ? _benchYOffset : -1f / (float)TileSize.y;
            return new Vector2(xOffset, yOffset);
        }
        set {
            _benchXOffset = value.x;
            _benchYOffset = value.y;
        }
    }

    public Vector2 BenchOffsetWorld { get { return BoardOffsetWorld + BenchOffsetTiles * TileSize; } }

}