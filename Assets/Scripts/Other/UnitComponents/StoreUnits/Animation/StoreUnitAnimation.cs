using UnityEngine;
using System;
using System.Collections.Generic;
using static GameInfo;

public class StoreUnitAnimation : UnitAnimation {

    private void Start() { animator = GetComponent<Animator>(); SubscribeLocalEventHandlers(); }

    private void SubscribeLocalEventHandlers() {
        if (BoltNetwork.IsClient) {
            var clientStore = ClientStoreMan.Instance;
            clientStore.UnitArrivedInStoreEvent += HandleUnitArrivedInStoreEvent;
        }
    }

    private void HandleUnitArrivedInStoreEvent(StoreUnit storeUnit, int _) {
        if (IsThis<StoreUnit>(storeUnit)) animator.SetTrigger(Dropped);
    }

}