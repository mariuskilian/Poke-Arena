using UnityEngine;
using UnityEditor;
using System;
using System.Text.RegularExpressions;
using System.Globalization;
using System.IO;
using System.Collections.Generic;
using static GameInfo;

[CustomEditor(typeof(GameSettings))]
public class GameSettingsEditor : Editor {

    private GameSettings gameSettings;

    private readonly GUILayoutOption
        height = GUILayout.Height(18),
        maxWidth = GUILayout.MaxWidth(50)
        ;

    public override void OnInspectorGUI() {
        gameSettings = target as GameSettings;

        if (gameSettings.DropChance == null || gameSettings.DropChance.array2D == null
            || gameSettings.DropChance.array2D.Length != NumLevels
            || gameSettings.DropChance.array2D[0].row.Length != NumRarities)
                gameSettings.DropChance = new Array2DInt(NumLevels, NumRarities);
        if (gameSettings.PoolSize == null || gameSettings.PoolSize.Length != NumRarities)
            gameSettings.PoolSize = new int[NumRarities];

        // Begin Drop Chance Table (drawn column-wise)
        GUILayout.BeginHorizontal();
        {

            // Draw all Level name side-headers
            GUILayout.BeginVertical();
            {
                GUILayout.Label("Pool Sizes:", height);
                GUILayout.Label("Rarities:", height);
                for (int i = 0; i < NumLevels; i++) {
                    GUILayout.Label("Level " + (i + 1), height);
                }
            }
            GUILayout.EndVertical();

            // Draw all input boxes with their rarity as a header
            foreach (Rarity r in Rarities) {
                GUILayout.BeginVertical();
                {
                    string poolSize = GUILayout.TextField(gameSettings.PoolSize[(int)r].ToString(), maxWidth, height);
                    poolSize = Regex.Replace(poolSize, "[^0-9]", "");
                    gameSettings.PoolSize[(int)r] = (poolSize == "") ? 0 : int.Parse(poolSize);

                    // Format rarity name to make it look nicer
                    string rarityName = r.ToString();
                    if (rarityName == null || rarityName.Length == 0) rarityName = "";
                    if (rarityName.Length >= 4) rarityName = rarityName.Substring(0, Mathf.Max(4, rarityName.Length - (rarityName.Length / 2)));
                    if (rarityName != r.ToString()) rarityName += ".";

                    // Add Label for rarity name and corresponding Text Fields
                    GUILayout.Label(rarityName, maxWidth, height);
                    for (int lvl = 0; lvl < NumLevels; lvl++) {
                        string s = gameSettings.DropChance[lvl, (int)r].ToString();
                        if (s == null) s = "";
                        s = Regex.Replace(GUILayout.TextField(s, 3, maxWidth, height), "[^0-9]", "");
                        gameSettings.DropChance[lvl, (int)r] = (s == "") ? 0 : int.Parse(s);
                    }
                }
                GUILayout.EndVertical();
            }

        }
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Format")) gameSettings.DropChance.Format(100);

    }

    void GUIDivider(int i_height = 1) {
        GUILayout.Label("");
        Rect rect = EditorGUILayout.GetControlRect(false, i_height);
        rect.height = i_height;
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
        GUILayout.Label("");
    }
}