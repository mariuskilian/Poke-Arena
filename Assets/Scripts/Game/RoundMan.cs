using UnityEngine;
using System.Collections.Generic;

public class RoundMan : MonoBehaviour {

    #region Singleton
    private static RoundMan _instance;
    public static RoundMan Instance {
        get {
            if (_instance == null) { //should NEVER happen
                GameObject go = new GameObject("Round Manager");
                go.AddComponent<RoundMan>();
                Debug.LogWarning("Round Manager instance was null");
            }
            return _instance;
        }
    }
    #endregion

    #region Round Setup Definition
    public enum RoundType { PLAYER, NPC, TRAINER };

    private List<RoundType> FirstRound = new List<RoundType>() {
        RoundType.NPC, RoundType.NPC, RoundType.NPC, RoundType.TRAINER};

    private List<RoundType> Round = new List<RoundType>() {
        RoundType.NPC, RoundType.PLAYER, RoundType.PLAYER, RoundType.PLAYER,
        RoundType.PLAYER, RoundType.PLAYER, RoundType.TRAINER };
    #endregion

    private void Awake() {
        _instance = this;
    }

    void Update() {
        
    }
}
