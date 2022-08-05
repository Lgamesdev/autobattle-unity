using UnityEngine;

namespace LGamesDev.Core.CharacterRenderer
{
    public class UnitAnimator
    {
        private int[] trianglesArr;
        private readonly UnitRenderer unitRenderer;
        private Vector2[] uvsArr;

        private Vector3[] verticesArr;

        public UnitAnimator(UnitRenderer unitRenderer)
        {
            this.unitRenderer = unitRenderer;
        }

        public void PlayAnimationIdle()
        {
            unitRenderer.PlayAnim("idle");
        }

        public void PlayWalkAnim()
        {
        }
    }
}