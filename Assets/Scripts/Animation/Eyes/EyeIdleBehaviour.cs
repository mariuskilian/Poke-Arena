using System.Collections;
using UnityEngine;

public class EyeIdleBehaviour : EyeBehaviour {

    private static readonly string
        IDLE = "Idle"
        ;

    bool isBlinking = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateEnter(animator, stateInfo, layerIndex);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        if (!isBlinking) GameMan.Instance.StartCoroutine(RandomBlink(animator));
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateExit(animator, stateInfo, layerIndex);
    }

    private IEnumerator RandomBlink(Animator animator) {
        isBlinking = true;
        int timeBetweenBlinks = GameMan.random.Next(3, 12);
        float random = (float) GameMan.random.NextDouble();
        float timeClosed = (random * random) * 0.35f + 0.05f;
        yield return new WaitForSeconds(timeBetweenBlinks);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(IDLE))
            ChangeEyes(OPEN, EXPR_SEMI_CLOSED, new Expr(CLOSED, timeClosed), EXPR_SEMI_OPEN);
        isBlinking = false;
    }
}
