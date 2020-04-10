using UnityEngine;
using System;
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
    public enum Phase { START, SETUP, PREPARE, BATTLE, OVERTIME, END };

    private readonly List<RoundType> FirstRound = new List<RoundType>() {
        RoundType.NPC, RoundType.NPC, RoundType.NPC, RoundType.TRAINER};

    private readonly List<RoundType> Round = new List<RoundType>() {
        RoundType.NPC, RoundType.PLAYER, RoundType.PLAYER, RoundType.PLAYER,
        RoundType.PLAYER, RoundType.PLAYER, RoundType.TRAINER };
    #endregion

    #region Events
    public Action
        StartOfRoundEvent,
        EndOfRoundEvent
        ;
    public Action<Phase> ChangeRoundPhaseEvent;
    #endregion

    #region Constants
    [SerializeField]
    private int //in Seconds
        BEFORE_ROUND_TIME = 1,
        SET_UP_TIME = 30,
        PREPARE_TIME = 5,
        BATTLE_TIME = 30,
        BATTLE_OVERTIME = 10,
        AFTER_ROUND_TIME = 3
        ;
    #endregion

    #region Helper Variables
    public RoundType CurrentRoundType { get; private set; }

    public int StageNumber { get; private set; }
    public int RoundNumber { get; private set; }

    public float TimeLeftInPhase { get; private set; }
    private float StartTime;
    public Phase CurrentPhase { get; private set; }
    private bool isInPhase = false;
    #endregion

    #region Containers
    Queue<RoundType> CurrentStage;
    #endregion

    private void Awake() {
        _instance = this;
    }

    private void Start() {
        InitFirstRound();
    }

    private void Update() {
        UpdateTimer();
    }

    private void UpdateTimer() {
        if (isInPhase) {
            TimeLeftInPhase -= Time.deltaTime;
            if (TimeLeftInPhase < 0) TimeLeftInPhase = 0f;
            if (TimeLeftInPhase == 0) isInPhase = false;
        } else {
            switch (CurrentPhase) { //setup next phase
                case Phase.START:
                    CurrentPhase = Phase.SETUP;
                    StartTime = SET_UP_TIME;
                    break;
                case Phase.SETUP:
                    CurrentPhase = Phase.PREPARE;
                    StartTime = PREPARE_TIME;
                    break;
                case Phase.PREPARE:
                    CurrentPhase = Phase.BATTLE;
                    StartTime = BATTLE_TIME;
                    break;
                case Phase.BATTLE:
                    CurrentPhase = Phase.OVERTIME;
                    StartTime = BATTLE_OVERTIME;
                    break;
                case Phase.OVERTIME:
                    CurrentPhase = Phase.END;
                    StartTime = AFTER_ROUND_TIME;
                    EndOfRoundEvent?.Invoke();
                    break;
                case Phase.END:
                default:
                    CurrentPhase = Phase.START;
                    StartTime = BEFORE_ROUND_TIME;
                    StartOfRoundEvent?.Invoke();
                    DetermineNextRoundType();
                    break;
            }
            TimeLeftInPhase = StartTime + 1;
            isInPhase = true;
        }
    }

    private void InitFirstRound() {
        CurrentStage = new Queue<RoundType>(FirstRound);
        CurrentPhase = Phase.END;
        StageNumber = 1;
        RoundNumber = 0;
    }

    private void DetermineNextRoundType() {
        if (CurrentStage.Count == 0) {
            StageNumber++;
            RoundNumber = 1;
            CurrentStage = new Queue<RoundType>(Round);
        }
        RoundNumber++;
        CurrentRoundType = CurrentStage.Dequeue();
    }
}
