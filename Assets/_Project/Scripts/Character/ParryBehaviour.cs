using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace LGamesDev
{
    public class ParryBehaviour : StateMachineBehaviour
    {
        //[SerializeField] private float timeUntilParrying;

        private bool _isParrying;
        //private float _idleTime;
        private int _parryingAnimation;
        
        private static readonly int IsParrying = Animator.StringToHash("isParrying");
        private static readonly int ParryingAnimation = Animator.StringToHash("ParryingAnimation");

        //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            ResetIdle();
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (animator.GetBool(IsParrying)
                && !_isParrying)
            {
                /*_idleTime += Time.deltaTime;

                if (!(_idleTime > timeUntilParrying
                    && stateInfo.normalizedTime % 1 < 0.02f)) return;*/
                
                _isParrying = true;
                _parryingAnimation = 1;
                animator.SetFloat(ParryingAnimation, _parryingAnimation, 0.3f, Time.deltaTime);
            }
            else if (stateInfo.normalizedTime % 1 > 0.98)
            {
                ResetIdle();
                animator.SetFloat(ParryingAnimation, _parryingAnimation, 0.3f, Time.deltaTime);
            }
            
        }

        private void ResetIdle()
        {
            //_idleTime = 0;
            _isParrying = false;
            _parryingAnimation = 0;
        }
    }
}
