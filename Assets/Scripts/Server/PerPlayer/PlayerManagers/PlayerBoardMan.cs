using UnityEngine;
using Bolt;
using System;
using System.Collections.Generic;

public class PlayerBoardMan : PlayerManager {

    public const int
        BoardWidth = 10, BoardHeight = 10,
        BenchSize = 10, BenchYOffset = -2
        ;
    public const float
        BenchXOffset = (BoardWidth - BenchSize) / 2f,
        TileSize = 1, TileOffset = 0.5f,
        DragUnitZOffset = -0.3f
        ;

    public Tile[,] Board { get; private set; } // null if invalid Tile
    public Tile[] Bench { get; private set; }
    private Tile ReserveTile;

    private Dictionary<string, UnitContainer> UnitContainers;

    private void Start() { InitBoard(); InitBench(); InitUnitContainers(); SubscribeLocalEventHandlers(); }

    #region Initialization
    private void InitBoard() {
        Board = new Tile[BoardWidth, BoardHeight / 2];
        for (int i = 0; i < BoardWidth; i++) {
            for (int j = 0; j < BoardHeight / 2; j++) {
                if ((i == 0 || i == BoardWidth - 1) && j < 3) Board[i, j] = null;
                else if (j == 0 && (i < 3 || i > BoardWidth - 4)) Board[i, j] = null;
                else Board[i, j] = new Tile(i, j);
            }
        }
    }

    private void InitBench() {
        Bench = new Tile[BenchSize];
        for (int i = 0; i < BenchSize; i++) Bench[i] = new Tile(i, -1);
        ReserveTile = new Tile(BenchSize, -1);
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
            // TODO: Revoke control / destroy unit
        }
        // TODO: Properly spawn evolved Unit
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
        for (int i = 0; i < BenchSize; i++) if (!Bench[i].IsTileFilled) return true;
        // TODO: Check for evolution
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
        for (int i = 0; i < BenchSize; i++) if (!Bench[i].IsTileFilled) {
            Bench[i].FillTile(unit);
            benchHasFreeTile = true;
            break;
        }
        if (!benchHasFreeTile) ReserveTile.FillTile(unit);
        
        CheckForEvolution(unit);
    }
    #endregion

}