using UnityEngine;
using System.Collections.Generic;

public class Player {

    public List<ManagerBehaviour> managerBehaviours;

    public Player() {
        managerBehaviours.Add(new FinanceMan());
        Level = new LevelMan();
        UI = new UIMan();
        Store = new StoreMan();
        StoreButtons = new StoreButtonMan();
    }

}