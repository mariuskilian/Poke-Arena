using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public abstract class UnitAnimation : UnitBehaviour {

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
        SHAKE = "Shake"
        ;
    protected const string //CARRY
        PICKED_UP = "Picked Up",
        CARRIED = "Carried",
        CARRY_PRE_CLIP_SPEED = "CarryPre Clip Speed Normalizer",
        CARRY_POST_CLIP_SPEED = "CarryPost Clip Speed Normalizer"
        ;
    //protected const string //BATTLE

    //protected const string //MOVEMENT

    protected const string //STORE
        DROPPED_IN_STORE = "Dropped in Store"
        ;
    protected const string //HELPERS
        INDEX = " Index" //postfix for versioned gestures
        ;
    #endregion

    #region Constants
    private const int
        MAX_NUM_VERSIONS = 5; //maximum number of versions any unit has for a single animation
    private const float
        MIN_SLEEP_LENGTH = 3f,
        MAX_SLEEP_LENGTH = 15f,
        MIN_DOZE_LENGTH = 1f,
        MAX_DOZE_LENGTH = 5f;
    private readonly Dictionary<string, KeyValuePair<float, float>> TimedGestureLengths = new Dictionary<string, KeyValuePair<float, float>> {
        {SLEEP, new KeyValuePair<float, float>(MIN_SLEEP_LENGTH, MAX_SLEEP_LENGTH) },
        {DOZE, new KeyValuePair<float, float>(MIN_DOZE_LENGTH, MAX_DOZE_LENGTH) }
    };
    #endregion

    #region Containers
    private readonly List<string> GestureTriggerOnly = new List<string> {
        COME_HERE, DISTRACTED, LOOK_THERE, NO_THANKS, NAME };
    private readonly List<string> GestureBoolOnly = new List<string> {
        DOZE, SLEEP };
    private readonly List<string> GestureTriggerWithVersions = new List<string> {
        EXCITED, SHAKE };

    protected Dictionary<string, Action<bool>> AvailableAnimations;
    #endregion

    protected Animator anim;

    protected new void Awake() {
        base.Awake();
        anim = gameObject.GetComponent<Animator>();
        Initialization();
    }

    protected virtual void Update() {
    }

    private void Initialization() {
        //Get list of all overridden clips
        AnimatorOverrideController aoc = anim.runtimeAnimatorController as AnimatorOverrideController;
        List<KeyValuePair<AnimationClip, AnimationClip>> overrideClips = new List<KeyValuePair<AnimationClip, AnimationClip>>(aoc.overridesCount);
        aoc.GetOverrides(overrideClips);

        AvailableAnimations = new Dictionary<string, Action<bool>>();

        Dictionary<string, string> overrideClipNames = overrideClips.ToDictionary(
            pair => pair.Key.name, pair => { if (pair.Value != null) return pair.Value.name; else return null; });

        //map all clips with trigger only to their functions
        foreach (string clip in GestureTriggerOnly) {
            if (!overrideClipNames.TryGetValue(clip, out string newClip)) continue;
            var copy = clip;
            if (newClip != null) AvailableAnimations.Add(copy, isReactive => TriggerGesture(copy, isReactive));
        }
        //map all clips with bool only to their functions
        foreach (string clip in GestureBoolOnly) {
            if (!overrideClipNames.TryGetValue(clip, out string newClip)) continue;
            var copy = clip;
            if (newClip != null) AvailableAnimations.Add(copy,
                isReactive => StartCoroutine(TimedGesture(copy, TimedGestureLengths[copy].Key, TimedGestureLengths[copy].Value, isReactive)));
        }
        //map all clips with trigger with versions to their functions
        foreach (string clip in GestureTriggerWithVersions) {
            int numVersions = 0;
            for (int i = 1; i <= MAX_NUM_VERSIONS; i++) {
                if (!overrideClipNames.TryGetValue(clip + " V" + i, out string newClip)) continue;
                if (newClip != null) numVersions++;
            }
            if (numVersions > 0) {
                var copy = clip;
                AvailableAnimations.Add(copy, isReactive => {
                    int version = GameMan.rng.Next(1, numVersions + 1);
                    anim.SetInteger(copy + INDEX, version);
                    TriggerGesture(copy, isReactive);
                });
            }
        }
    }

    private void TriggerGesture(string gestureName, bool isReactive) {
        if (isReactive) {
            ResetAllTriggers();
            anim.SetTrigger(TYPE_REACTIVE_GESTURE);
        } else {
            anim.SetTrigger(TYPE_NON_REACTIVE_GESTURE);
        }
        anim.SetTrigger(gestureName);
    }

    private void ResetAllTriggers() {
        foreach (AnimatorControllerParameter par in anim.parameters) {
            if (par.type == AnimatorControllerParameterType.Trigger)
                anim.ResetTrigger(par.name);
        }
    }

    private IEnumerator TimedGesture(string gestureName, float min_length, float max_length, bool isReactive) {
        string gestureType = (isReactive) ? TYPE_REACTIVE_GESTURE : TYPE_NON_REACTIVE_GESTURE;
        anim.SetTrigger(gestureType);
        anim.SetBool(gestureName, true);
        yield return new WaitForSeconds(((float) GameMan.rng.NextDouble() * (max_length - min_length)) + min_length);
        anim.SetBool(gestureName, false);
    }

    #region Helpers

    protected void TryPerformAnimation(string name, bool isReactive) {
        AvailableAnimations.TryGetValue(name, out Action<bool> func);
        func?.Invoke(isReactive);
    }
    #endregion
}
