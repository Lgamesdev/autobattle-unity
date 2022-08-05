using UnityEngine;

namespace LGamesDev.Core.CharacterRenderer
{
    public class BaseCharacter : MonoBehaviour
    {
        private AnimatedCharacter animatedCharacter;
        private Color materialTintColor;
        private UnitAnimator unitAnimator;
        private UnitRenderer unitRenderer;

        private void Awake()
        {
            var bodyTransform = transform.Find("Body");
            transform.Find("Body").GetComponent<MeshRenderer>().material = new Material(GetMaterial());

            unitRenderer = new UnitRenderer( /*1f, bodyTransform.TransformPoint, */
                mesh => bodyTransform.GetComponent<MeshFilter>().mesh = mesh);
            unitAnimator = new UnitAnimator(unitRenderer);
            /*
        UnitAnimType idleUnitAnim = UnitAnimType.GetUnitAnimType("dSwordTwoHandedBack_Idle");
        UnitAnimType walkUnitAnim = UnitAnimType.GetUnitAnimType("dSwordTwoHandedBack_Walk");
        UnitAnimType hitUnitAnim = UnitAnimType.GetUnitAnimType("dBareHands_Hit");
        attackUnitAnim = UnitAnimType.GetUnitAnimType("dBareHands_PunchQuickAttack");
        */

            animatedCharacter = new AnimatedCharacter(unitAnimator /*, idleUnitAnim, walkUnitAnim, 1f, 1f*/);
        }

        private void Update()
        {
            unitRenderer.Update(Time.deltaTime);

            /*if (materialTintColor.a > 0)
        {
            float tintFadeSpeed = 6f;
            materialTintColor.a -= tintFadeSpeed * Time.deltaTime;
            GetMaterial().SetColor("_Tint", materialTintColor);
        }*/
        }

        public AnimatedCharacter GetAnimatedCharacter()
        {
            return animatedCharacter;
        }

        public Material GetMaterial()
        {
            return transform.Find("Body").GetComponent<MeshRenderer>().material;
        }

        public void SetColorTint(Color color)
        {
            materialTintColor = color;
        }

        public void PlayAnimMove(Vector3 moveDir)
        {
            animatedCharacter.SetMoveVector(moveDir);
        }

        public void PlayAnimIdle()
        {
            animatedCharacter.SetMoveVector(Vector3.zero);
        }

        public void PlayAnimIdle(Vector3 animDir)
        {
            animatedCharacter.PlayIdleAnim(animDir);
        }

        /*public void PlayAnimSlideRight()
    {
        unitAnimation.PlayAnimForced(UnitAnim.GetUnitAnim("dBareHands_SlideRight"), 1f, null);
    }

    public void PlayAnimSlideLeft()
    {
        unitAnimation.PlayAnimForced(UnitAnim.GetUnitAnim("dBareHands_SlideLeft"), 1f, null);
    }

    public void PlayAnimLyingUp()
    {
        unitAnimation.PlayAnimForced(UnitAnim.GetUnitAnim("LyingUp"), 1f, null);
    }

    public void PlayAnimAttack(Vector3 attackDir, Action onHit, Action onComplete)
    {
        unitAnimation.PlayAnimForced(attackUnitAnim, attackDir, 1f, (UnitAnim unitAnim) => {
            if (onComplete != null) onComplete();
        }, (string trigger) => {
            if (onHit != null) onHit();
        }, null);
    }

    public void SetAnimsBareHands()
    {
        animatedWalker.SetAnimations(UnitAnimType.GetUnitAnimType("dBareHands_Idle"), UnitAnimType.GetUnitAnimType("dBareHands_Walk"), 1f, 1f);
        attackUnitAnim = UnitAnimType.GetUnitAnimType("dBareHands_PunchQuickAttack");
    }

    public void SetAnimsSwordTwoHandedBack()
    {
        animatedWalker.SetAnimations(UnitAnimType.GetUnitAnimType("dSwordTwoHandedBack_Idle"), UnitAnimType.GetUnitAnimType("dSwordTwoHandedBack_Walk"), 1f, 1f);
        attackUnitAnim = UnitAnimType.GetUnitAnimType("dSwordTwoHandedBack_Sword");
    }

    public void SetAnimsSwordShield()
    {
        animatedWalker.SetAnimations(UnitAnimType.GetUnitAnimType("dSwordShield_Idle"), UnitAnimType.GetUnitAnimType("dSwordShield_Walk"), 1f, 1f);
        attackUnitAnim = UnitAnimType.GetUnitAnimType("dSwordShield_Attack");
    }

    public Vector3 GetHandLPosition()
    {
        return unitSkeleton.GetBodyPartPosition("HandL");
    }

    public Vector3 GetHandRPosition()
    {
        return unitSkeleton.GetBodyPartPosition("HandR");
    }*/
    }
}