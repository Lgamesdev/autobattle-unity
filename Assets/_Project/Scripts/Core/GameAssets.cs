using System.Reflection;
using UnityEngine;
using V_AnimationSystem;

namespace LGamesDev.Core
{
    public class GameAssets : MonoBehaviour
    {
        private static GameAssets _i;

        public Transform pfDamagePopup;
        public Transform levelLoader;

        public static GameAssets i
        {
            get
            {
                if (_i == null) _i = Instantiate(Resources.Load<GameAssets>("GameAssets"));
                return _i;
            }
        }

        public static class UnitAnimTypeEnum
        {
            public static UnitAnimType dSwordTwoHandedBack_Idle;
            public static UnitAnimType dSwordTwoHandedBack_Walk;
            public static UnitAnimType dSwordTwoHandedBack_Sword;
            public static UnitAnimType dSwordTwoHandedBack_Sword2;

            public static UnitAnimType dMinion_Idle;
            public static UnitAnimType dMinion_Walk;
            public static UnitAnimType dMinion_Attack;

            public static UnitAnimType dShielder_Idle;
            public static UnitAnimType dShielder_Walk;

            public static UnitAnimType dSwordShield_Idle;
            public static UnitAnimType dSwordShield_Walk;

            public static UnitAnimType dMarine_Idle;
            public static UnitAnimType dMarine_Walk;
            public static UnitAnimType dMarine_Attack;

            public static UnitAnimType dBareHands_Idle;
            public static UnitAnimType dBareHands_Walk;

            static UnitAnimTypeEnum()
            {
                V_Animation.Init();
                var fieldInfoArr = typeof(UnitAnimTypeEnum).GetFields(BindingFlags.Static | BindingFlags.Public);
                foreach (var fieldInfo in fieldInfoArr)
                    if (fieldInfo != null)
                        fieldInfo.SetValue(null, UnitAnimType.GetUnitAnimType(fieldInfo.Name));
            }
        }
    }
}