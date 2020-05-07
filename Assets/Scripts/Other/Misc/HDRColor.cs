using UnityEngine;

public class HDRColor
{
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

    public static Color
        Secret = ConvertFromHDR(new HDRColor(6f, 191, 54, 0)),
        Epic = ConvertFromHDR(new HDRColor(5f, 144, 0, 191)),
        Rare = ConvertFromHDR(new HDRColor(3f, 0, 29, 255)),
        Uncommon = ConvertFromHDR(new HDRColor(2.5f, 8, 58, 0)),
        Common = ConvertFromHDR(new HDRColor(2f, 160, 160, 160));

    public static Color[] Glow = { Common, Uncommon, Rare, Epic, Secret };

    private static Color ConvertFromHDR(HDRColor hdrc) {
        Color c = new Vector4(hdrc.Red, hdrc.Green, hdrc.Blue, hdrc.Alpha);
        return c;
    }
}