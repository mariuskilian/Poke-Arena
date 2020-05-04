using UnityEngine;

[CreateAssetMenu(fileName = "ArenaLayout", menuName = "Poke-Arena/Arena Layout")]
public class ArenaLayout : ScriptableObject {

    public Array2DBool board = new Array2DBool(10, 10);
    public int BenchSize;

    public bool
        EqualDimensions,
        PointSymmetric,
        YSymmetric,
        BenchCentered,
        DefaultYOffset
        ;

    public enum Fill { STRETCH, FIT_WIDTH, FIT_HEIGHT, FIT }
    public Fill fillMode;

    public Vector2Int BoardSize {
        get { return new Vector2Int(board.Length, board[0].Length); }
    }

    public Vector2 BoardTileSize {
        get {
            float width = 10f / (float)BoardSize.x;
            float height = 10f / (float)BoardSize.y;
            float factor = 1f;
            if (fillMode != Fill.STRETCH) {
                if (fillMode == Fill.FIT_HEIGHT || width > height) {
                    width = height;
                    factor = width * BoardSize.x / 20f;
                } else if (fillMode == Fill.FIT_WIDTH || width < height) {
                    height = width;
                    factor = height * BoardSize.y / 20f;
                }
            }
            if (factor > 1f) {
                width = width / factor;
                height = height / factor;
            }
            return new Vector2(width, height);
        }
    }

    public Vector2 BenchTileSize { get { return new Vector2(10f / BenchSize, 1); } }

    public Vector2 BoardTileOffset { get { return 0.5f * BoardTileSize; } }
    public Vector2 BenchTileOffset { get { return 0.5f * BenchTileSize; } }

    public Vector2 BoardOffset {
        get {
            return new Vector2(
                (20 - (BoardTileSize.x * BoardSize.x)) / 2f,
                (20 - (BoardTileSize.y * BoardSize.y)) / 2f
            );
        }
    }

    private float _benchXOffset = 0f;
    private float _benchYOffset = -1f;
    public Vector2 BenchOffset {
        get {
            float xOffset = (!BenchCentered) ? _benchXOffset : (float)(BoardSize.x * BoardTileSize.x - BenchSize * BenchTileSize.x) / 2f;
            float yOffset = (!DefaultYOffset) ? _benchYOffset : -1f;
            return new Vector2(xOffset, yOffset);
        }
        set {
            _benchXOffset = value.x;
            _benchYOffset = value.y;
        }
    }

}