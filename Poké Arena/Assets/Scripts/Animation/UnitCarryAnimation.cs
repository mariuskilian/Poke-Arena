using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class UnitCarryAnimation : MonoBehaviour {

    #region Animator Strings
    //PARAMETER NAMES
    private const string //CARRY
        TRIGGER_PICKED_UP = "Picked Up",
        BOOL_CARRIED = "Carried",
        FLOAT_CARRY_PRE_CLIP_SPEED = "CarryPre Clip Speed Normalizer",
        FLOAT_CARRY_POST_CLIP_SPEED = "CarryPost Clip Speed Normalizer";
    #endregion

    #region Constants
    private readonly float pickUpDropClipLength = 0.33f; //in seconds
    #endregion

    #region Containers
    private Animator anim;
    #endregion

    #region Variables
    #endregion

    #region Unity Methods
    private void Start() {
        anim = GetComponent<Animator>();
        InitEventSubscribers();
        SetSpeedCarryClips();
    }

    private void Update() {
    }
    #endregion

    #region Initialisation
    private void InitEventSubscribers() {
        BoardMan board = BoardMan.Instance;
        board.UnitSelectEvent += HandleUnitSelectEvent;
        board.UnitDeselectEvent += HandleUnitDeselectEvent;
    }

    //Finds clip speeds of CarryPre and CarryPost in the process
    private void SetSpeedCarryClips() {
        //Get list of all overridden clips
        AnimatorOverrideController aoc = anim.runtimeAnimatorController as AnimatorOverrideController;
        List<KeyValuePair<AnimationClip, AnimationClip>> overrideClips = new List<KeyValuePair<AnimationClip, AnimationClip>>(aoc.overridesCount);
        aoc.GetOverrides(overrideClips);

        Dictionary<string, float> overrideClipNames = overrideClips.ToDictionary(
            pair => pair.Key.name, pair => { if (pair.Value != null) return pair.Value.length; else return -1;});

        float clipLength;
        if (overrideClipNames.TryGetValue(FLOAT_CARRY_PRE_CLIP_SPEED, out clipLength))
            anim.SetFloat(FLOAT_CARRY_PRE_CLIP_SPEED, clipLength / pickUpDropClipLength);
        if (overrideClipNames.TryGetValue(FLOAT_CARRY_POST_CLIP_SPEED, out clipLength))
            anim.SetFloat(FLOAT_CARRY_POST_CLIP_SPEED, clipLength / pickUpDropClipLength);
    }
    #endregion

    #region Event Handlers
    private void HandleUnitSelectEvent(Unit unit) {
        if (IsThisUnit(unit)) {
            anim.SetTrigger(TRIGGER_PICKED_UP);
            anim.SetBool(BOOL_CARRIED, true);
        }
    }

    private void HandleUnitDeselectEvent(Unit unit) {
        if (IsThisUnit(unit)) anim.SetBool(BOOL_CARRIED, false);
    }
    #endregion

    #region Helpers
    private bool IsThisUnit(Unit unit) {
        return unit.gameObject.GetInstanceID() == gameObject.GetInstanceID();
    }
    #endregion
}
