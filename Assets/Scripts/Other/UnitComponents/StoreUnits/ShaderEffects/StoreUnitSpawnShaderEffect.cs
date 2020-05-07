using UnityEngine;
using static GameInfo;

public class StoreUnitSpawnShaderEffect : UnitShaderEffects {

    public struct SpawnShaderProperties {
        public SpawnShaderProperties(Color effectColor, float delay, int size = 200) {
            EffectColor = effectColor; Delay = delay; Size = size;
        }
        public Color EffectColor { get; private set; }
        public float Delay { get; private set; }
        public int Size { get; private set; }
    }

    public static readonly SpawnShaderProperties[] GlowByQuality = new SpawnShaderProperties[] {
        new SpawnShaderProperties(HDRColor.Glow[(int)Rarity.COMMON], 0.02f),
        new SpawnShaderProperties(HDRColor.Glow[(int)Rarity.UNCOMMON], 0.04f),
        new SpawnShaderProperties(HDRColor.Glow[(int)Rarity.RARE], 0.12f),
        new SpawnShaderProperties(HDRColor.Glow[(int)Rarity.EPIC], 0.25f),
        new SpawnShaderProperties(HDRColor.Glow[(int)Rarity.SECRET], 0.4f)
    };

    private SpawnShaderProperties glowShader;

    private readonly string
        AlphaFade = "_AlphaFade",
        TextureDelay = "_TextureDelay",
        Size = "_DissolveSize",
        Color = "_DissolveColor"
        ;

    private bool spawned = true;

    private void Start() { SubscribeLocalEventHandlers(); }

    private void Update() {
        if (spawned) return;
        float speed = 0.5f;
        foreach (Material m in Materials) {
            m.SetFloat(AlphaFade, m.GetFloat(AlphaFade) + speed * Time.deltaTime);
            if (m.GetFloat(AlphaFade) >= 2f + 0.05f) spawned = true;
        }
    }

    private void SubscribeLocalEventHandlers() {
        if (!BoltNetwork.IsClient) return;
        var clientStore = ClientStoreMan.Instance;
        clientStore.UnitArrivedInStoreEvent += HandleUnitArrivedInStoreEvent;
    }

    private void HandleUnitArrivedInStoreEvent(StoreUnit storeUnit, int idx) {
        if (!IsThis<StoreUnit>(storeUnit)) return;
        glowShader = GlowByQuality[(int) storeUnit.boardUnit.properties.rarity];
        foreach (Material m in Materials) {
            m.SetColor(Color, glowShader.EffectColor);
            m.SetFloat(Size, glowShader.Size);
            m.SetFloat(TextureDelay, glowShader.Delay);
            m.SetFloat(AlphaFade, 0.2f);
        }
        spawned = false;
    }

}