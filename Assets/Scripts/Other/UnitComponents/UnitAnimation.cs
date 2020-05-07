using UnityEngine;
using System.Collections.Generic; 
using System.Linq;

public abstract class UnitAnimation : UnitComponent {

    // Type of gesture
    protected const string Reactive = "ReactiveGesture", NonReactive = "NonReactiveGesture";

    // Clip names
    protected const string ComeHere = "ComeHere", LookThere = "LookThere", NoThanks = "NoThanks",
        Name = "Name", Doze = "Doze", Excited = "Excited", Shake = "Shake";
    
    // Other parameter names
    protected const string PickedUp = "PickedUp", Carried = "Carried", DroppedInStore = "DroppedInStore",
        CarryPreClipSpeed = "CarryPreClipSpeedNormalizer", CarryPostClipSpeed = "CarryPostClipSpeedNormalizer";

    protected static readonly int MaxNumVersions = 5;

    protected Animator animator;

    private List<string> AllGestures;
    protected List<string> AvailableGestures;

    protected new void Awake() { base.Awake(); animator = GetComponent<Animator>(); FindAvailableGestures(); }

    private void FindAvailableGestures() {
        AllGestures = new List<string> {
            ComeHere, LookThere, NoThanks, Name, Doze };
        for (int v = 0; v < MaxNumVersions; v++) {
            AllGestures.Add(Excited + "V" + v);
            AllGestures.Add(Shake + "V" + v);
        }

        AvailableGestures = new List<string>();

        var aoc = animator.runtimeAnimatorController as AnimatorOverrideController;
        var overrideClips = new List<KeyValuePair<AnimationClip, AnimationClip>>(aoc.overridesCount);
        aoc.GetOverrides(overrideClips);
        var overrideClipNames = overrideClips.ToDictionary(
            pair => pair.Key.name,
            pair => { if (pair.Value != null) return pair.Value.name; else return null; }
        );

        foreach (string clip in AllGestures) if (overrideClipNames.TryGetValue(clip, out var newClip) && newClip != null)
            AvailableGestures.Add(clip);
        
    }
}