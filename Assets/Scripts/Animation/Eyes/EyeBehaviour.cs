using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EyeBehaviour : StateMachineBehaviour
{

    protected struct Expr {
        public Vector2 Expression { get; private set; }
        public float Duration { get; private set; }

        public Expr(Vector2 Expression, float Duration) {
            this.Expression = Expression;
            this.Duration = Duration;
        }
    }

    protected static readonly Vector2
        OPEN = new Vector2(0f, 0f),
        SQUEEZE = new Vector2(0f, 0.25f),
        CLOSED = new Vector2(0f, 0.5f),
        SEMI = new Vector2(0f, 0.75f),
        ANGRY = new Vector2(0.5f, 0f),
        SAD = new Vector2(0.5f, 0.5f),
        HAPPY = new Vector2(0.5f, 0.75f)
        ;

    //Some good values (hopefully)
    protected static Expr EXPR_SEMI_CLOSED = new Expr(SEMI, 0.06f);
    protected static Expr EXPR_SEMI_OPEN = new Expr(SEMI, 0.03f);

    Material eye = null;

    private Coroutine _changeEyesHandle;
    private Coroutine ChangeEyesHandle {
        get { return _changeEyesHandle; }
        set {
            if (_changeEyesHandle != null) {
                GameMan.Instance.StopCoroutine(_changeEyesHandle);
            }
            _changeEyesHandle = value;
        }
    }

    protected virtual void ChangeEyes(Vector2 endExpression, params Expr[] ViaExpressions) {
        ChangeEyes(endExpression, 0f, ViaExpressions);
    }

    protected virtual void ChangeEyes(Vector2 endExpression, float initialDelay, params Expr[] ViaExpressions) {
        ChangeEyesHandle = GameMan.Instance.StartCoroutine(ChangeEyesCoroutine(endExpression, initialDelay, ViaExpressions));
    }

    private IEnumerator ChangeEyesCoroutine(Vector2 endExpression, float initialDelay, Expr[] ViaExpressions) {
        yield return new WaitForSeconds(initialDelay);
        foreach (Expr exp in ViaExpressions) {
            eye.mainTextureOffset = exp.Expression;
            yield return new WaitForSeconds(exp.Duration);
        }
        eye.mainTextureOffset = endExpression;
        ChangeEyesHandle = null;
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (eye == null)
            foreach (Material m in animator.gameObject.GetComponentInChildren<Renderer>().materials) {
                if (m.name.Contains("Eye")) {
                    eye = m;
                    break;
                }
            }
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
