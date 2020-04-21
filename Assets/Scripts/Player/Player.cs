public class Player {

    public FinanceMan Finance { get; private set; }
    public LevelMan Level { get; private set; }
    public StoreMan Store { get; private set; }
    public StoreButtonMan StoreButtons { get; private set; }

    public Player() {
        Finance = new FinanceMan();
        Level = new LevelMan();
    }

}