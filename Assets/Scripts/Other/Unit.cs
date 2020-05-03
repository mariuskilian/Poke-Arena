using UnityEngine;
using Bolt;
using static GameInfo;

public class Unit : EntityBehaviour<IUnitState> {

    public Tile CurrentTile { get; private set; }

    public Unit evolution;
    public EvlChain evolutionChain;
    public UnitProperties properties;

    public void UpdateTile(Tile tile) { CurrentTile = tile; }

}