using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static GameInfo;

public abstract class UnitAnimation : UnitBehaviour {

    // Type of gesture
    protected const string Reactive = "ReactiveGesture", NonReactive = "NonReactiveGesture";

    // Clip names
    protected const string ComeHere = "ComeHere", Distracted = "Distracted", LookThere = "LookThere", NoThanks = "NoThanks",
        Name = "Name", Doze = "Doze", Sleep = "Sleep", Excited = "Excited", Shake = "Shake";
    
    // Other parameter names
    protected const string PickedUp = "PickedUp", Carried = "Carried", CarryPreClipSpeed = "CarryPreClipSpeedNormalizer",
        CarryPostClipSpeed = "CarryPostClipSpeedNormalizer";

    protected bool TriggerAsReactive { set { if (value) unit.state.ReactiveGesture(); else unit.state.NonReactiveGesture(); } }

    // Dictionaries map clip names to the Actions of accessing and changing the correct parameters
    protected Dictionary<string, Action> TriggerOnlyGestures, TriggerOnlyAnimations;
    protected Dictionary<string, Action<bool>> TimedGestures, BoolOnlyAnimations;
    protected Dictionary<string, Action<int>> TriggerGesturesWithVersions;
    private void InitDictionaries() {
        // Animations that are specific to an action. All units have these animations
        TriggerOnlyAnimations = new Dictionary<string, Action> {
            { PickedUp, () => unit.state.PickedUp() } };
        BoolOnlyAnimations = new Dictionary<string, Action<bool>> {
            { Carried, b => unit.state.Carried = b} };

        // Animations that can be randomly triggered, but not all units have all these animations
        TriggerOnlyGestures = new Dictionary<string, Action> {
            { ComeHere, () => unit.state.ComeHere() },
            { Distracted, () => unit.state.Distracted() },
            { LookThere, () => unit.state.LookThere() },
            { NoThanks, () => unit.state.NoThanks() },
            { Name, () => unit.state.Name() } };
        TimedGestures = new Dictionary<string, Action<bool>> {
            { Doze, b => unit.state.Doze = b},
            { Sleep, b => unit.state.Sleep = b} };
        TriggerGesturesWithVersions = new Dictionary<string, Action<int>> {
            { Excited, v => { unit.state.Excited(); unit.state.ExcitedIndex = v; } },
            { Shake, v => { unit.state.Shake(); unit.state.ShakeIndex = v; } } };
    }

    protected readonly Dictionary<string, float[]> TimedLengths = new Dictionary<string, float[]> {
        {"Doze", new float[]{ 1f, 5f } }, {"Sleep", new float[]{ 3f, 15f } } };

    protected Animator animator;
    protected Dictionary<string, Action<bool>> AvailableGestures;

    private static readonly int MaxNumVersions = 5;

    protected new void Awake() { base.Awake(); Initialization(); }

    private void Initialization() {
        animator = unit.state.Animator;
        InitDictionaries();
        AvailableGestures = new Dictionary<string, Action<bool>>();

        var aoc = animator.runtimeAnimatorController as AnimatorOverrideController;
        var overrideClips = new List<KeyValuePair<AnimationClip, AnimationClip>>(aoc.overridesCount);
        aoc.GetOverrides(overrideClips);

        var overrideClipNames = overrideClips.ToDictionary(
            pair => pair.Key.name,
            pair => { if (pair.Value != null) return pair.Value.name; else return null; }
        );

        foreach (var g in TriggerOnlyGestures) {
            if (!overrideClipNames.TryGetValue(g.Key, out var newClip) || newClip == null) continue;
            AvailableGestures.Add(g.Key, b => { TriggerAsReactive = b; g.Value?.Invoke(); });
        }

        foreach (var g in TimedGestures) {
            if (!overrideClipNames.TryGetValue(g.Key, out var newClip) || newClip == null) continue;
            var l = TimedLengths[g.Key];
            AvailableGestures.Add(g.Key, b => { TriggerAsReactive = b; StartCoroutine(TimedGesture(g.Value, l[0], l[1])); });
        }
        
        foreach (var g in TriggerGesturesWithVersions) {
            int numVersions = 0;
            for (int v = 1; v <= MaxNumVersions; v++)
                if (overrideClipNames.TryGetValue(g.Key + "V" + v, out var newClip) && newClip != null) numVersions++;

            if (numVersions == 0) continue;
            AvailableGestures.Add(g.Key, b => { TriggerAsReactive = b; g.Value?.Invoke(RNG.Next(numVersions) + 1); });
        }
    }

    private IEnumerator TimedGesture(Action<bool> action, float minLength, float maxLength) {
        action?.Invoke(true);
        yield return new WaitForSeconds((float)RNG.NextDouble() * (maxLength - minLength) + minLength);
        action?.Invoke(false);
    }

    protected void TryPerformGesture(string name, bool isReactive) {
        AvailableGestures.TryGetValue(name, out Action<bool> action);
        action?.Invoke(isReactive);
    }
}