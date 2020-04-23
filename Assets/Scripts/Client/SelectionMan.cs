using UnityEngine;
using System.Collections.Generic;

public class SelectionMan {

    #region Constants
    private const float DRAG_Z_OFFSET = -0.3f;
    #endregion

    #region Variables
    private Tile hoveredTile = null;
    private Tile selectedTile = null;
    #endregion

    #region Containers
    public Tile[,] Board { get; private set; }
    private Tile[] Bench;
    private Tile reserveTile;

    private Dictionary<string, UnitContainer> unitContainers;
    #endregion

}