using System;
using UnityEngine;
using System.Collections.Generic;
using Bolt;

public class NewBoardMan : Manager {

    #region Constants
    public const int BOARD_WIDTH = 10, BOARD_HEIGHT = 10;
    public const int BENCH_SIZE = 10, BENCH_Y = -2;
    public const float TILE_SIZE = 1f, TILE_OFFSET = 0.5f;
    #endregion

    #region Variables
    private Tile hoveredTile = null;
    private Tile selectedTile = null;
    #endregion

}