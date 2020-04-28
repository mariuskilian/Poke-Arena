using UnityEngine;

[System.Serializable]
public class Array2DArena {

    [SerializeField] public RowArena[] array2D;

    public ArenaInfo this[int x, int y] {
        get { return array2D[x].row[y]; }
        set { array2D[x].row[y] = value; }
    }

    public RowArena this[int x] {
        get { return array2D[x]; }
        set { array2D[x] = value; }
    }

    public Array2DArena(int width, int height) {
        array2D = new RowArena[width];
        for (int i = 0; i < width; i++) {
            array2D[i] = new RowArena(height);
        }
    }

    public bool IsInitialized {
        get {
            if (array2D != null) {
                if (Length == 0) return true;
                else return array2D[0].row != null;
            } else return false;
        }
    }

    public int Length {
        get {
            return array2D.Length;
        }
    }
}

[System.Serializable]
public class RowArena {
    [SerializeField] public ArenaInfo[] row;

    public RowArena(int length) {
        row = new ArenaInfo[length];
        for (int i = 0; i < length; i++) row[i] = default;
    }

    public int Length {
        get {
            return row.Length;
        }
    }
}