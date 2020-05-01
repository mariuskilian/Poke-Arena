using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugDisplayTimeLoaded : MonoBehaviour
{
    public Text Text;

    void Update()
    {
        if (BoltNetwork.IsServer) {
            if (GameMan.Instance == null) return;
            Text.text = GameMan.Instance.LoadStatus.ToString() + "%";
        }
    }
}
