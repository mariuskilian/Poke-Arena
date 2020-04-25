using System;
using UnityEngine;
using System.Collections.Generic;

public class BoardMan : PlayerManager {

    #region Constants
    public const int BOARD_WIDTH = 10, BOARD_HEIGHT = 10;
    public const int BENCH_SIZE = 10, BENCH_Y = -2;
    public const float TILE_SIZE = 1f, TILE_OFFSET = 0.5f;
    #endregion

    #region Variables
    private Tile toTile = null; // Currently hovered (by mouse) tile. null if nothing
    private Tile fromTile = null; // Currently selected tile. null if nothing

    [SerializeField] private GameObject team = null;

    [SerializeField] private float dragUnitZOffset = -0.3f;
    #endregion

    #region Containers
    public Tile[,] Board { get; private set; } //null if invalid Tile
    private Tile[] Bench;
    private Tile reserveTile;

    private Dictionary<string, UnitContainer> unitContainers;
    #endregion

    #region Events
    public Action<Unit> UnitDeselectEvent;
    public Action<Unit, Tile> UnitTeleportEvent; //unit, fromTile
    public Action<List<Tile>, Unit> EvolutionEvent;
    #endregion

    #region Unity Methods

    private void Start() {
        InitBoardAndBench();
        unitContainers = new Dictionary<string, UnitContainer>();
    }
    #endregion

    #region Initialisation (Board, Bench, Event Subscribers)
    //Defines the board shape according to created geometry and initiates Board and Bench
    private void InitBoardAndBench() {
        Board = new Tile[BOARD_WIDTH, BOARD_HEIGHT / 2];
        for (int i = 0; i < BOARD_WIDTH; i++) {
            for (int j = 0; j < BOARD_HEIGHT / 2; j++) {
                if ((i == 0 || i == BOARD_WIDTH - 1) && j < 3)      //leftmost and rightmost column trim invalid
                    Board[i, j] = null;
                else if (j == 0 && (i < 3 || i > BOARD_WIDTH - 4))  //bottommost row trim invalid
                    Board[i, j] = null;
                else {                                              //everything else is a valid field
                    Board[i, j] = new Tile(i, j);
                }
            }
        }

        Bench = new Tile[BENCH_SIZE];
        for (int i = 0; i < BENCH_SIZE; i++) {
            Bench[i] = new Tile(i, -1);
        }

        reserveTile = new Tile(1000, -1);
    }
    #endregion

    #region Unit Container
    //Finds unit container for unit, or creates new one if none exists yet
    private UnitContainer FindOrCreateUnitContainer(Unit unit) {
        if (unitContainers.ContainsKey(unit.properties.name)) {
            return unitContainers[unit.properties.name];
        }
        GameObject go = new GameObject(unit.properties.name);
        go.AddComponent<UnitContainer>();
        go.transform.SetParent(team.transform);
        UnitContainer unitContainer = go.GetComponent<UnitContainer>();
        unitContainers.Add(unit.properties.name, unitContainer);
        return unitContainer;
    }
    #endregion

    #region Helper Methods
    private void InitializeUnit(Unit unit, Tile tile) {
        UnitContainer unitContainer = FindOrCreateUnitContainer(unit);
        unit.transform.SetParent(unitContainer.transform);
        tile.FillTile(unit);
        CheckForEvolution(unit);
    }

    private void CheckForEvolution(Unit unit) {
        if (unit.evolution == null) return;
        List<Tile> tiles = FindOrCreateUnitContainer(unit).CheckForEvolution(unit.evl_chain);
        if (tiles != null) {
            Evolve(tiles);
        }
    }

    private void Evolve(List<Tile> tiles) {
        Unit evolvedUnit = PoolMan.Instance.InstantiateUnit(tiles[0].GetUnit().evolution);
        EvolutionEvent?.Invoke(tiles, evolvedUnit);
        foreach (Tile t in tiles) t.ClearTile().gameObject.SetActive(false);
        InitializeUnit(evolvedUnit, tiles[0]);
    }

    public Tile FindTile(Vector3 localPos) {
        int x = (int)(localPos.x / TILE_SIZE);
        int y = (int)(localPos.z / TILE_SIZE);

        if (x < 0 || x > BOARD_WIDTH || (y < 0 && y != BENCH_Y) || y > BOARD_HEIGHT) return null;
        
        if (y == BENCH_Y) return Bench[x];
        return Board[x, y];
    }
    #endregion

    #region Handle incoming Events
    public override void OnEvent(SelectionMoveUnitEvent evnt) {
        // Only the board manager from the player that sent the event should process it
        if (!IsThisPlayer(evnt.RaisedBy)) return;

        // Find tile of From- and To-Position, then swap the tiles
        Tile fromTile = FindTile(evnt.FromPos);
        Tile toTile = FindTile(evnt.ToPos);

        if (!fromTile.isTileFilled) {
            fromTile.ResetTile();
            return;
        }

        Unit fromUnit = fromTile.GetUnit();
        Unit toUnit = toTile?.GetUnit();
        if (toTile != null && toTile != fromTile) {
            Tile.SwapTiles(toTile, fromTile);
        } else {
            fromTile.ResetTile();
        }
        fromTile = null;
        UnitDeselectEvent?.Invoke(fromUnit);
        if (toUnit != null && toUnit != fromUnit)
            UnitTeleportEvent?.Invoke(toUnit, fromUnit.GetTile());
    }
    #endregion

    /*
    #region Handle incoming Events
    private bool HandleRequestBuyEvent(Unit unit) {
        for (int i = 0; i < BENCH_SIZE; i++) {
            if (!Bench[i].IsTileFilled()) {
                InitializeUnit(unit, Bench[i]);
                return true;
            }
        }
        if (FindOrCreateUnitContainer(unit).GetNumBaseUnits() >= 2) {
            InitializeUnit(unit, reserveTile);
            return true;
        }
        return false;
    }
    #endregion
    */
}
