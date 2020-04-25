﻿using UnityEngine;
using UnityEditor;
using System;
using System.Text.RegularExpressions;
using System.Globalization;
using System.IO;
using System.Collections.Generic;

[CustomEditor(typeof(GameSettings))]
public class GameSettingsEditor : Editor {

    private GameSettings gameSettings;

    private string[,] DropChance;
    private int maxLevel, numRarities;

    private readonly int height = 18;
    private readonly int width = 50;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        gameSettings = target as GameSettings;

        if (DropChance == null) {
            maxLevel = gameSettings.maxLevel;
            if (gameSettings.Rarities == null ) gameSettings.Rarities = new RarityInfo[0];
            numRarities = gameSettings.Rarities.Length;
            DropChance = new string[maxLevel, numRarities];
        }

        GUIDivider();

        GUILayout.BeginHorizontal();
        {
            GUILayout.BeginVertical();
            {
                GUILayout.Label("Rarity:", GUILayout.Height(height));
                GUILayout.Label("Pool Size:", GUILayout.Height(height));
            }
            GUILayout.EndVertical();

            for (int i = 0; i < gameSettings.Rarities.Length; i++) {
                GUILayout.BeginVertical();
                {
                    gameSettings.Rarities[i].rarity = (GameSettings.Rarity)EditorGUILayout.EnumPopup(gameSettings.Rarities[i].rarity, GUILayout.Height(height));

                    string poolSize = GUILayout.TextField(gameSettings.Rarities[i].poolSize.ToString(), GUILayout.Height(height));
                    poolSize = Regex.Replace(poolSize, "[^0-9]", "");
                    gameSettings.Rarities[i].poolSize = (poolSize == "") ? 0 : int.Parse(poolSize);
                }
                GUILayout.EndVertical();
            }
        }
        GUILayout.EndHorizontal();

        GUIDivider();

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
                    string rarityName = gameSettings.Rarities[rar].rarity.ToString();
                    if (rarityName == null || rarityName.Length == 0) rarityName = "";
                    if (rarityName.Length >= 4) rarityName = rarityName.Substring(0, Mathf.Max(4, rarityName.Length - (rarityName.Length / 2)));
                    if (rarityName != gameSettings.Rarities[rar].rarity.ToString()) rarityName += ".";

                    // Add Label for rarity name and corresponding Text Fields
                    GUILayout.Label(rarityName, GUILayout.MaxWidth(width), GUILayout.Height(height));
                    for (int lvl = 0; lvl < maxLevel; lvl++) {
                        string s = DropChance[lvl, rar];
                        if (s == null) s = "";
                        DropChance[lvl, rar] = Regex.Replace(GUILayout.TextField(s, 3, GUILayout.MaxWidth(width), GUILayout.Height(height)), "[^0-9]", "");
                    }
                }
                GUILayout.EndVertical();
            }

        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Update")) Update();
            if (GUILayout.Button("Format")) Format();
            if (GUILayout.Button("Clear")) Clear();
            if (GUILayout.Button("Save")) Save();
        }
        GUILayout.EndHorizontal();

    }

    private void Update() {
        maxLevel = gameSettings.maxLevel;
        numRarities = gameSettings.Rarities.Length;

        DropChance = new string[maxLevel, numRarities];

        if (gameSettings.DropChance == null) return;

        for (int lvl = 0; lvl < gameSettings.DropChance.array2D.Length; lvl++) {
            for (int rar = 0; rar < gameSettings.DropChance.array2D[lvl].row.Length; rar++) {
                int value = gameSettings.DropChance[lvl, rar];
                DropChance[lvl, rar] = (value == 0) ? "" : value.ToString();
            }
        }

        Debug.Log("Drop Chances Updated!");
    }

    private void Format() {
        int desSum = 100;

        for (int lvl = 0; lvl < maxLevel; lvl++) {
            int sum = 0;
            for (int rar = 0; rar < numRarities; rar++) sum += GetIntValue(lvl, rar);

            int overhead = desSum;
            for (int rar = 0; rar < numRarities; rar++) {
                int num = GetIntValue(lvl, rar);
                if (sum == 0) num++;
                num = (num * desSum) / ((sum == 0) ? numRarities : sum);
                overhead -= num;
                DropChance[lvl, rar] = (num == 0) ? "" : num.ToString();
            }

            int index = 0;
            while (overhead != 0) {
                int num = GetIntValue(lvl, index);
                num += (int)Mathf.Sign(overhead);
                DropChance[lvl, index] = (num == 0) ? "" : num.ToString();
                overhead -= (int)Mathf.Sign(overhead);
                index = ++index % numRarities;
            }
        }

        Debug.Log("Drop Chances Formatted!");
    }

    private void Clear() {
        for (int lvl = 0; lvl < maxLevel; lvl++) {
            for (int rar = 0; rar < numRarities; rar++) {
                DropChance[lvl, rar] = "";
            }
        }
    }

    private void Save() {
        gameSettings.DropChance = new Array2D(maxLevel, numRarities);

        for (int lvl = 0; lvl < maxLevel; lvl++) {
            for (int rar = 0; rar < numRarities; rar++) {
                int num = GetIntValue(lvl, rar);
                gameSettings.DropChance[lvl, rar] = num;
            }
        }

        Debug.Log("Drop Chances Saved!");
    }

    private int GetIntValue(int level, int rarity) {
        string s = DropChance[level, rarity];
        return (s == null || s == "") ? 0 : int.Parse(s);
    }

    void GUIDivider(int i_height = 1) {
        GUILayout.Label("");
        Rect rect = EditorGUILayout.GetControlRect(false, i_height);
        rect.height = i_height;
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
        GUILayout.Label("");
    }
}