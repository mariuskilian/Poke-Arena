using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameMode))]
public class GameModeEditor : Editor {

    private GameMode mode;

    public override void OnInspectorGUI() {
        mode = target as GameMode;

        Undo.RecordObject(mode, "Game Mode value change");
        EditorUtility.SetDirty(mode);

        Vector2Int ArenaLayoutSize = Vector2Int.zero;
        if (mode.arenaLayout != null && mode.arenaLayout.IsInitialized) {
            ArenaLayoutSize.x = mode.arenaLayout.Length;
            if (ArenaLayoutSize.x > 0)
                ArenaLayoutSize.y = mode.arenaLayout[0].Length;
        }

        GUILayout.Label("");

        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("Arena layout size:", GUILayout.Width(Screen.width * 0.4f));
            GUILayout.Label("X", GUILayout.Width(15));
            ArenaLayoutSize.x = (int)Mathf.Clamp(EditorGUILayout.DelayedIntField(ArenaLayoutSize.x), 1, 5);
            GUILayout.Label("Y", GUILayout.Width(15));
            ArenaLayoutSize.y = (int)Mathf.Clamp(EditorGUILayout.DelayedIntField(ArenaLayoutSize.y), 1, 5);
        }
        GUILayout.EndHorizontal();

        if (mode.arenaLayout == null || !mode.arenaLayout.IsInitialized
            || mode.arenaLayout.array2D.Length == 0
            || mode.arenaLayout.array2D.Length != ArenaLayoutSize.x
            || mode.arenaLayout.array2D[0].Length != ArenaLayoutSize.y) {
            mode.arenaLayout = new Array2DArena(ArenaLayoutSize.x, ArenaLayoutSize.y);
        }

        GUIDivider();

        int space = 2;
        int boxSize = (int)Mathf.Min(75, (float)(Screen.width - 40) / (float)ArenaLayoutSize.x);
        boxSize = (boxSize % 2 == 0) ? boxSize - 1 : boxSize;
        GUILayoutOption
            width = GUILayout.Width(boxSize),
            height = GUILayout.Height(boxSize)
            ;

        int areaSize = (boxSize + space) * ArenaLayoutSize.x;
        Rect area = new Rect((Screen.width - areaSize - 20) / 2, 40, areaSize, areaSize);

        Texture2D
            unused = MakeTexture(boxSize, space, false, false),
            singleUsed = MakeTexture(boxSize, space, true, false),
            doubleUsed = MakeTexture(boxSize, space, true, true)
            ;

        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            for (int i = 0; i < ArenaLayoutSize.x; i++) {
                EditorGUILayout.BeginVertical();
                {
                    for (int j = ArenaLayoutSize.y - 1; j >= 0; j--) {
                        bool active = mode.arenaLayout[i, j].active;
                        bool shared = mode.arenaLayout[i, j].shared;

                        Texture2D tex;
                        if (!active) tex = unused;
                        else if (!shared) tex = singleUsed;
                        else tex = doubleUsed;

                        if (GUILayout.Button(tex)) {
                            if (!active) {
                                mode.arenaLayout[i, j].active = true;
                                mode.arenaLayout[i, j].shared = false;
                            } else if (!shared) {
                                mode.arenaLayout[i, j].active = true;
                                mode.arenaLayout[i, j].shared = true;
                            } else {
                                mode.arenaLayout[i, j].active = false;
                                mode.arenaLayout[i, j].shared = false;
                            }
                        }
                    }
                }
                EditorGUILayout.EndVertical();
            }
            GUILayout.FlexibleSpace();
        }
        EditorGUILayout.EndHorizontal();
    }

    private Texture2D MakeTexture(int boxSize, int space, bool arenaActive, bool sharedArena) {
        var texture = new Texture2D(boxSize, boxSize, TextureFormat.ARGB32, false);
        int size = boxSize - 2 * space;
        int radius = size / 2;
        int outline = (int)Mathf.Max(1, size / 25);

        int belt = size / 15;
        int outerInnerRadius = radius / 3;
        int innerInnerRadius = radius / 5;
        int innerInnerInnerRadius = radius / 6;

        int playerSize = (int)innerInnerRadius * 2 + 1;
        int playerRadius = (int)playerSize / 2;
        var playerTex = new Texture2D(playerSize, playerSize, TextureFormat.ARGB32, false);
        for (int i = 0; i < playerSize; i++) {
            for (int j = 0; j < playerSize; j++) {
                Color c = Color.blue;
                int x = i - playerRadius;
                int y = j - playerRadius;
                float distance = Mathf.Sqrt(x * x + y * y);
                c.a = Mathf.Clamp(playerRadius - distance, 0f, 1f);
                playerTex.SetPixel(i, j, c);
            }
        }
        playerTex.Apply();

        for (int i = 0; i < boxSize; i++) {
            for (int j = 0; j < boxSize; j++) {
                Color c = Color.green;

                int x = i - radius - space;
                int y = j - radius - space;

                if (y == 0 || (y < belt && y > -belt)) c = Color.black;
                else if (y > 0) c = Color.red;
                else c = Color.white;

                float distance = Mathf.Sqrt(x * x + y * y);

                if (distance > radius - 1) c = MixColors(c, Color.black, radius, distance);

                if (distance < outerInnerRadius + 1) c = MixColors(c, Color.black, distance, outerInnerRadius);
                if (distance < innerInnerRadius + 1) c = MixColors(c, Color.white, distance, innerInnerRadius);
                if (distance < innerInnerInnerRadius) c = MixColors(c, Color.black, distance, innerInnerInnerRadius);

                c.a = Mathf.Clamp(radius - distance + outline, 0f, 1f);

                if (!arenaActive) c.a = c.a / 10f;

                texture.SetPixel(i, j, c);

            }
        }

        if (arenaActive) {
            int xOffset = (boxSize - playerSize) / 2;
            int yOffset = xOffset - (int) (boxSize / 3.5f);

            for (int i = 0; i < playerSize; i++) {
                for (int j = 0; j < playerSize; j++) {
                    Color c;
                    Color c1 = playerTex.GetPixel(i, j);

                    Color c2Down = texture.GetPixel(i + xOffset, j + yOffset);
                    c = BlendColors(c1, c2Down, c1.a);
                    c.a = c2Down.a;
                    texture.SetPixel(i + xOffset, j + yOffset, c);

                    if (sharedArena) {
                        Color c2Up = texture.GetPixel(i + xOffset, boxSize - 1 - j - yOffset);
                        c = BlendColors(c1, c2Up, c1.a);
                        c.a = c2Up.a;
                        texture.SetPixel(i + xOffset, boxSize - 1 - j - yOffset, c);
                    }
                }
            }
        }

        texture.Apply();

        return texture;
    }

    private Color BlendColors(Color c1, Color c2, float factor) {
        c1.r = factor * c1.r + (1 - factor) * c2.r;
        c1.g = factor * c1.g + (1 - factor) * c2.g;
        c1.b = factor * c1.b + (1 - factor) * c2.b;
        return c1;
    }

    private Color MixColors(Color c1, Color c2, float radius, float distance) {
        return BlendColors(c1, c2, radius - distance);
    }

    void GUIDivider(int i_height = 1) {
        GUILayout.Label("");
        Rect rect = EditorGUILayout.GetControlRect(false, i_height);
        rect.height = i_height;
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
        GUILayout.Label("");
    }

}