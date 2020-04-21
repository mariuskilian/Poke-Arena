using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitShaderEffects : UnitBehaviour {

    struct ShaderProperties {
        public Color EffectColor { get; private set; }
        public float Delay { get; private set; }
        public int Size { get; private set; }

        public ShaderProperties(Color effectColor, float delay, int size = 200) {
            EffectColor = effectColor;
            Delay = delay;
            Size = size;
        }
    }

    private static readonly ShaderProperties[] GlowByQuality = new ShaderProperties[] {
        new ShaderProperties(HDRColor.Glow[GameSettings.Rarity.COMMON], 0.02f),
        new ShaderProperties(HDRColor.Glow[GameSettings.Rarity.UNCOMMON], 0.04f),
        new ShaderProperties(HDRColor.Glow[GameSettings.Rarity.RARE], 0.12f),
        new ShaderProperties(HDRColor.Glow[GameSettings.Rarity.EPIC], 0.25f),
        new ShaderProperties(HDRColor.Glow[GameSettings.Rarity.SECRET], 0.4f)
    };

    private ShaderProperties glowShader;

    private readonly string IRIS = "Iris";
    private readonly string
        ALPHA_FADE = "_AlphaFade",
        TEXTURE_DELAY = "_TextureDelay",
        SIZE = "_DissolveSize",
        COLOR = "_DissolveColor"
        ;

    private bool spawned = true;

    private Material[] materials;

    private void Start() {
        materials = gameObject.GetComponentInChildren<Renderer>().materials;
    }

    private void Update() {
        if (!spawned) {
            foreach (Material m in materials) {
                if (m.name.Contains(IRIS)) continue;
                float speed = 0.5f;
                m.SetFloat(ALPHA_FADE, m.GetFloat(ALPHA_FADE) + speed * Time.deltaTime);
                if (m.GetFloat(ALPHA_FADE) >= 2f + 0.05f) spawned = true;
            }
        }
    }

    // private void HandleNewUnitInStoreEvent(Unit unit, int index) {
    //     if (!IsThisUnit(unit)) return;
    //     glowShader = GlowByQuality[(int) unit.properties.rarity];
    //     foreach (Material m in materials) {
    //         if (m.name == IRIS) continue;
    //         m.SetColor(COLOR, glowShader.EffectColor);
    //         m.SetFloat(SIZE, glowShader.Size);
    //         m.SetFloat(TEXTURE_DELAY, glowShader.Delay);
    //         m.SetFloat(ALPHA_FADE, 0.2f);
    //     }
    //     spawned = false;
    // }
}
