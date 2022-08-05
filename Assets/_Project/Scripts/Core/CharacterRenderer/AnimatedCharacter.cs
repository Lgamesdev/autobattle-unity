using UnityEngine;

namespace LGamesDev.Core.CharacterRenderer
{
    public class AnimatedCharacter
    {
        private Vector3 lastMoveVector;

        private readonly UnitAnimator unitAnimator;
        /*private UnitAnimType idleAnimType;
    private UnitAnimType walkAnimType;
    private float idleFrameRate;
    private float walkFrameRate;*/

        public AnimatedCharacter(UnitAnimator unitAnimator)
        {
            this.unitAnimator = unitAnimator;
            lastMoveVector = new Vector3(0, -1);

            PlayIdleAnim(lastMoveVector);
        }

        public void SetMoveVector(Vector3 moveVector)
        {
            if (moveVector == Vector3.zero)
            {
                //Idle 
                //TODO : faire la direction de l'anim avec lastMoveVector
                unitAnimator.PlayAnimationIdle();
            }
            else
            {
                lastMoveVector = moveVector;
                unitAnimator.PlayWalkAnim();
            }
        }

        public void PlayIdleAnim(Vector3 dir)
        {
            unitAnimator.PlayAnimationIdle( /*idleAnimType, dir, idleFrameRate, null, null, null*/);
        }

        public Vector3 GetLastMoveVector()
        {
            return lastMoveVector;
        }

        /*public void SetAnimations(UnitAnimType idleAnimType, UnitAnimType walkAnimType, float idleFrameRate, float walkFrameRate)
    {
        this.idleAnimType = idleAnimType;
        this.walkAnimType = walkAnimType;
        this.idleFrameRate = idleFrameRate;
        this.walkFrameRate = walkFrameRate;
        unitAnimation.PlayAnim(idleAnimType, lastMoveVector, idleFrameRate, null, null, null);
    }*/
    }
}