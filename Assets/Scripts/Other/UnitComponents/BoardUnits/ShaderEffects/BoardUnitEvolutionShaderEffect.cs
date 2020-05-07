using UnityEngine;
using System.Collections;
using static GameInfo;

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
    
    private bool isEvolving = false;

    private float speed = 0.5f;

    private new void Awake() {
        base.Awake();
        This<BoardUnit>().state.AddCallback("ShaderEvoFade", UpdateEvoParameters);
        This<BoardUnit>().state.AddCallback("ShaderEvoAlphaFade", UpdateEvoParameters);
    }

    private void Start() { if (BoltNetwork.IsServer) { SubscribeLocalEventHandlers(); } }

    private IEnumerator EvolveUnit() {
        yield return new WaitForSeconds(2 * speed * ShaderEvoFade);
        ShaderEvoAlphaFade =  1f;
        isEvolving = true;
        while (isEvolving) {
            ShaderEvoFade += speed * Time.deltaTime;
            if (ShaderEvoFade == 1f) isEvolving = false;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(0.1f);
        isEvolving = true;
        while (isEvolving) {
            ShaderEvoAlphaFade -= speed * Time.deltaTime;
            if (ShaderEvoAlphaFade == 0f) isEvolving = false;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator SpawnEvolvedUnit() {
        ShaderEvoFade =  1f;
        ShaderEvoAlphaFade = 1f;
        ShaderEvoAlphaFade = 0f;
        isEvolving = true;
        while (isEvolving) {
            ShaderEvoAlphaFade += speed * Time.deltaTime;
            if (ShaderEvoAlphaFade == 1f) isEvolving = false;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(0.1f);
        isEvolving = true;
        while (isEvolving) {
            ShaderEvoFade -= speed * Time.deltaTime;
            if (ShaderEvoFade == 0f) isEvolving = false;
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
        var board = player.GetPlayerMan<PlayerBoardMan>();
        board.EvolvingUnitEvent += HandleEvolvingUnitEvent;
        board.SpawnedEvolvedUnitEvent += HandleSpawnedEvolvedUnitEvent;
    }

    private void HandleEvolvingUnitEvent(BoardUnit unit) {
        if (!IsThis<BoardUnit>(unit)) return;
        StopAllCoroutines();
        StartCoroutine(EvolveUnit());
    }

    private void HandleSpawnedEvolvedUnitEvent(BoardUnit unit) {
        if (!IsThis<BoardUnit>(unit)) return;
        StartCoroutine(SpawnEvolvedUnit());
    }

}