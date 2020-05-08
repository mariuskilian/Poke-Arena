using UnityEngine;
using Bolt;
using System;
using System.Collections.Generic;
using System.Collections;

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

        Board = new Tile[Layout.BoardSizeTiles.x, Layout.BoardSizeTiles.y];
        for (int i = 0; i < Layout.BoardSizeTiles.x; i++) {
            for (int j = 0; j < Layout.BoardSizeTiles.y / 2; j++) {
                Board[i, j] = (Layout.board[i, j]) ? new Tile(i, j) : null;
            }
        }

        Bench = new Tile[Layout.BenchSizeTiles];
        for (int i = 0; i < Layout.BenchSizeTiles; i++) Bench[i] = new Tile(i, -1);
        ReserveTile = new Tile(Layout.BenchSizeTiles, -1);
    }

    private void InitUnitContainers() { UnitContainers = new Dictionary<string, UnitContainer>(); }

    private Tile FindTile(Vector3 worldPos, bool isBoardTile) {
        Vector3 localPos = player.transform.InverseTransformPoint(worldPos);
        Vector2 offset = ((isBoardTile) ? Layout.BoardOffsetWorld : Layout.BenchOffsetWorld) + Layout.TileOffset;
        localPos.x = (localPos.x - offset.x) / Layout.TileSize.x;
        localPos.z = (localPos.z - offset.y) / Layout.TileSize.y;

        Vector2Int tilePos = new Vector2Int(Mathf.RoundToInt(localPos.x), Mathf.RoundToInt(localPos.z));
        if (isBoardTile) {
            if (tilePos.x < 0 || tilePos.x >= Layout.BoardSizeTiles.x || tilePos.y < 0 || tilePos.y >= Layout.BoardSizeTiles.y / 2) return null;
            return Board[tilePos.x, tilePos.y];
        } else {
            if (tilePos.x < 0 || tilePos.x >= Layout.BenchSizeTiles) return null;
            return Bench[tilePos.x];
        }
    }
    #endregion

    #region Unit Containers
    private UnitContainer FindOrCreateUnitContainer(BoardUnit unit) {
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
    public bool TryEvolve(BoardUnit unit) {
        if (unit.evolution == null) return false;
        List<Tile> Tiles = FindOrCreateUnitContainer(unit).TryGetEvolvingUnits(unit.evolutionChain);
        if (Tiles == null) return false;
        EvolvingUnitsEvent?.Invoke(Tiles);
        return true;
    }
    #endregion

    public BoardUnit SpawnUnit(BoardUnit unitPrefab) {
        var unitEntity = BoltNetwork.Instantiate(unitPrefab.gameObject);
        unitEntity.AssignControl(player.Connection);
        BoardUnit unit = unitEntity.GetComponent<BoardUnit>();
        unit.SetClickable(true);
        unit.SetOwner(player);
        FindOrCreateUnitContainer(unit).TryAddUnit(unit);
        return unit;
    }

    public bool CanSpawnUnit(BoardUnit unit) {
        if (FindOrCreateUnitContainer(unit).IsReadyForEvolve(unit.evolutionChain)) return true;
        for (int i = 0; i < Layout.BenchSizeTiles; i++) if (!Bench[i].IsTileFilled) return true;
        return false;
    }

    #region Local Events
    public Action<BoardUnit> UnitPlacedEvent;
    public Action<BoardUnit> UnitTeleportedEvent;
    public Action<List<Tile>> EvolvingUnitsEvent;
    #endregion

    #region Local Event Handlers
    private void SubscribeLocalEventHandlers() {
        var store = player.GetPlayerMan<PlayerStoreMan>();
        store.UnitCaughtEvent += HandleUnitCaughtEvent;
    }

    private void HandleUnitCaughtEvent(StoreUnit storeUnit) {
        BoardUnit unit = SpawnUnit(storeUnit.boardUnit);

        bool benchHasFreeTile = false;
        for (int i = 0; i < Layout.BenchSizeTiles; i++) if (!Bench[i].IsTileFilled) {
                Bench[i].FillTile(unit);
                benchHasFreeTile = true;
                break;
            }
        if (!benchHasFreeTile) ReserveTile.FillTile(unit);

        TryEvolve(unit);
    }
    #endregion

    #region Global Event Handlers
    public override void OnEvent(ClientUnitDeselectEvent evnt) {
        if (!IsThisPlayer(evnt.RaisedBy)) return;
        var entity = BoltNetwork.FindEntity(evnt.Unit);
        if (entity == null || !entity.TryGetComponent<BoardUnit>(out BoardUnit unit)) return;
        if (!unit.entity.IsController(evnt.RaisedBy)) return;
        Tile tile = FindTile(evnt.ClickPosition, evnt.ClickedBoard);
        if (tile == null) unit.CurrentTile.ResetTile();
        else {
            UnitPlacedEvent?.Invoke(unit);
            if (tile.IsTileFilled) UnitTeleportedEvent?.Invoke(tile.CurrentUnit);
            Tile.SwapTiles(unit.CurrentTile, tile);
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Z)) {
            if (Bench[0].CurrentUnit != null && Bench[1].CurrentUnit != null) { Tile.SwapTiles(Bench[0], Bench[1]); }
        }
    }
    #endregion

}