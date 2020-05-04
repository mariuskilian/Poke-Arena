using UnityEngine;

[System.Serializable]
public class Array2DBool {

    [SerializeField] public RowBool[] array2D;

    public bool this[int x, int y] {
        get { return array2D[x][y]; }
        set { array2D[x][y] = value; }
    }

    public RowBool this[int x] { get { return array2D[x]; } }

    public Array2DBool(int width, int height) {
        array2D = new RowBool[width];
        for (int i = 0; i < width; i++) array2D[i] = new RowBool(height);
    }

    public bool IsInitialized {
        get {
            if (array2D != null) {
                if (Length == 0) return true;
                else return array2D[0].row != null;
            } else return false;
        }
    }

    public int Length { get { return array2D.Length; } }
}

[System.Serializable]
public class RowBool {
    [SerializeField] public bool[] row;

    public RowBool(int length) {
        row = new bool[length];
        for (int i = 0; i < length; i++) row[i] = default;
    }

    public bool this[int x] {
        get { return row[x]; }
        set { row[x] = value; }
    }

    public int Length { get { return row.Length; } }
}
