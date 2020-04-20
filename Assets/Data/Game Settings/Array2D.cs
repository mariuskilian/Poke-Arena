using UnityEngine;

[System.Serializable]
public class Array2D {

    [SerializeField] public Row[] array2D;

    public int this[int x, int y] {
        get { return array2D[x].row[y]; }
        set { array2D[x].row[y] = value; }
    }

    public Array2D(int width, int height) {
        array2D = new Row[width];
        for (int i = 0; i < width; i++) {
            array2D[i] = new Row(height);
        }
    }
}

[System.Serializable]
public class Row {
    [SerializeField] public int[] row;

    public Row(int length) {
        row = new int[length];
        for (int i = 0; i < length; i++) row[i] = 0;
    }
}