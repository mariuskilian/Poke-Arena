using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitShaderEffects : UnitBehaviour {

    struct ShaderProperties {
        public Color EffectColor { get; private set; }
        public float Delay { get; private set; }
        public int Size { get; private set; }

        public ShaderProperties(Color effectColor, float delay, int size = 30) {
            EffectColor = effectColor;
            Delay = delay;
            Size = size;
        }
    }

    private UnitShaderEffects.ShaderProperties[] ShadersByQuality = new UnitShaderEffects.ShaderProperties[] {
        new UnitShaderEffects.ShaderProperties(HDRColor.Common, 0.025f),
        new UnitShaderEffects.ShaderProperties(HDRColor.Uncommon, 0.05f),
        new UnitShaderEffects.ShaderProperties(HDRColor.Rare, 0.1f),
        new UnitShaderEffects.ShaderProperties(HDRColor.Epic, 0.2f),
        new UnitShaderEffects.ShaderProperties(HDRColor.Legendary, 0.4f)
    };

    private ShaderProperties randomProp;

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
        InitEventSubscribers();
        materials = gameObject.GetComponentInChildren<Renderer>().materials;
    }

    private void Update() {
        if (!spawned) {
            foreach (Material m in materials) {
                if (m.name.Contains(IRIS)) continue;
                float speed = 0.2f;
                m.SetFloat(ALPHA_FADE, m.GetFloat(ALPHA_FADE) + speed * Time.deltaTime);
                if (m.GetFloat(ALPHA_FADE) >= 2f + 0.05f) spawned = true;
            }
        }
    }

    private void InitEventSubscribers() {
        StoreMan.Instance.NewUnitInStoreEvent += HandleNewUnitInStoreEvent;
    }

    private void HandleNewUnitInStoreEvent(Unit unit, int index) {
        if (!IsThisUnit(unit)) return;
        randomProp = ShadersByQuality[index];
        foreach (Material m in materials) {
            if (m.name == IRIS) continue;
            m.SetColor(COLOR, randomProp.EffectColor);
            m.SetFloat(SIZE, randomProp.Size);
            m.SetFloat(TEXTURE_DELAY, randomProp.Delay);
            m.SetFloat(ALPHA_FADE, 0.2f);
        }
        spawned = false;
    }
}
