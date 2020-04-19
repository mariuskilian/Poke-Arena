using UnityEngine;
using UnityEditor;
using System;
using System.Text.RegularExpressions;
using System.Globalization;
using System.IO;

[CustomEditor(typeof(GameSettings))]
public class GameSettingsEditor : Editor {

    private GameSettings gameSettings;

    private int numRarities, maxLevel;

    private readonly int height = 18;
    private readonly int width = 50;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        gameSettings = target as GameSettings;

        GUIDivider();

        UpdateDropChances();

        // Begin Drop Chance Table (drawn column-wise)
        GUILayout.BeginHorizontal();
        {

            // Draw all Level name side-headers
            GUILayout.BeginVertical();
            {
                GUILayout.Label("Drop Chances", GUILayout.Height(height));
                for (int i = 0; i < maxLevel; i++) {
                    GUILayout.Label("Level " + (i + 1), GUILayout.Height(height));
                }
            }
            GUILayout.EndVertical();

            // Draw all input boxes with their rarity as a header
            for (int rar = 0; rar < numRarities; rar++) {
                GUILayout.BeginVertical();
                {
                    // Format rarity name to make it look nicer
                    string rarityName = gameSettings.Rarities[rar];
                    if (rarityName == null || rarityName.Length == 0) rarityName = "";
                    if (rarityName.Length >= 4) rarityName = rarityName.Substring(0, Mathf.Max(4, rarityName.Length - (rarityName.Length / 2)));
                    if (rarityName.Length != rar.ToString().Length) rarityName += ".";

                    // Add Label for rarity name and corresponding Text Fields
                    GUILayout.Label(rarityName, GUILayout.MaxWidth(width), GUILayout.Height(height));
                    for (int lvl = 0; lvl < maxLevel; lvl++) {
                        string s = gameSettings.DropChances[lvl, rar].ToString();
                        s = GUILayout.TextField(s, 3, GUILayout.MaxWidth(width), GUILayout.Height(height));
                        s = Regex.Replace(s, "[^0-9]", "");
                        if (s.Length == 0) s = 0.ToString();
                        gameSettings.DropChances[lvl, rar] = int.Parse(s);
                    }
                }
                GUILayout.EndVertical();
            }

        }
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Format")) gameSettings.DropChances.Format(100);

    }

    private void UpdateDropChances() {
        if (gameSettings.DropChances == null) {
            maxLevel = gameSettings.Rarities.Count;
            maxLevel = gameSettings.maxLevel;
            gameSettings.DropChances = new Array2D(maxLevel, numRarities);
        }

        // Check if array size changed, then manually create new array, keeping all possible values
        if (numRarities != gameSettings.Rarities.Count || maxLevel != gameSettings.maxLevel) {
            Array2D tmp = gameSettings.DropChances;
            gameSettings.DropChances = new Array2D(gameSettings.maxLevel, gameSettings.Rarities.Count);
            for (int lvl = 0; lvl < gameSettings.maxLevel; lvl++) {
                for (int rar = 0; rar < gameSettings.Rarities.Count; rar++) {
                    int value = (rar >= numRarities || lvl >= maxLevel) ? 0 : tmp[lvl, rar];
                    gameSettings.DropChances[lvl, rar] = value;
                }
            }
            numRarities = gameSettings.Rarities.Count;
            maxLevel = gameSettings.maxLevel;
        }
    }

        /*gameSettings = target as GameSettings;

        GUIDivider();

        // Check if array size changed
        int numRaritiesNew = gameSettings.Rarities.Count;
        int numLevelsNew = gameSettings.numLevels;
        if (numRarities != numRaritiesNew || numLevels != numLevelsNew || DropChances == null)
            Resize(numRaritiesNew, numLevelsNew);

        // Begin Drop Chance Table (drawn column-wise)
        GUILayout.BeginHorizontal();
        {

            // Draw all Level name side-headers
            GUILayout.BeginVertical();
            {
                GUILayout.Label("Drop Chances", GUILayout.Height(height));
                for (int i = 0; i < numLevels; i++) {
                    GUILayout.Label("Level " + (i + 1), GUILayout.Height(height));
                }
            }
            GUILayout.EndVertical();

            // Draw all input boxes with their rarity as a header
            for (int rar = 0; rar < numRarities; rar++) {
                GUILayout.BeginVertical();
                {
                    // Format rarity name to make it look nicer
                    string rarityName = gameSettings.Rarities[rar];
                    rarityName = rarityName.Substring(0, Mathf.Max(4, rarityName.Length - (rarityName.Length / 2)));
                    if (rarityName.Length != rar.ToString().Length) rarityName += ".";

                    // Add Label for rarity name and corresponding Text Fields
                    GUILayout.Label(rarityName, GUILayout.MaxWidth(width), GUILayout.Height(height));
                    for (int lvl = 0; lvl < numLevels; lvl++) {
                        DropChances[lvl, rar] = GUILayout.TextField(
                            DropChances[lvl, rar],
                            3,
                            GUILayout.MaxWidth(width),
                            GUILayout.Height(height)
                        );
                    }
                }
                GUILayout.EndVertical();
            }

        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Format")) Format();
            if (GUILayout.Button("Save")) Save();
        }
        GUILayout.EndHorizontal();

    }

    private void Resize(int numRaritiesNew, int numLevelsNew) {
        DropChances = new string[numRaritiesNew, numLevelsNew];
        for (int rar = 0; rar < numRaritiesNew; rar++) {
            for (int lvl = 0; lvl < numLevelsNew; lvl++) {
                if (rar < gameSettings.Rarities.Count && lvl < gameSettings.numLevels) {
                    string value = "";
                    if (gameSettings.DropChances != null) value = gameSettings.DropChances[lvl, rar].ToString();
                    DropChances[lvl, rar] = value;
                }
            }
        }
        numRarities = numRaritiesNew;
        numLevels = numLevelsNew;
    }

    private void Format() {
        for (int lvl = 0; lvl < numLevels; lvl++) {

            // Stores the sum of current row, to make sure it doesn't exceed 100
            int rowBuffer = 100;
            for (int rar = 0; rar < numRarities; rar++) {

                // Format 2d Array
                string s = DropChances[lvl, rar];
                if (s.Length == 0) s = "0";
                int val = Mathf.Min(int.Parse(s), rowBuffer);
                rowBuffer -= val;

                // Overwrite 2d Array
                DropChances[lvl, rar] = val.ToString();
            }

            // Adds the remaining row buffer to the first entry of the row
            DropChances[0, lvl] = (int.Parse(DropChances[0, lvl]) + rowBuffer).ToString();
        }
        Debug.Log("Game Settings formatted, so that each row sums up to 100.");
    }

    private void Save() {
        Format();
        //gameSettings.DropChances = new int[numRarities, numLevels];

        for (int i = 0; i < numRarities; i++) {
            for (int j = 0; j < numLevels; j++) {
                //gameSettings.DropChances[i, j] = int.Parse(DropChances[i, j]);
            }
        }
        Debug.Log("Game Settings saved successfully!");
    }*/

    void GUIDivider(int i_height = 1) {
        GUILayout.Label("");
        Rect rect = EditorGUILayout.GetControlRect(false, i_height);
        rect.height = i_height;
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
        GUILayout.Label("");
    }
}