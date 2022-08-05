using System;
using UnityEngine;
using V_AnimationSystem;
using CodeMonkey.Utils;
using UnityEngine.UI;

/*
 * Character Base Class
 * */
public class Character_Base : MonoBehaviour {

    #region BaseSetup
    protected V_UnitSkeleton unitSkeleton;
    protected V_UnitAnimation unitAnimation;
    protected AnimatedWalker animatedWalker;

    protected Material material;

    protected UnitAnimType attackUnitAnim;
    protected Color materialTintColor;

    protected virtual void Awake() {
        Transform bodyTransform = transform.Find("Body");
        bodyTransform.GetComponent<MeshRenderer>().material = Instantiate(GetMaterial());

        material = GetMaterial();

        unitSkeleton = new V_UnitSkeleton(1f, bodyTransform.TransformPoint, (Mesh mesh) => bodyTransform.GetComponent<MeshFilter>().mesh = mesh);
        unitAnimation = new V_UnitAnimation(unitSkeleton);

        UnitAnimType idleUnitAnim = UnitAnimType.GetUnitAnimType("dSwordTwoHandedBack_Idle");//UnitAnimType.GetUnitAnimType("dBareHands_Idle");
        UnitAnimType walkUnitAnim = UnitAnimType.GetUnitAnimType("dSwordTwoHandedBack_Walk");
        UnitAnimType hitUnitAnim = UnitAnimType.GetUnitAnimType("dBareHands_Hit");
        attackUnitAnim = UnitAnimType.GetUnitAnimType("dBareHands_PunchQuickAttack");

        animatedWalker = new AnimatedWalker(unitAnimation, idleUnitAnim, walkUnitAnim, 1f, 1f);
    }

    protected virtual void Update() {
        /*unitSkeleton.Update(Time.deltaTime);

        if (materialTintColor.a > 0) {
            float tintFadeSpeed = 6f;
            materialTintColor.a -= tintFadeSpeed * Time.deltaTime;
            GetMaterial().SetColor("_Tint", materialTintColor);
        }*/
    }

    public virtual V_UnitAnimation GetUnitAnimation() {
        return unitAnimation;
    }

    public virtual AnimatedWalker GetAnimatedWalker() {
        return animatedWalker;
    }
    #endregion

    public virtual Material GetMaterial() {
        return transform.Find("Body").GetComponent<MeshRenderer>().material;
    }

    public virtual void SetColorTint(Color color) {
        materialTintColor = color;
    }

    public virtual void PlayAnimMove(Vector3 moveDir) {
        animatedWalker.SetMoveVector(moveDir);
    }

    public virtual void PlayAnimIdle() {
        animatedWalker.SetMoveVector(Vector3.zero);
    }

    public virtual void PlayAnimIdle(Vector3 animDir) {
        animatedWalker.PlayIdleAnim(animDir);
    }

    public virtual void PlayAnimSlideRight() {
        unitAnimation.PlayAnimForced(UnitAnim.GetUnitAnim("dBareHands_SlideRight"), 1f, null);
    }

    public virtual void PlayAnimSlideLeft() {
        unitAnimation.PlayAnimForced(UnitAnim.GetUnitAnim("dBareHands_SlideLeft"), 1f, null);
    }

    public virtual void PlayAnimLyingUp() {
        unitAnimation.PlayAnimForced(UnitAnim.GetUnitAnim("LyingUp"), 1f, null);
    }

    public virtual void PlayAnimAttack(Vector3 attackDir, Action onHit, Action onComplete) {
        unitAnimation.PlayAnimForced(attackUnitAnim, attackDir, 1f, (UnitAnim unitAnim) => {
            if (onComplete != null) onComplete();
        }, (string trigger) => {
            if (onHit != null) onHit();
        }, null);
    }

    public virtual void PlayAnimJump(Vector3 moveDir)
    {
        if (moveDir.x >= 0)
        {
            unitAnimation.PlayAnim(UnitAnim.GetUnitAnim("dBareHands_JumpRight"));
        }
        else
        {
            unitAnimation.PlayAnim(UnitAnim.GetUnitAnim("dBareHands_JumpLeft"));
        }
    }

    public virtual void SetAnimsBareHands() {
        animatedWalker.SetAnimations(UnitAnimType.GetUnitAnimType("dBareHands_Idle"), UnitAnimType.GetUnitAnimType("dBareHands_Walk"), 1f, 1f);
        attackUnitAnim = UnitAnimType.GetUnitAnimType("dBareHands_PunchQuickAttack");
    }

    public virtual void SetAnimsSwordTwoHandedBack() {
        animatedWalker.SetAnimations(UnitAnimType.GetUnitAnimType("dSwordTwoHandedBack_Idle"), UnitAnimType.GetUnitAnimType("dSwordTwoHandedBack_Walk"), 1f, 1f);
        attackUnitAnim = UnitAnimType.GetUnitAnimType("dSwordTwoHandedBack_Sword");
    }

    public virtual void SetAnimsSwordShield() {
        animatedWalker.SetAnimations(UnitAnimType.GetUnitAnimType("dSwordShield_Idle"), UnitAnimType.GetUnitAnimType("dSwordShield_Walk"), 1f, 1f);
        attackUnitAnim = UnitAnimType.GetUnitAnimType("dSwordShield_Attack");
    }

    public virtual Vector3 GetHandLPosition() {
        return unitSkeleton.GetBodyPartPosition("HandL");
    }

    public virtual Vector3 GetHandRPosition() {
        return unitSkeleton.GetBodyPartPosition("HandR");
    }
}
