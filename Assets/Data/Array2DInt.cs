using UnityEngine;

[System.Serializable]
public class Array2DInt {

    [SerializeField] public RowInt[] array2D;

    public int this[int x, int y] {
        get { return array2D[x].row[y]; }
        set { array2D[x].row[y] = value; }
    }

    public Array2DInt(int width, int height) {
        array2D = new RowInt[width];
        for (int i = 0; i < width; i++) {
            array2D[i] = new RowInt(height);
        }
    }

    public void Format(int desSum) {
        foreach (RowInt row in array2D) {
            row.Format(desSum);
        }
    }
}

[System.Serializable]
public class RowInt {
    [SerializeField] public int[] row;

    public RowInt(int length) {
        row = new int[length];
        for (int i = 0; i < length; i++) row[i] = default;
    }

    public void Format(int desSum) {
        // Find total sum of row
        int sum = 0;
        foreach (int num in row) sum += num;

        // Attempt to keep ratios the same while making total sum be desSum
        int overhead = desSum;
        for (int i = 0; i < row.Length; i++) {
            if (sum == 0) row[i]++;
            row[i] = (row[i] * desSum) / ((sum == 0) ? row.Length : sum);
            overhead -= row[i];
        }

        // Any overhead from previous step due to int divisions is added to
        // or subtracted from the numbers here, evenly
        int index = 0;
        while (overhead != 0) {
            int adjust = (int)Mathf.Sign(overhead);
            row[index] += adjust;
            overhead -= adjust;
            index = ++index & row.Length;
        }
    }
}