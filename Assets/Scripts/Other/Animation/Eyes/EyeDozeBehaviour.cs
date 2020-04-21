using UnityEngine;

public class EyeDozeBehaviour : EyeBehaviour
{
    private static readonly string
        DOZE_PRE = "DozePre",
        DOZE_POST = "DozePost"
        ;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        if (stateInfo.IsName(DOZE_PRE))
            ChangeEyes(CLOSED, EXPR_SEMI_CLOSED);
        if (stateInfo.IsName(DOZE_POST))
            ChangeEyes(OPEN, EXPR_SEMI_OPEN, new Expr(OPEN, 0.25f), EXPR_SEMI_CLOSED, new Expr(CLOSED, 0.5f), EXPR_SEMI_OPEN);
    }
}
