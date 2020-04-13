using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitShaderEffects : UnitBehaviour {

    struct ShaderProperties {
        public Color EffectColor { get; private set; }
        public float Delay { get; private set; }
        public float Thickness { get; private set; }
        public int Size { get; private set; }

        public ShaderProperties(Color effectColor, float delay, float thickness, int size = 30) {
            EffectColor = effectColor;
            Delay = delay;
            Thickness = thickness;
            Size = size;
        }
    }

    private UnitShaderEffects.ShaderProperties[] shadersByQuality = new UnitShaderEffects.ShaderProperties[] {
        new UnitShaderEffects.ShaderProperties(Color.gray, 0f, 0f),
        new UnitShaderEffects.ShaderProperties(Color.green, 0f, 0.03f),
        new UnitShaderEffects.ShaderProperties(Color.blue, 0.1f, 0.03f),
        new UnitShaderEffects.ShaderProperties(Color.magenta, 0.3f, 0.03f),
        new UnitShaderEffects.ShaderProperties(Color.yellow, 0.6f, 0.03f)
    };

    private readonly string IRIS = "Iris", FIRE = "Fire";
    private readonly string
        ALPHA_FADE = "_AlphaFade",
        TEXTURE_FADE = "_TextureFade",
        THICKNESS = "_DissolveThickness",
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
                if (m.name.Contains(IRIS) || m.name.Contains(FIRE)) continue;
                float speed = 0.75f;

                m.SetFloat(ALPHA_FADE, m.GetFloat(ALPHA_FADE) + speed * Time.deltaTime);
                if (m.GetFloat(ALPHA_FADE) >= 0.6f)
                    m.SetFloat(TEXTURE_FADE, m.GetFloat(TEXTURE_FADE) + speed * Time.deltaTime);
                if (m.GetFloat(TEXTURE_FADE) >= 2f) {
                    m.SetFloat(ALPHA_FADE, 2f);
                    m.SetFloat(TEXTURE_FADE, 2f);
                    spawned = true;
                }
            }
        }
    }

    private void InitEventSubscribers() {
        StoreMan.Instance.NewUnitInStoreEvent += HandleNewUnitInStoreEvent;
    }

    private void HandleNewUnitInStoreEvent(Unit unit, int index) {
        if (!IsThisUnit(unit)) return;
        foreach (Material m in materials) {
            if (m.name == IRIS || m.name.Contains(FIRE)) continue;
            m.SetFloat(ALPHA_FADE, 0.2f);
            m.SetFloat(TEXTURE_FADE, 0f);
        }
        spawned = false;
    }
}
