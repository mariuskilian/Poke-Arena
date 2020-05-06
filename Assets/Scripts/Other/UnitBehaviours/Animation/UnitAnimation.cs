using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static GameInfo;

public abstract class UnitAnimation : UnitBehaviour {

    /*protected static readonly Action // Gestures
        ReactiveGesture, NonReactiveGesture;

    protected static readonly Action
        ComeHere, Distracted, LookThere, NoThanks, Name, Doze, Sleep;

    protected static readonly Action
        Excited, Shake;

    private const int
        MaxNumVersions = 5 // Max. number of versions any unit has for a single animation
        ;

    private static readonly float[]
        SleepRange = { 3f, 15f },
        DozeRange = { 1f, 5f }
        ;

    private readonly Dictionary<string, float[]> TimedGestureLengths = new Dictionary<string, float[]> { { Sleep, SleepRange }, { Doze, DozeRange } };

    private readonly List<string>
        TriggerOnlyGestures = new List<Action> { ComeHere, Distracted, LookThere, NoThanks, Name },
        BoolOnlyGestures = new List<Action> { Doze, Sleep },
        TriggerGesturesWithVersions = new List<Action> { Excited, Shake }
        ;

    protected Dictionary<string, Action<bool>> AvailableAnimations;

    protected Animator anim;

    protected new void Awake() {
        base.Awake();
        anim = unit.state.Animator;
        Initialization();
    }

    private void Initialization() {
        // Get list of all overridden clips
        var aoc = anim.runtimeAnimatorController as AnimatorOverrideController;
        var overrideClips = new List<KeyValuePair<AnimationClip, AnimationClip>>(aoc.overridesCount);
        aoc.GetOverrides(overrideClips);

        AvailableAnimations = new Dictionary<string, Action<bool>>();

        Dictionary<string, string> overrideClipNames = overrideClips.ToDictionary(
            pair => pair.Key.name,
            pair => { if (pair.Value != null) return pair.Value.name; else return null; }
        );

        foreach (string clip in TriggerOnlyGestures) {
            if (!overrideClipNames.TryGetValue(clip, out var newClip)) continue;
            var copy = clip;
            if (newClip != null) AvailableAnimations.Add(copy, isReactive => TriggerGesture(copy, isReactive));
        }

        foreach (string clip in BoolOnlyGestures) {
            if (!overrideClipNames.TryGetValue(clip, out var newClip)) continue;
            var copy = clip; var range = TimedGestureLengths[copy];
            if (newClip != null) AvailableAnimations.Add(copy, isReactive => StartCoroutine(TimedGesture(copy, range[0], range[1], isReactive)));
        }

        foreach (string clip in TriggerGesturesWithVersions) {
            int numVersions = 0;
            for (int i = 1; i < MaxNumVersions; i++) {
                if (!overrideClipNames.TryGetValue(clip + "V" + i, out var newClip)) continue;
                if (newClip != null) numVersions++;
            }
            if (numVersions > 0) {
                var copy = clip;
                AvailableAnimations.Add(copy, isReactive => {
                    int version = RNG.Next(numVersions);
                    unit.state.Animator.SetInteger(copy + Index, version + 1);
                    TriggerGesture(copy, isReactive);
                });
            }
        }
    }

    private void TriggerGesture(string gestureName, bool isReactive) {
        if (isReactive) { ResetAllTriggers(); unit.state.ReactiveGesture(); } else unit.state.NonReactiveGesture();

        anim.SetTrigger(gestureName);
    }

    private IEnumerator TimedGesture(string gestureName, float minLength, float maxLength, bool isReactive) {
        string gestureType = (isReactive) ? ReactiveGesture : NonReactiveGesture;
        anim.SetTrigger(gestureName);
        anim.SetBool(gestureName, true);
        yield return new WaitForSeconds(((float)RNG.NextDouble() * (maxLength - minLength)) + minLength);
        anim.SetBool(gestureName, false);
    }

    private void ResetAllTriggers() {
        foreach (var par in unit.state.Animator.parameters)
            if (par.type == AnimatorControllerParameterType.Trigger) anim.ResetTrigger(par.name);
    }

    protected void TryPerformAnimation(string name, bool isReactive) { AvailableAnimations.TryGetValue(name, out var func); func?.Invoke(isReactive); }

}

public class AnimationClass {
    public string Name { get; private set; }
    public Action*/
}