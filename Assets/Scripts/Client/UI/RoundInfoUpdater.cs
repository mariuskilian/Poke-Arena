using UnityEngine;
using TMPro;
using Bolt;

public class RoundInfoUpdater : GlobalEventListener {

    //private RoundMan round;


    [SerializeField]
    private TextMeshProUGUI
        timeText = null,
        roundText = null
        ;

    void Start() {
        //round = RoundMan.Instance;
    }

    /*
    void Update() {
        timeText.text = ((int) round.TimeLeftInPhase).ToString();
        roundText.text = "Stage " + round.StageNumber + "-" + round.RoundNumber + ", "
            + round.CurrentRoundType.ToString() + ": " + round.CurrentPhase.ToString();
    }
    */
}
