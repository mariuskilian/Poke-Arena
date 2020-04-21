using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class UnitCarryAnimation : UnitAnimation {

    #region Constants
    private readonly float pickUpDropClipLength = 0.33f; //in seconds
    #endregion

    #region Containers
    #endregion

    #region Variables
    #endregion

    #region Unity Methods
    private new void Awake() {
        base.Awake();
    }

    private void Start() {
        SetSpeedCarryClips();
    }

    protected override void Update() {
        base.Update();
    }
    #endregion

    #region Initialisation

    //Finds clip speeds of CarryPre and CarryPost in the process
    private void SetSpeedCarryClips() {
        //Get list of all overridden clips
        AnimatorOverrideController aoc = anim.runtimeAnimatorController as AnimatorOverrideController;
        List<KeyValuePair<AnimationClip, AnimationClip>> overrideClips = new List<KeyValuePair<AnimationClip, AnimationClip>>(aoc.overridesCount);
        aoc.GetOverrides(overrideClips);

        Dictionary<string, float> overrideClipNames = overrideClips.ToDictionary(
            pair => pair.Key.name, pair => { if (pair.Value != null) return pair.Value.length; else return -1;});

        float clipLength;
        if (overrideClipNames.TryGetValue(CARRY_PRE_CLIP_SPEED, out clipLength))
            anim.SetFloat(CARRY_PRE_CLIP_SPEED, clipLength / pickUpDropClipLength);
        if (overrideClipNames.TryGetValue(CARRY_POST_CLIP_SPEED, out clipLength))
            anim.SetFloat(CARRY_POST_CLIP_SPEED, clipLength / pickUpDropClipLength);
    }
    #endregion

    /*
    #region Event Handlers
    private void HandleUnitSelectEvent(Unit unit) {
        if (IsThisUnit(unit)) {
            anim.SetTrigger(PICKED_UP);
            anim.SetBool(CARRIED, true);
        }
    }

    private void HandleUnitDeselectEvent(Unit unit) {
        if (IsThisUnit(unit)) anim.SetBool(CARRIED, false);
    }
    #endregion
    */
}
