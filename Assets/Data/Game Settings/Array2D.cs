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

    public void Format(int desSum) {
        foreach (Row row in array2D) {
            row.FormatRow(desSum);
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

    public void FormatRow(int desSum) {
        int sum = 0;
        foreach (int num in row) sum += num;

        int overhead = desSum;
        for (int i = 0; i < row.Length; i++) {
            if (sum == 0) row[i]++;
            row[i] = (row[i] * desSum) / ((sum == 0) ? 1 : sum);
            overhead -= row[i];
        }
        int index = 0;
        while (overhead != 0) {
            row[index] += (int)Mathf.Sign(overhead);
            overhead -= (int)Mathf.Sign(overhead);
            index = ++index % row.Length;
        }
    }
}