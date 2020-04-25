using Bolt;

public class Unit : EntityBehaviour<IUnitState> {
    public Unit variant;
    public Unit evolution;
    public UnitProperties properties;
}