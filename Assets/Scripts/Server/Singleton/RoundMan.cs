using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;
using System;
using static GameInfo;

[BoltGlobalBehaviour(BoltNetworkModes.Server)]
public class RoundMan : GlobalEventListener {

    public static RoundMan Instance { get; private set; }
    private void Awake() { if (Instance == null) Instance = this; }

    public Round Round { get; private set; }
    public int StageNumber {
        get { return Round.state.RoundInfo.Stage; }
        set { Round.state.RoundInfo.Stage = value; }
    }
    public int RoundNumber {
        get { return Round.state.RoundInfo.Round; }
        set { Round.state.RoundInfo.Round = value; }
    }
    public Phase CurrentPhase {
        get { return ArrayOfEnum<Phase>()[Round.state.RoundInfo.PhaseID]; }
        set { Round.state.RoundInfo.PhaseID = (int)value; }
    }
    private float _timeLeft;
    public float TimeLeft {
        get { return _timeLeft; }
        set { Round.state.RoundInfo.Time = Mathf.Clamp((int)value, 0, StartTime); _timeLeft = value; }
    }
    private RoundType _currentRoundType;
    public RoundType CurrentRoundType {
        get { return _currentRoundType; }
        set { Round.state.RoundInfo.RoundType = _currentRoundType.ToString(); _currentRoundType = value; }
    }

    private Queue<RoundType> CurrentStage;
    private int StartTime;
    private bool isInPhase = false;

    private void Start() { CurrentStage = new Queue<RoundType>(FirstStage); SubscribeLocalEventHandlers(); }

    private void Update() { if (Round != null) UpdateTimer(); }

    private void InitFirstRound() {
        CurrentPhase = Phase.Start;
        StageNumber = 1;
        RoundNumber = 1;
        StartPhase();
    }

    private void UpdateTimer() {
        if (isInPhase) {
            TimeLeft = TimeLeft - Time.deltaTime;
            if (TimeLeft <= 0) isInPhase = false;
        } else DetermineNextPhase();
    }

    private void DetermineNextPhase() {
        if (CurrentPhase == Phase.End) {
            CurrentPhase = Phase.Start;
            DetermineNextRoundType();
        } else CurrentPhase = ArrayOfEnum<Phase>()[(int)CurrentPhase + 1];

        StartPhase();
    }

    private void DetermineNextRoundType() {
        if (CurrentStage.Count == 0) {
            StageNumber++;
            RoundNumber = 1;
            CurrentStage = new Queue<RoundType>(Stage);
        } else RoundNumber++;

        CurrentRoundType = CurrentStage.Dequeue();
    }

    private void StartPhase() {
        StartTime = PhaseTimes[(int)CurrentPhase];
        StartOfPhaseEvent?.Invoke(CurrentPhase);
        TimeLeft = StartTime + 1;
        isInPhase = true;
    }

    public Action<Phase> StartOfPhaseEvent;

    private void SubscribeLocalEventHandlers() {
        GameMan game = GameMan.Instance;
        game.GameLoadedEvent += HandleGameLoadedEvent;
        game.StartGameEvent += HandleAllPlayersLoadedEvent;
    }

    private void HandleGameLoadedEvent() { Round = BoltNetwork.Instantiate(BoltPrefabs.Round).GetComponent<Round>(); }

    private void HandleAllPlayersLoadedEvent() { InitFirstRound(); }

}
