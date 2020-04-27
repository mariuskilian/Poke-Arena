using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;

public class Arena : EntityBehaviour<IArenaState> {

    public GameObject Player1 { get; private set; }
    public GameObject Player2 { get; private set; }

    public bool Shared { get; set; }

    public override void Attached() {
        state.SetTransforms(state.Transform, transform);
    }

}
