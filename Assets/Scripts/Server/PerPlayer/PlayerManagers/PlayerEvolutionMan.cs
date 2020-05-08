using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using static GameInfo;

public class PlayerEvolutionMan : PlayerManager {

    public const float
        DurationGlowEffects = 0.6f,
        DurationAlphaEffects = 0.4f,
        MaxDelayBetweenUnits = 0.35f,
        GlowEffectThreshhold = 0.5f,
        AlphaEffectThreshhold = 0.6f
        ;

    private IEnumerator Evolve(List<BoardUnit> Units) {

        BoardUnit firstUnit = Units[0]; // The unit we will replace with the evolved Unit
        BoardUnit lastUnit = Units[Units.Count - 1]; // The reference unit to check if transitions are done

        // Start evolution for base units
        foreach (var unit in Units) {
            unit.SetClickable(false);
            EvolvingUnitEvent?.Invoke(unit);
            yield return new WaitForSeconds((float)RNG.NextDouble() * MaxDelayBetweenUnits);
        }

        // Wait until last one is ready
        while (lastUnit.state.ShaderEvoFade < GlowEffectThreshhold)
            yield return new WaitForEndOfFrame();

        // Spawn evolution
        BoardUnit evolution = firstUnit.evolution;

        StartCoroutine(DespawnEvolvingUnits(Units));
        yield return new WaitForSeconds(DurationAlphaEffects);

        BoardUnit evolvedUnit = player.GetPlayerMan<PlayerBoardMan>().SpawnUnit(evolution);
        evolvedUnit.SetClickable(false);
        yield return new WaitForEndOfFrame();
        Tile tile = firstUnit.CurrentTile;
        tile.ClearTile();
        tile.FillTile(evolvedUnit);
        StartCoroutine(SpawnEvolvedUnit(evolvedUnit));
    }

    private IEnumerator SpawnEvolvedUnit(BoardUnit evolvedUnit) {
        SpawningEvolvedUnitEvent?.Invoke(evolvedUnit);
        while (evolvedUnit.state.ShaderEvoAlphaFade < AlphaEffectThreshhold)
            yield return new WaitForEndOfFrame();
        
        evolvedUnit.SetClickable(true);
        if (!player.GetPlayerMan<PlayerBoardMan>().TryEvolve(evolvedUnit))
            FinalizeEvolvedSpawnEvent?.Invoke(evolvedUnit);
    }

    private IEnumerator DespawnEvolvingUnits(List<BoardUnit> Units) {
        foreach (var unit in Units) DespawningUnitEvent?.Invoke(unit);
        BoardUnit lastUnit = Units[Units.Count - 1];
        int numUnitsDestroyed = 0;
        while (numUnitsDestroyed < Units.Count) {
            List<BoardUnit> _Units = new List<BoardUnit>(Units);
            foreach (var unit in _Units) if (unit.state.ShaderEvoAlphaFade == 0f) {
                if (unit.CurrentTile.CurrentUnit == unit) unit.CurrentTile.ClearTile();
                Units.Remove(unit);
                BoltNetwork.Destroy(unit.gameObject);
                numUnitsDestroyed++;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public Action<BoardUnit> EvolvingUnitEvent;
    public Action<BoardUnit> DespawningUnitEvent;

    public Action<BoardUnit> SpawningEvolvedUnitEvent;
    public Action<BoardUnit> FinalizeEvolvedSpawnEvent;

    private void Start() { SubscribeLocalEventHandlers(); }

    private void SubscribeLocalEventHandlers() {
        var board = player.GetPlayerMan<PlayerBoardMan>();
        board.EvolvingUnitsEvent += HandleEvolvingUnitsEvent;
    }

    private void HandleEvolvingUnitsEvent(List<BoardUnit> Units) {
        StartCoroutine(Evolve(Units));
    }

}