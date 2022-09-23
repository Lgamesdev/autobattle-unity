using System;
using LGamesDev.Core.Player;
using UnityEngine;
using V_AnimationSystem;

namespace LGamesDev.Core.CharacterRenderer
{
    public class GuestHandler : MonoBehaviour
    {
        private Texture2D _baseSpritesheetTexture;
        private Texture2D _chestTexture;
        private Texture2D _helmetTexture;

        private UnitAnimType _idleUnitAnimType;

        private Material _material;
        private Texture2D _swordTexture;
        private V_UnitAnimation _unitAnimation;

        private V_UnitSkeleton _unitSkeleton;

        private void Start()
        {
            var bodyTransform = transform.Find("Body");
            _unitSkeleton = new V_UnitSkeleton(1f, bodyTransform.TransformPoint,
                mesh => bodyTransform.GetComponent<MeshFilter>().mesh = mesh);
            _unitAnimation = new V_UnitAnimation(_unitSkeleton);

            _idleUnitAnimType = UnitAnimType.GetUnitAnimType("dBareHands_Idle");

            _unitAnimation.PlayAnim(_idleUnitAnimType, new Vector3(0, -1), 1f, null, null, null);
        }

        private void Update()
        {
            _unitSkeleton.Update(Time.deltaTime);
        }

        public event EventHandler OnEquipChanged;

        private void UpdateTexture()
        {
            var texture = new Texture2D(512, 512, TextureFormat.RGBA32, true);

            var spritesheetBasePixels = _baseSpritesheetTexture.GetPixels(0, 0, 512, 512);
            texture.SetPixels(0, 0, 512, 512, spritesheetBasePixels);

            texture.Apply();

            _material.mainTexture = texture;
        }

        public void SetEquipment(Equipment equipment)
        {
            if (equipment != null)
                // Equip Item
                switch (equipment.equipmentType)
                {
                    case EquipmentType.Weapon:
                        _swordTexture = equipment.icon.texture;
                        break;
                    case EquipmentType.Chest:
                        _chestTexture = equipment.icon.texture;
                        break;
                    case EquipmentType.Helmet:
                        _helmetTexture = equipment.icon.texture;
                        break;
                }

            UpdateTexture();
            OnEquipChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}