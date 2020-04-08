using UnityEngine;

public class EyeExcitedBehaviour : EyeBehaviour {

    private bool
        hasChangedToHappy = false,
        hasChangedBackToNormal = false
        ;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        ResetBools();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        if (!hasChangedToHappy && stateInfo.normalizedTime >= 0.1f) {
            hasChangedToHappy = true;
            ChangeEyes(HAPPY, EXPR_SEMI_CLOSED);
        }
        if (!hasChangedBackToNormal && stateInfo.normalizedTime >= 0.9f) {
            hasChangedBackToNormal = true;
            ChangeEyes(OPEN, EXPR_SEMI_OPEN);
        }
    }

    private void ResetBools() {
        hasChangedToHappy = hasChangedBackToNormal = false;
    }
}
