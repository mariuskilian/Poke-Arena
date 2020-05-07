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

        Undo.RecordObject(gameSettings, "Game Settings value changed");
        EditorUtility.SetDirty(gameSettings);

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
            foreach (Rarity r in ArrayOfEnum<Rarity>()) {
                GUILayout.BeginVertical();
                {
                    int poolSize = EditorGUILayout.DelayedIntField(gameSettings.PoolSize[(int)r]);
                    gameSettings.PoolSize[(int)r] = Mathf.Clamp(poolSize, 0, 100);

                    // Format rarity name to make it look nicer
                    string rarityName = r.ToString();
                    if (rarityName == null || rarityName.Length == 0) rarityName = "";
                    if (rarityName.Length >= 4) rarityName = rarityName.Substring(0, Mathf.Max(4, rarityName.Length - (rarityName.Length / 2)));
                    if (rarityName != r.ToString()) rarityName += ".";

                    // Add Label for rarity name and corresponding Text Fields
                    GUILayout.Label(rarityName, maxWidth, height);
                    for (int lvl = 0; lvl < NumLevels; lvl++) {
                        int dropChance = EditorGUILayout.DelayedIntField(gameSettings.DropChance[lvl, (int)r]);
                        gameSettings.DropChance[lvl, (int)r] = Mathf.Clamp(dropChance, 0, 100);
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