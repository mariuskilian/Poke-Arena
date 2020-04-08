using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UnitGestureAnimation : MonoBehaviour {

    #region Animator Strings
    //PARAMETER NAMES
    private const string //GESTURES
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
    private const string //HELPERS
        INDEX = " Index"; //postfix for versioned gestures

    private readonly List<string> ClipsTriggerOnly = new List<string> {
            COME_HERE, DISTRACTED, LOOK_THERE, NO_THANKS, NAME };
    private readonly List<string> ClipsBoolOnly = new List<string> {
            DOZE, SLEEP };
    private readonly List<string> ClipsTriggerWithVersions = new List<string> {
            EXCITED, SHAKE };
    #endregion

    #region Constants
    private const int
        MAX_NUM_VERSIONS = 5; //maximum number of versions any unit has for a single animation
    private const float
        MIN_SLEEP_LENGTH = 2f,
        MAX_SLEEP_LENGTH = 5f,
        MIN_DOZE_LENGTH = 0.5f,
        MAX_DOZE_LENGTH = 2f;
    private readonly Dictionary<string, KeyValuePair<float, float>> TimedGestureLengths = new Dictionary<string, KeyValuePair<float, float>> {
        {SLEEP, new KeyValuePair<float, float>(MIN_SLEEP_LENGTH, MAX_SLEEP_LENGTH) },
        {DOZE, new KeyValuePair<float, float>(MIN_DOZE_LENGTH, MAX_DOZE_LENGTH) }
    };
    #endregion

    #region Containers
    private Animator anim;
    private Dictionary<string, Action<bool>> AvailableGestures;
    #endregion

    #region Variables
    #endregion

    #region Unity Methods
    private void Start() {
        anim = GetComponent<Animator>();
        InitEventSubscribers();
        InitAvailableGestures();
        InitIdleAnimation();
    }
    #endregion

    #region Initialisation
    private void InitIdleAnimation() {
        StartCoroutine(RandomGesture());
    }

    private void InitEventSubscribers() {
        BoardMan board = BoardMan.Instance;
        board.UnitTeleportEvent += HandleUnitTeleportEvent;
    }

    //Inits available gestures by checking which gestures have been overridden by Animation Override Controller
    //Also finds clip speeds of CarryPre and CarryPost in the process
    private void InitAvailableGestures() {
        AvailableGestures = new Dictionary<string, Action<bool>>();

        //Get list of all overridden clips
        AnimatorOverrideController aoc = anim.runtimeAnimatorController as AnimatorOverrideController;
        List<KeyValuePair<AnimationClip, AnimationClip>> overrideClips = new List<KeyValuePair<AnimationClip, AnimationClip>>(aoc.overridesCount);
        aoc.GetOverrides(overrideClips);

        Dictionary<string, string> overrideClipNames = overrideClips.ToDictionary(
            pair => pair.Key.name, pair => { if (pair.Value != null) return pair.Value.name; else return null; });

        //map all clips with trigger only to their functions
        foreach (string clip in ClipsTriggerOnly) {
            if (!overrideClipNames.TryGetValue(clip, out string newClip)) continue;
            var copy = clip;
            if (newClip != null) AvailableGestures.Add(copy, isReactive => TriggerGesture(copy, isReactive));
        }
        //map all clips with bool only to their functions
        foreach (string clip in ClipsBoolOnly) {
            if (!overrideClipNames.TryGetValue(clip, out string newClip)) continue;
            var copy = clip;
            if (newClip != null) AvailableGestures.Add(copy, isReactive => StartCoroutine(TimedGesture(copy, TimedGestureLengths[copy].Key, TimedGestureLengths[copy].Value, isReactive)));
        }
        //map all clips with trigger with versions to their functions
        foreach (string clip in ClipsTriggerWithVersions) {
            int numVersions = 0;
            for (int i = 1; i <= MAX_NUM_VERSIONS; i++) {
                if (!overrideClipNames.TryGetValue(clip + " V" + i, out string newClip)) continue;
                if (newClip != null) numVersions++;
            }
            if (numVersions > 0) {
                var copy = clip;
                AvailableGestures.Add(copy, isReactive => {
                    int version = GameMan.random.Next(1, numVersions + 1);
                    anim.SetInteger(copy + INDEX, version);
                    TriggerGesture(copy, isReactive);
                });
            }
        }
    }
    #endregion
    private void TriggerGesture(string gestureName, bool isReactive) {
        string gestureType = (isReactive) ? TYPE_REACTIVE_GESTURE : TYPE_NON_REACTIVE_GESTURE;
        anim.SetTrigger(gestureType);
        anim.SetTrigger(gestureName);
    }

    private IEnumerator TimedGesture(string gestureName, float min_length, float max_length, bool isReactive) {
        string gestureType = (isReactive) ? TYPE_REACTIVE_GESTURE : TYPE_NON_REACTIVE_GESTURE;
        anim.SetTrigger(gestureType);
        anim.SetBool(gestureName, true);
        yield return new WaitForSeconds(((float) GameMan.random.NextDouble() * (max_length-min_length)) + min_length);
        anim.SetBool(gestureName, false);
    }

    private IEnumerator RandomGesture() {
        while (true) {
            yield return new WaitForSeconds(((float) GameMan.random.NextDouble() * 8f) + 2f);
            if (GameMan.random.Next(0, 10) < 5) {
                int index = GameMan.random.Next(0, AvailableGestures.Count);
                List<string> Keys = new List<string>(AvailableGestures.Keys);
                string key = Keys[index];
                AvailableGestures[key](false);
            }
        }
    }

    public void EyeGesture(Vector2 expression) {

    }

    #region Event Handlers
    private void HandleUnitTeleportEvent(Unit unit) {
        if (IsThisUnit(unit))
            if (AvailableGestures.TryGetValue(SHAKE, out Action<bool> shake))
                shake(true);
    }
    #endregion

    #region Helper Methods
    private bool IsThisUnit(Unit unit) {
        return unit.gameObject.GetInstanceID() == gameObject.GetInstanceID();
    }
    #endregion
}
