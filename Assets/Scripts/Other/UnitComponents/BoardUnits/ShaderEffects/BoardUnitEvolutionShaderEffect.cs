using UnityEngine;
using System.Collections;
using static PlayerEvolutionMan;

public class BoardUnitEvolutionShaderEffect : UnitShaderEffects {

    public float ShaderEvoFade {
        get { return This<BoardUnit>().state.ShaderEvoFade; }
        set { This<BoardUnit>().state.ShaderEvoFade = Mathf.Clamp(value, 0f, 1f); }
    }
    public float ShaderEvoAlphaFade {
        get {return This<BoardUnit>().state.ShaderEvoAlphaFade; }
        set { This<BoardUnit>().state.ShaderEvoAlphaFade = Mathf.Clamp(value, 0f, 1f); }
    }

    private readonly string
        EvoFade = "_EvoFade",
        EvoAlphaFade = "_EvoAlphaFade"
        ;

    private new void Awake() {
        base.Awake();
        This<BoardUnit>().state.AddCallback("ShaderEvoFade", UpdateEvoParameters);
        This<BoardUnit>().state.AddCallback("ShaderEvoAlphaFade", UpdateEvoParameters);
    }

    private void Start() { if (BoltNetwork.IsServer) {
        ShaderEvoFade = 1f; // Dummy to trigger Callback
        ShaderEvoFade = 0f;
        ShaderEvoAlphaFade = 0f; // Dummy to trigger Callback
        ShaderEvoAlphaFade = 1f;
        SubscribeLocalEventHandlers();
    } }

    // Handles to avoid race conditions between Coroutines affecting ShaderEvoFade and ShaderEvoAlphaFade;
    private Coroutine _evoFadeAlphaCoroutine;
    private IEnumerator EvoFadeAlphaCoroutine {
        set {
            if (_evoFadeAlphaCoroutine != null) StopCoroutine(_evoFadeAlphaCoroutine);
            _evoFadeAlphaCoroutine = StartCoroutine(value);
        }
    }
    private Coroutine _evoFadeCoroutine;
    private IEnumerator EvoFadeCoroutine {
        set {
            if (_evoFadeCoroutine != null) StopCoroutine(_evoFadeCoroutine);
            _evoFadeCoroutine = StartCoroutine(value);
        }
    }

    private IEnumerator EvolveUnit() {
        yield return new WaitForSeconds(SpeedOfEffects * ShaderEvoFade); // In case ShaderEvoFade doesn't quite start at 0, so that timings remain intact
        while (true) {
            ShaderEvoFade += SpeedOfEffects * Time.deltaTime;
            if (ShaderEvoFade == 1f) break;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator DespawnEvolvingUnit() {
        yield return new WaitForSeconds(SpeedOfEffects * (1 - ShaderEvoAlphaFade)); // In case ShaderEvoAlphaFade doesn't quite start at 1, so that timings remain intact
        while (true) {
            ShaderEvoAlphaFade -= SpeedOfEffects * Time.deltaTime;
            if (ShaderEvoAlphaFade == 0f) break;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator SpawnEvolvedUnit() {
        ShaderEvoFade =  1f;
        ShaderEvoAlphaFade = 1f;
        ShaderEvoAlphaFade = 0f;
        while (true) {
            ShaderEvoAlphaFade += SpeedOfEffects * Time.deltaTime;
            if (ShaderEvoAlphaFade == 1f) break;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator FinalizeEvolvedUnit() {
        yield return new WaitForSeconds(SpeedOfEffects * (1 - ShaderEvoFade)); // In case ShaderEvoFade doesn't quite start at 1, so that timings remain intact
        while (true) {
            ShaderEvoFade -= SpeedOfEffects * Time.deltaTime;
            if (ShaderEvoFade == 0f) break;
            yield return new WaitForEndOfFrame();
        }
    }

    private void UpdateEvoParameters() {
        foreach (var m in Materials) {
            m.SetFloat(EvoFade, ShaderEvoFade);
            m.SetFloat(EvoAlphaFade, ShaderEvoAlphaFade);
        }
    }

    private void SubscribeLocalEventHandlers() {
        Player player = This<BoardUnit>().Owner;
        var evolution = player.GetPlayerMan<PlayerEvolutionMan>();
        evolution.EvolvingUnitEvent += HandleEvolvingUnitEvent;
        evolution.DespawningUnitEvent += HandleDespawningUnitEvent;
        evolution.SpawningEvolvedUnitEvent += HandleSpawningEvolvedUnitEvent;
        evolution.FinalizeEvolvedSpawnEvent += HandleFinalizeEvolvedSpawnEvent;
    }

    private void HandleEvolvingUnitEvent(BoardUnit unit) {
        if (!IsThis<BoardUnit>(unit)) return;
        EvoFadeCoroutine = EvolveUnit();
    }

    private void HandleDespawningUnitEvent(BoardUnit unit) {
        if (!IsThis<BoardUnit>(unit)) return;
        EvoFadeAlphaCoroutine = DespawnEvolvingUnit();
    }

    private void HandleSpawningEvolvedUnitEvent(BoardUnit unit) {
        if (!IsThis<BoardUnit>(unit)) return;
        EvoFadeAlphaCoroutine = SpawnEvolvedUnit();
    }

    private void HandleFinalizeEvolvedSpawnEvent(BoardUnit unit) {
        if (!IsThis<BoardUnit>(unit)) return;
        EvoFadeCoroutine = FinalizeEvolvedUnit();
    }

}