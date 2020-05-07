using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static GameInfo;

public class BoardUnitAnimation : UnitAnimation {

    protected bool TriggerAsReactive { set { if (value) This<BoardUnit>().state.ReactiveGesture(); else This<BoardUnit>().state.NonReactiveGesture(); } }

    // Dictionaries map clip names to the Actions of accessing and changing the correct parameters
    protected Dictionary<string, Action> TriggerOnlyGestures, TriggerOnlyAnimations;
    protected Dictionary<string, Action<bool>> TimedGestures, BoolOnlyAnimations;
    protected Dictionary<string, Action<int>> TriggerGesturesWithVersions;
    private void InitDictionaries() {
        // Animations that are specific to an action. All units have these animations
        TriggerOnlyAnimations = new Dictionary<string, Action> {
            { PickedUp, () => This<BoardUnit>().state.PickedUp() } };
        BoolOnlyAnimations = new Dictionary<string, Action<bool>> {
            { Carried, b => This<BoardUnit>().state.Carried = b} };

        // Animations that can be randomly triggered, but not all units have all these animations
        TriggerOnlyGestures = new Dictionary<string, Action> {
            { ComeHere, () => This<BoardUnit>().state.ComeHere() },
            { LookThere, () => This<BoardUnit>().state.LookThere() },
            { NoThanks, () => This<BoardUnit>().state.NoThanks() },
            { Name, () => This<BoardUnit>().state.Name() } };
        TimedGestures = new Dictionary<string, Action<bool>> {
            { Doze, b => This<BoardUnit>().state.Doze = b} };
        TriggerGesturesWithVersions = new Dictionary<string, Action<int>> {
            { Excited, v => { This<BoardUnit>().state.Excited(); This<BoardUnit>().state.ExcitedIndex = v; } },
            { Shake, v => { This<BoardUnit>().state.Shake(); This<BoardUnit>().state.ShakeIndex = v; } } };
    }

    protected readonly Dictionary<string, float[]> TimedLengths = new Dictionary<string, float[]> {
        {Doze, new float[]{ 1f, 5f } } };
    protected Dictionary<string, Action<bool>> Gestures;

    protected new void Awake() { base.Awake(); Initialization(); }

    private void Initialization() {
        animator = This<BoardUnit>().state.Animator;
        InitDictionaries();
        Gestures = new Dictionary<string, Action<bool>>();

        foreach (var g in TriggerOnlyGestures) {
            if (!AvailableGestures.Contains(g.Key)) continue;
            Gestures.Add(g.Key, b => { TriggerAsReactive = b; g.Value?.Invoke(); });
        }

        foreach (var g in TimedGestures) {
            if (!AvailableGestures.Contains(g.Key)) continue;
            var l = TimedLengths[g.Key];
            Gestures.Add(g.Key, b => { TriggerAsReactive = b; StartCoroutine(TimedGesture(g.Value, l[0], l[1])); });
        }
        
        foreach (var g in TriggerGesturesWithVersions) {
            int numVersions = 0;
            for (int v = 1; v <= MaxNumVersions; v++) if (AvailableGestures.Contains(g.Key + "V" + v)) numVersions++;

            if (numVersions == 0) continue;
            Gestures.Add(g.Key, b => { TriggerAsReactive = b; g.Value?.Invoke(RNG.Next(numVersions) + 1); });
        }
    }

    private IEnumerator TimedGesture(Action<bool> action, float minLength, float maxLength) {
        action?.Invoke(true);
        yield return new WaitForSeconds((float)RNG.NextDouble() * (maxLength - minLength) + minLength);
        action?.Invoke(false);
    }

    protected void TryPerformGesture(string name, bool isReactive) {
        Gestures.TryGetValue(name, out var action);
        action?.Invoke(isReactive);
    }
}