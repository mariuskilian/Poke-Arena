using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class BoardMan : MonoBehaviour{

    #region Singleton
    private static BoardMan _instance;
    public static BoardMan Instance {
        get {
            if (_instance == null) { //should NEVER happen
                GameObject go = new GameObject("Arena");
                go.AddComponent<BoardMan>();
                Debug.LogWarning("Board Manager instance was null");
            } 
            return _instance;
        }
    }
    #endregion

    #region Constants
    public const int BOARD_WIDTH = 10, BOARD_HEIGHT = 10;
    public const int BENCH_SIZE = 10, BENCH_Y = -2;
    public const float TILE_SIZE = 1f, TILE_OFFSET = 0.5f;
    #endregion

    #region Variables
    private Tile hoveredTile = null; // Currently hovered (by mouse) tile. null if nothing
    private Tile selectedTile = null; // Currently selected tile. null if nothing

    public GameMan.LEVEL currentLevel = GameMan.LEVEL.ONE;
    [SerializeField] private GameObject team = null;

    [SerializeField] private float dragUnitZOffset = -0.3f;
    #endregion

    #region Containers
    private Tile[,] Board; //null if invalid Tile
    private Tile[] Bench;
    private Tile reserveTile;

    private Dictionary<string, UnitContainer> unitContainers;
    #endregion

    #region Events
    public Action<Unit> UnitSelectEvent;
    public Action<Unit> UnitDeselectEvent;
    public Action<Unit> UnitTeleportEvent;
    public Action<List<Tile>> EvolutionEvent;
    #endregion

    #region Unity Methods (Awake, Start, Update)
    private void Awake() {
        _instance = this; // Singleton
    }

    private void Start() {
        InitBoardAndBench();
        InitEventSubscribers();
        unitContainers = new Dictionary<string, UnitContainer>();
    }

    private void Update() {
        UpdpateTileHovered();
        CheckForInput();
        if (selectedTile != null) DragSelectedUnit();
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

    //Initializes all event subscribers for this class
    private void InitEventSubscribers() {
        StoreMan store = StoreMan.Instance;
        store.BuyRequestEvent += HandleRequestBuyEvent;
    }
    #endregion

    #region Mouse and Selection Tracking
    //Updates which Tile the mouse is currently hovering
    private void UpdpateTileHovered() {

        hoveredTile = null;

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25f, LayerMask.GetMask("Board", "Bench"))) {
            int x = (int) hit.point.x;
            int y = (int) hit.point.z;
            if (y == -1) {
                if (x >= 0 && x < BENCH_SIZE) hoveredTile = Bench[x];
            } else if (x >= 0 && x < BOARD_WIDTH && y >= 0 && y < BOARD_HEIGHT / 2) {
                if (Board[x, y] != null) hoveredTile = Board[x, y];
            }
        }
    }

    private void SelectUnit() {
        if (!Camera.main) return;

        if (EventSystem.current.IsPointerOverGameObject()) return;

        selectedTile = null;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25f, LayerMask.GetMask("Units"))) {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Units")) {
                selectedTile = hit.transform.parent.GetComponent<Unit>().GetTile();
            }
        } else if (hoveredTile != null) {
            if (hoveredTile.IsTileFilled()) selectedTile = hoveredTile;
        }

        if (selectedTile != null) UnitSelectEvent?.Invoke(selectedTile.GetUnit());
    }

    private void DragSelectedUnit() {
        if (selectedTile == null) return;

        if (!Camera.main) return;

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25f, LayerMask.GetMask("Drag Unit"))) {
            selectedTile.GetUnit().transform.position = hit.point + Vector3.up * dragUnitZOffset;
        }
    }

    private void DeselectUnit() {
        Unit selected_unit = selectedTile.GetUnit();
        Unit hovered_unit = hoveredTile?.GetUnit();
        if (hoveredTile != null && hoveredTile != selectedTile) {
            Tile.SwapTiles(hoveredTile, selectedTile);
        } else {
            selectedTile.ResetTile();
        }
        selectedTile = null;
        UnitDeselectEvent?.Invoke(selected_unit);
        if (hovered_unit != null && hovered_unit != selected_unit) UnitTeleportEvent?.Invoke(hovered_unit);
    }
    #endregion

    #region Unit Container
    //Finds unit container for unit, or creates new one if none exists yet
    private UnitContainer FindOrCreateUnitContainer(Unit unit) {
        if (unitContainers.ContainsKey(unit.baseStats.name)) {
            return unitContainers[unit.baseStats.name];
        }
        GameObject go = new GameObject(unit.baseStats.name);
        go.AddComponent<UnitContainer>();
        go.transform.SetParent(team.transform);
        UnitContainer unitContainer = go.GetComponent<UnitContainer>();
        unitContainers.Add(unit.baseStats.name, unitContainer);
        return unitContainer;
    }
    #endregion

    #region Helper Methods
    private void CheckForInput() {
        if (Input.GetMouseButtonDown(0)) {
            if (selectedTile == null) {
                SelectUnit();
            } else {
                DeselectUnit();
            }
        }
    }

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
        EvolutionEvent?.Invoke(tiles);
        Unit evolvedUnit = GameMan.Instance.InstantiateUnit(tiles[0].GetUnit().evolution);
        foreach (Tile t in tiles) t.ClearTile().gameObject.SetActive(false);
        InitializeUnit(evolvedUnit, tiles[0]);

    }
    #endregion

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
}
