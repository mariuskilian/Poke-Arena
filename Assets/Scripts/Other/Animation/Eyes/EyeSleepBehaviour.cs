using UnityEngine;

public class EyeSleepBehaviour : EyeBehaviour {

    private static readonly string
        SLEEP_PRE = "SleepPre",
        SLEEP_POST = "SleepPost"
        ;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        if (stateInfo.IsName(SLEEP_PRE))
            ChangeEyes(CLOSED, EXPR_SEMI_CLOSED);
        if (stateInfo.IsName(SLEEP_POST))
            ChangeEyes(SEMI, new Expr(SEMI, 1.25f), new Expr(SQUEEZE, 2f));
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateExit(animator, stateInfo, layerIndex);
        if (stateInfo.IsName(SLEEP_POST))
            ChangeEyes(OPEN, EXPR_SEMI_CLOSED, new Expr(CLOSED, 0.1f), EXPR_SEMI_OPEN);
    }
}
