using UnityEngine;

public abstract class UnitAnimation : MonoBehaviour {

    #region Animator Strings
    //PARAMETER NAMES
    protected const string //GESTURES
        TYPE_REACTIVE_GESTURE = "Reactive Gesture",
        TYPE_NON_REACTIVE_GESTURE = "Non-Reactive Gesture",
        //clips without versions
        COME_HERE = "Come Here",
        DISTRACTED = "Distracted",
        LOOK_THERE = "Look There",
        NO_THANKS = "No Thanks",
        NAME = "Name",
        DOZE = "Doze",
        SLEEP = "Sleep",
        //clips with versions
        EXCITED = "Excited",
        SHAKE = "Shake";
    protected const string //HELPERS
        INDEX = " Index"; //postfix for versioned gestures
    protected const string //CARRY
        TRIGGER_PICKED_UP = "Picked Up",
        BOOL_CARRIED = "Carried",
        FLOAT_CARRY_PRE_CLIP_SPEED = "CarryPre Clip Speed Normalizer",
        FLOAT_CARRY_POST_CLIP_SPEED = "CarryPost Clip Speed Normalizer";
    protected const string //STORE
        TRIGGER_DROPPED_IN_STORE = "Dropped in Store"
        ;
    #endregion

    protected Animator anim;

    private void Awake() {
        anim = gameObject.GetComponent<Animator>();
    }

    #region Helpers
    protected bool IsThisUnit(Unit unit) {
        return unit.gameObject == gameObject;
    }
    #endregion
}
