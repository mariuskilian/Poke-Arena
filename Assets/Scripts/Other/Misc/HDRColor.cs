using UnityEngine;
using System.Collections.Generic;

public class HDRColor {
    public float Red { get; private set; }
    public float Green { get; private set; }
    public float Blue { get; private set; }
    public float Alpha { get; private set; }

    public HDRColor(float intensity, int red, int green, int blue, float alpha = 1.0f) {
        float factor = Mathf.Pow(2, intensity);
        Red = red * factor / 255f;
        Green = green * factor / 255f;
        Blue = blue * factor / 255f;
        Alpha = alpha;
    }

    public static Dictionary<GameSettings.Rarity, Color> Glow = new Dictionary<GameSettings.Rarity, Color>() {
        { GameSettings.Rarity.LEGENDARY, ConvertFromHDR(new HDRColor(7f, 255, 100, 0)) },
        { GameSettings.Rarity.SECRET, ConvertFromHDR(new HDRColor(5f, 191, 54, 0)) },
        { GameSettings.Rarity.EPIC, ConvertFromHDR(new HDRColor(4.8f, 144, 0, 191)) },
        { GameSettings.Rarity.RARE, ConvertFromHDR(new HDRColor(3.8f, 0, 29, 255)) },
        { GameSettings.Rarity.UNCOMMON, ConvertFromHDR(new HDRColor(3.2f, 8, 58, 0)) },
        { GameSettings.Rarity.COMMON, ConvertFromHDR(new HDRColor(2f, 160, 160, 160)) }
    };

    private static Color ConvertFromHDR(HDRColor hdrc) {
        Color c = new Vector4(hdrc.Red, hdrc.Green, hdrc.Blue, hdrc.Alpha);
        return c;
    }
}
