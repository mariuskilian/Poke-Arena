using UnityEngine;
using System;
using System.Collections.Generic;

public class RoundMan : ManagerBehaviour {

    #region Singleton
    public static RoundMan Instance { get; private set; }
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
    public Action<Phase> StartOfPhaseEvent;
    #endregion

    #region Constants
    [SerializeField]
    private const int //in Seconds
        START_TIME = 1,
        SET_UP_TIME = 30,
        PREPARE_TIME = 5,
        BATTLE_TIME = 30,
        BATTLE_OVERTIME = 10,
        END_TIME = 3
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

    Dictionary<Phase, int> PhaseTimes = new Dictionary<Phase, int> {
        { Phase.START, START_TIME }, { Phase.SETUP, SET_UP_TIME }, { Phase.PREPARE, PREPARE_TIME },
        { Phase.BATTLE, BATTLE_TIME }, {Phase.OVERTIME, BATTLE_OVERTIME }, { Phase.END, END_TIME }
    };
    #endregion

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        InitFirstRound();
    }

    private new void Update() {
        base.Update();
        UpdateTimer();
    }

    private void UpdateTimer() {
        if (isInPhase) {
            TimeLeftInPhase -= Time.deltaTime;
            if (TimeLeftInPhase < 0) TimeLeftInPhase = 0f;
            if (TimeLeftInPhase == 0) isInPhase = false;
        } else {
            if ((int)CurrentPhase++ == Enum.GetNames(typeof(Phase)).Length) {
                CurrentPhase = Phase.START;
                DetermineNextRoundType();
            }
            if (PhaseTimes.ContainsKey(CurrentPhase)) {
                StartTime = PhaseTimes[CurrentPhase];
            } else {
                Debug.Log(CurrentPhase);
            }
            StartOfPhaseEvent?.Invoke(CurrentPhase);
            TimeLeftInPhase = StartTime + 1;
            isInPhase = true;
        }
    }

    private void InitFirstRound() {
        CurrentStage = new Queue<RoundType>(FirstRound);
        CurrentPhase = Phase.START;
        StageNumber = 1;
        RoundNumber = 1;
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
