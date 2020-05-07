using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ArenaLayout))]
public class ArenaLayoutEditor : Editor {

    private ArenaLayout layout;

    int screenWidth;

    int gridSize;

    int tileWidth;
    int tileHeight;

    Vector2Int space = new Vector2Int(3, 2);

    public override void OnInspectorGUI() {
        layout = target as ArenaLayout;

        Undo.RecordObject(layout, "Arena Layout value change");
        EditorUtility.SetDirty(layout);

        Vector2Int ArenaSize = Vector2Int.zero;
        if (layout.board != null && layout.board.IsInitialized) {
            ArenaSize.x = layout.BoardSizeTiles.x;
            if (ArenaSize.x > 0) ArenaSize.y = layout.BoardSizeTiles.y;
        }

        GUILayout.Label("");

        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("Arena size:", GUILayout.Width(Screen.width * 0.37f));
            GUILayout.Label("Eq. Dimensions:");
            layout.EqualDimensions = EditorGUILayout.Toggle(layout.EqualDimensions);
            bool eq = layout.EqualDimensions;
            if (!eq) {
                GUILayout.Label("X", GUILayout.Width(15));
                ArenaSize.x = Mathf.Clamp(EditorGUILayout.DelayedIntField(ArenaSize.x), 1, 15);
            }
            GUILayout.Label((eq) ? "X & Y" : "Y");
            ArenaSize.y = Mathf.Clamp(EditorGUILayout.DelayedIntField(ArenaSize.y), 1, 14);
            if (ArenaSize.y % 2 != 0) ArenaSize.y++;
            if (eq) ArenaSize.x = ArenaSize.y;
        }
        GUILayout.EndHorizontal();

        layout.BenchSizeTiles = EditorGUILayout.DelayedIntField("Bench size", layout.BenchSizeTiles);

        if (layout.board == null || !layout.board.IsInitialized
            || layout.BoardSizeTiles.x == 0 || layout.BoardSizeTiles.x != ArenaSize.x
            || layout.BoardSizeTiles.y != ArenaSize.y) {
            layout.board = new Array2DBool(ArenaSize.x, ArenaSize.y);
        }

        GUIDivider();

        GUILayout.BeginHorizontal();
        {
            GUILayout.BeginVertical();
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.Label("ARENA SETTINGS");
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();

                Rect rect = EditorGUILayout.GetControlRect(false, 1);
                rect.height = 1;
                EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));

                GUILayout.BeginHorizontal();
                {
                    layout.PointSymmetric = GUIVerticalBool("Point Symmetric", layout.PointSymmetric);
                    GUIVerticalDivider(2);
                    layout.YSymmetric = GUIVerticalBool("Y Symmetric", layout.YSymmetric);
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
            GUIVerticalDivider(3);
            GUILayout.BeginVertical();
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.Label("BENCH SETTINGS");
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();

                Rect rect = EditorGUILayout.GetControlRect(false, 1);
                rect.height = 1;
                EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));

                GUILayout.BeginHorizontal();
                {
                    layout.BenchCentered = GUIVerticalBool("Bench X Centered", layout.BenchCentered);
                    GUIVerticalDivider(2);
                    layout.DefaultYOffset = GUIVerticalBool("Bench Default Y Offset", layout.DefaultYOffset);
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();

            
        }
        GUILayout.EndHorizontal();


        GUIDivider();

        layout.fillMode = (ArenaLayout.Fill)EditorGUILayout.EnumPopup("Fill Mode", layout.fillMode);

        GUIDivider();

        screenWidth = (int)((float)Screen.width * 0.8f);

        gridSize = Mathf.Min(50, (int)(screenWidth / 20f));

        tileWidth = Mathf.RoundToInt(screenWidth * layout.TileSize.x / 20f) - space.x;
        tileHeight = Mathf.RoundToInt(screenWidth * layout.TileSize.y / 20f) - space.y;

        Texture2D
            active = MakeTexture(tileWidth, tileHeight, Color.white),
            pointSymActive = MakeTexture(tileWidth, tileHeight, Color.gray),
            ySymActive = MakeTexture(tileWidth, tileHeight, 0.8f * Color.white + Color.black),
            inactive = MakeTexture(tileWidth, tileHeight, Color.clear)
            ;

        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            //ArenaSide(true);
            for (int i = 0; i < ArenaSize.x; i++) {
                EditorGUILayout.BeginVertical();
                {
                    for (int j = ArenaSize.y - 1; j >= 0; j--) {
                        Texture2D tex;
                        bool pointSym = layout.PointSymmetric && j >= ArenaSize.y / 2;
                        bool ySym = layout.YSymmetric && i >= (ArenaSize.x + 1) / 2;
                        if (layout.board[i, j]) {
                            if (pointSym) tex = pointSymActive;
                            else if (ySym) tex = ySymActive;
                            else tex = active;
                        } else tex = inactive;
                        
                        if (GUILayout.Button(tex, GUILayout.Width(tileWidth), GUILayout.Height(tileHeight))) {
                            layout.board[i, j] = !layout.board[i, j];
                        }
                    }
                }
                EditorGUILayout.EndVertical();
            }
            //ArenaSide(false);
            GUILayout.FlexibleSpace();
        }
        EditorGUILayout.EndHorizontal();

        if (layout.YSymmetric) {
            for (int i = 0; i < (ArenaSize.x + 1) / 2; i++) {
                for (int j = 0; j < ArenaSize.y; j++) {
                    int iAlt = ArenaSize.x - 1 - i;
                    layout.board[iAlt, j] = layout.board[i, j];
                }
            }
        }

        if (layout.PointSymmetric) {
            for (int i = 0; i < ArenaSize.x; i++) {
                for (int j = 0; j < ArenaSize.y / 2; j++) {
                    int iAlt = ArenaSize.x - 1 - i;
                    int jAlt = ArenaSize.y - 1 - j;
                    layout.board[iAlt, jAlt] = layout.board[i, j];
                }
            }
        }

    }

    private Texture2D MakeTexture(int width, int height, Color c) {
        var texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
        for (int i = 0; i < width; i++) for (int j = 0; j < height; j++) texture.SetPixel(i, j, c);
        texture.Apply();
        return texture;
    }

    void ArenaSide(bool left) {
        int sideWidth = (int)((screenWidth - (layout.BoardSizeTiles.x * (tileWidth + space.x))) / 2f);
        int widthOverhead = (sideWidth % gridSize);

        int sideHeight = (layout.BoardSizeTiles.y * (tileWidth + space.y));
        int heightOverhead = (sideHeight % gridSize);

        Texture2D black = MakeTexture(gridSize, gridSize, Color.black);

        bool rightFlexDone = false;

        bool firstHeightFlexDone = false;
        bool secondHeightFlexDone = false;

        if (gridSize <= 0) return;

        int numWidth = 0;
        while (sideWidth > 0) {

            if (numWidth++ > 10) break;

            int width;
            if (!left && !rightFlexDone) width = widthOverhead;
            else if (left && sideWidth < gridSize) width = widthOverhead;
            else width = gridSize;
            sideWidth -= width;
            rightFlexDone = true;

            GUILayout.BeginVertical();
            {
                int spaceHeight = sideHeight;

                int numHeight = 0;
                while (!secondHeightFlexDone) {
                    if (numHeight++ > 10) break;
                    int height;
                    if (!firstHeightFlexDone) height = heightOverhead;
                    else if (spaceHeight >= gridSize) height = gridSize;
                    else { height = heightOverhead; secondHeightFlexDone = true; }
                    spaceHeight -= height;
                    firstHeightFlexDone = true;
                    GUILayout.Button(black, GUILayout.Height(height - space.y), GUILayout.Width(width - space.x));
                }
            }
            GUILayout.EndVertical();
        }
    }

    void GUIDivider(int i_height = 1) {
        GUILayout.Label("");
        Rect rect = EditorGUILayout.GetControlRect(false, i_height);
        rect.height = i_height;
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
        GUILayout.Label("");
    }

    void GUIVerticalDivider(int height) {
        GUILayout.BeginVertical();
        for (int i = 0; i < height; i++) GUILayout.Label("|");
        GUILayout.EndVertical();
    }

    bool GUIVerticalBool(string text, bool input) {
        EditorGUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label(text);
                GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                bool result = GUILayout.Toggle(input, (Texture2D)null);
                if (text.StartsWith("Bench")) {
                    float xOffset = layout.BenchOffsetTiles.x;
                    float yOffset = layout.BenchOffsetTiles.y;
                    if (text.Contains(" X ")) {
                        if (!layout.BenchCentered) xOffset = EditorGUILayout.DelayedFloatField(xOffset);
                    }
                    if (text.Contains(" Y ")) {
                        if (!layout.DefaultYOffset) yOffset = EditorGUILayout.DelayedFloatField(yOffset);
                    }
                    layout.BenchOffsetTiles = new Vector2(xOffset, yOffset);
                }
                GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
        return result;
    }

}