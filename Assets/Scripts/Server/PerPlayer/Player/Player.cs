using UnityEngine;
using System.Collections.Generic;

public class Player {

    public List<ManagerBehaviour> managerBehaviours;

    public Player() {
        managerBehaviours.Add(new FinanceMan());
        managerBehaviours.Add(new LevelMan());
        managerBehaviours.Add(new UIMan());
        managerBehaviours.Add(new StoreMan());
        managerBehaviours.Add(new StoreButtonMan());
    }

}