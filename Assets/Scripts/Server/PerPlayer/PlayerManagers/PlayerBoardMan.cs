using UnityEngine;
using Bolt;
using System;
using System.Collections.Generic;

public class PlayerBoardMan : PlayerManager {

    public static ArenaLayout Layout { get; private set; }

    public Tile[,] Board { get; private set; } // null if invalid Tile
    public Tile[] Bench { get; private set; }
    private Tile ReserveTile;

    private Dictionary<string, UnitContainer> UnitContainers;

    private void Start() {
        if (Layout == null) Layout = DataHolder.Instance.ArenaLayouts[0];
        InitBoardAndBench();
        InitUnitContainers();
        SubscribeLocalEventHandlers();
    }

    #region Initialization
    private void InitBoardAndBench() {
        Layout = DataHolder.Instance.ArenaLayouts[0];

        Board = new Tile[Layout.BoardSize.x, Layout.BoardSize.y];
        for (int i = 0; i < Layout.BoardSize.x; i++) {
            for (int j = 0; j < Layout.BoardSize.y / 2; j++) {
                Board[i, j] = (Layout.board[i, j]) ? new Tile(i, j) : null;
            }
        }

        Bench = new Tile[Layout.BenchSize];
        for (int i = 0; i < Layout.BenchSize; i++) Bench[i] = new Tile(i, -1);
        ReserveTile = new Tile(Layout.BenchSize, -1);
    }

    private void InitUnitContainers() { UnitContainers = new Dictionary<string, UnitContainer>(); }
    #endregion

    #region Unit Containers
    private UnitContainer FindOrCreateUnitContainer(Unit unit) {
        if (UnitContainers.TryGetValue(unit.properties.name, out var container)) return container;

        GameObject containerObject = new GameObject(unit.properties.name);
        containerObject.AddComponent<UnitContainer>();
        containerObject.transform.SetParent(player.Team.transform);
        containerObject.transform.localPosition = Vector3.zero;
        containerObject.transform.localRotation = Quaternion.identity;
        UnitContainers.Add(unit.properties.name, containerObject.GetComponent<UnitContainer>());
        return UnitContainers[unit.properties.name];
    }
    #endregion

    #region Evolution
    private void CheckForEvolution(Unit unit) {
        if (unit.evolution == null) return;
        List<Tile> Tiles = FindOrCreateUnitContainer(unit).CheckForEvolution(unit.evolutionChain);
        if (Tiles != null) Evolve(Tiles);
    }

    private void Evolve(List<Tile> Tiles) {
        Unit evolvedUnit = SpawnUnit(Tiles[0].CurrentUnit.evolution);
        foreach (Tile t in Tiles) {
            Unit unit = t.ClearTile();
            BoltNetwork.Destroy(unit.gameObject);
        }
        Tiles[0].FillTile(evolvedUnit);
        CheckForEvolution(evolvedUnit);
    }
    #endregion

    private Unit SpawnUnit(Unit unitPrefab) {
        var unitEntity = BoltNetwork.Instantiate(unitPrefab.gameObject);
        unitEntity.AssignControl(player.Connection);
        Unit unit = unitEntity.GetComponent<Unit>();
        FindOrCreateUnitContainer(unit).TryAddUnit(unit);
        return unit;
    }

    public bool CanSpawnUnit(Unit unit) {
        if (FindOrCreateUnitContainer(unit).IsReadyForEvolve) return true;
        for (int i = 0; i < Layout.BenchSize; i++) if (!Bench[i].IsTileFilled) return true;
        return false;
    }

    #region Local Event Handlers
    private void SubscribeLocalEventHandlers() {
        var store = player.GetPlayerMan<PlayerStoreMan>();
        store.UnitCaughtEvent += HandleUnitCaughtEvent;
    }

    private void HandleUnitCaughtEvent(StoreUnit storeUnit) {
        Unit unit = SpawnUnit(storeUnit.unit);

        bool benchHasFreeTile = false;
        for (int i = 0; i < Layout.BenchSize; i++) if (!Bench[i].IsTileFilled) {
                Bench[i].FillTile(unit);
                benchHasFreeTile = true;
                break;
            }
        if (!benchHasFreeTile) ReserveTile.FillTile(unit);

        CheckForEvolution(unit);
    }
    #endregion

}