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
    private void CheckForEvolution(BoardUnit unit) {
        if (unit.evolution == null) return;
        List<Tile> Tiles = FindOrCreateUnitContainer(unit).CheckForEvolution(unit.evolutionChain);
        if (Tiles != null) StartCoroutine(Evolve(Tiles));
    }

    private IEnumerator Evolve(List<Tile> Tiles) {

        BoardUnit firstUnit = Tiles[0].CurrentUnit;
        BoardUnit lastUnit = Tiles[Tiles.Count - 1].CurrentUnit;
        List<BoardUnit> Units = new List<BoardUnit>();

        // Start evolution for base units
        foreach (var t in Tiles) {
            t.CurrentUnit.SetClickable(false);
            EvolvingUnitEvent?.Invoke(t.CurrentUnit);
            Units.Add(t.CurrentUnit);
            yield return new WaitForSeconds(0.2f); // Tiny delay between each of them
        }

        // Wait until last one is ready
        while (lastUnit.state.ShaderEvoFade < 1f) yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.1f); // Then wait just a little bit more

        // Spawn evolution
        BoardUnit evolvedUnit = SpawnUnit(firstUnit.evolution);
        evolvedUnit.SetClickable(false);
        yield return new WaitForEndOfFrame();
        Tiles[0].ClearTile();
        Tiles[0].FillTile(evolvedUnit);
        SpawnedEvolvedUnitEvent?.Invoke(evolvedUnit);

        // Wait until it has spawned
        while (evolvedUnit.state.ShaderEvoAlphaFade < 1f) yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.1f); // Then wait just a little bit more

        // Despawn base units once last one is invisible
        while (lastUnit.state.ShaderEvoAlphaFade > 0f) yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.1f); // Then wait just a little bit more ;)

        foreach (var unit in Units) { if (unit != firstUnit) unit.CurrentTile.ClearTile(); BoltNetwork.Destroy(unit.gameObject); }

        // Check if this unit can evolve
        CheckForEvolution(evolvedUnit);

        // Wait for evolved unit to be done spawning
        while (evolvedUnit.state.ShaderEvoFade > 0f) yield return new WaitForEndOfFrame();
        evolvedUnit.SetClickable(true);
    }
    #endregion

    private BoardUnit SpawnUnit(BoardUnit unitPrefab) {
        var unitEntity = BoltNetwork.Instantiate(unitPrefab.gameObject);
        unitEntity.AssignControl(player.Connection);
        BoardUnit unit = unitEntity.GetComponent<BoardUnit>();
        unit.SetClickable(true);
        unit.SetOwner(player);
        FindOrCreateUnitContainer(unit).TryAddUnit(unit);
        return unit;
    }

    public bool CanSpawnUnit(BoardUnit unit) {
        if (FindOrCreateUnitContainer(unit).IsReadyForEvolve) return true;
        for (int i = 0; i < Layout.BenchSizeTiles; i++) if (!Bench[i].IsTileFilled) return true;
        return false;
    }

    #region Local Events
    public Action<BoardUnit> UnitPlacedEvent;
    public Action<BoardUnit> UnitTeleportedEvent;
    public Action<BoardUnit> EvolvingUnitEvent;
    public Action<BoardUnit> SpawnedEvolvedUnitEvent;
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

        CheckForEvolution(unit);
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