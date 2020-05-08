using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using static GameInfo;

public class PlayerEvolutionMan : PlayerManager {

    public const float
        SpeedOfEffects = 0.6f,
        MaxDelayBetweenUnits = 0.35f,
        EffectThreshhold = 0.5f
        ;

    private IEnumerator Evolve(List<Tile> Tiles) {

        BoardUnit firstUnit = Tiles[0].CurrentUnit; // The unit we will replace with the evolved Unit
        BoardUnit lastUnit = Tiles[Tiles.Count - 1].CurrentUnit; // The reference unit to check if transitions are done
        List<BoardUnit> Units = new List<BoardUnit>();

        // Start evolution for base units
        foreach (var t in Tiles) {
            BoardUnit unit = t.CurrentUnit;
            unit.SetClickable(false);
            EvolvingUnitEvent?.Invoke(unit);
            Units.Add(unit);
            yield return new WaitForSeconds((float)RNG.NextDouble() * MaxDelayBetweenUnits);
        }

        // Wait until last one is ready
        while (lastUnit.state.ShaderEvoFade < EffectThreshhold)
            yield return new WaitForEndOfFrame();

        // Spawn evolution
        BoardUnit evolution = firstUnit.evolution;

        StartCoroutine(DespawnEvolvingUnits(Units));
        yield return new WaitForSeconds(SpeedOfEffects);

        BoardUnit evolvedUnit = player.GetPlayerMan<PlayerBoardMan>().SpawnUnit(evolution);
        evolvedUnit.SetClickable(false);
        yield return new WaitForEndOfFrame();
        Tiles[0].ClearTile();
        Tiles[0].FillTile(evolvedUnit);
        StartCoroutine(SpawnEvolvedUnit(evolvedUnit));
    }

    private IEnumerator SpawnEvolvedUnit(BoardUnit evolvedUnit) {
        SpawningEvolvedUnitEvent?.Invoke(evolvedUnit);
        while (evolvedUnit.state.ShaderEvoAlphaFade < EffectThreshhold)
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

    private void HandleEvolvingUnitsEvent(List<Tile> Tiles) {
        StartCoroutine(Evolve(Tiles));
    }

}