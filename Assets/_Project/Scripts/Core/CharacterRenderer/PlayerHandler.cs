using LGamesDev.Core.Character;
using LGamesDev.Core.Player;
using UnityEngine;

namespace LGamesDev.Core.CharacterRenderer
{
    public class PlayerHandler : Character_Base
    {
        private Texture2D _baseSpritesheetTexture;

        private CharacterEquipmentManager _equipmentManager;
        private Texture2D _chestTexture;
        private bool _hasArmor;

        private bool _hasHelmet;
        private bool _hasSword;
        private Texture2D _helmetTexture;

        private Inventory _inventory;

        private Body _playerBody;
        private Texture2D _swordTexture;

        protected override void Awake()
        {
            /*base.Awake();

        _hasHelmet = false;
        _hasArmor = false;
        _hasSword = false;

        _playerBody = GameManager.Instance.GetPlayerBody();

        _baseSpritesheetTexture = material.mainTexture as Texture2D;

        _characterEquipment = CharacterEquipment.instance;

        _characterEquipment.onEquipmentChanged += UpdateEquipmentTexture;*/
        }

        private void Start()
        {
            /*SetGuestSpriteSheetTexture();

        SetupTexture();*/
        }

        protected override void Update()
        {
            base.Update();

            if (Input.GetKeyUp(KeyCode.U)) PlayAnimIdle(new Vector3(-1, 0));

            if (Input.GetKeyUp(KeyCode.I)) PlayAnimIdle(new Vector3(0, 0));

            if (Input.GetKeyUp(KeyCode.O)) PlayAnimIdle(new Vector3(1, 0));
        }

        private void SetupTexture()
        {
            foreach (CharacterEquipment characterEquipment in _equipmentManager.currentEquipment)
                if (characterEquipment.equipment != null)
                    UpdateEquipmentTexture(characterEquipment.equipment, null);
        }

        private void UpdateTexture()
        {
            var texture = new Texture2D(512, 512, TextureFormat.RGBA32, true);

            var spritesheetBasePixels = _baseSpritesheetTexture.GetPixels(0, 0, 512, 512);
            texture.SetPixels(0, 0, 512, 512, spritesheetBasePixels);

            if (_hasHelmet)
                texture.SetPixels(0, 384, 384, 128, _helmetTexture.GetPixels(0, 0, 384, 128));
            else
                texture.SetPixels(0, 384, 384, 128, GetHeadBaseTexture().GetPixels(0, 0, 384, 128));

            if (_hasArmor)
                texture.SetPixels(0, 256, 384, 128, _chestTexture.GetPixels(0, 0, 384, 128));
            else
                texture.SetPixels(0, 256, 384, 128, GetBodyTexture().GetPixels(0, 0, 384, 128));

            if (_hasSword) texture.SetPixels(0, 128, 128, 128, _swordTexture.GetPixels(0, 0, 128, 128));

            texture.Apply();

            material.mainTexture = texture;
        }

        public void UpdateEquipmentTexture(Equipment newItem, Equipment oldItem)
        {
            if (newItem != null)
                // Equip Item
                switch (newItem.equipmentType)
                {
                    case EquipmentSlot.Weapon:
                        _hasSword = true;
                        _swordTexture = newItem.sprite.texture;
                        break;
                    case EquipmentSlot.Chest:
                        _hasArmor = true;
                        _chestTexture = newItem.sprite.texture;
                        break;
                    case EquipmentSlot.Head:
                        _hasHelmet = true;
                        _helmetTexture = newItem.sprite.texture;
                        break;
                    /*case EquipmentSlot.Shield:
                    break;*/
                }
            else if (oldItem != null)
                // Unequip Item
                switch (oldItem.equipmentType)
                {
                    case EquipmentSlot.Weapon:
                        _hasSword = false;
                        _swordTexture = null;
                        break;
                    case EquipmentSlot.Chest:
                        _hasArmor = false;
                        _chestTexture = null;
                        break;
                    case EquipmentSlot.Head:
                        _hasHelmet = false;
                        _helmetTexture = null;
                        break;
                    /*case EquipmentSlot.Shield:
                    break;*/
                }

            UpdateTexture();
        }

        public void SetGuestSpriteSheetTexture()
        {
            var texture = new Texture2D(512, 512, TextureFormat.RGBA32, true);

            var spritesheetBasePixels =
                Resources.Load<Texture2D>("Textures/Base/GuestSpriteSheet_Base").GetPixels(0, 0, 512, 512);
            texture.SetPixels(0, 0, 512, 512, spritesheetBasePixels);

            ColorUtility.TryParseHtmlString(_playerBody.skinColor, out Color skinColor);
            
            var handPixels = Resources.Load<Texture2D>("Textures/Base/SpriteSheet_Hand").GetPixels(0, 0, 64, 64);
            TintColorArray(handPixels, skinColor);
            texture.SetPixels(384, 448, 64, 64, handPixels);

            texture.SetPixels(0, 384, 384, 128, GetHeadBaseTexture().GetPixels(0, 0, 384, 128));

            texture.SetPixels(0, 256, 384, 128, GetBodyTexture().GetPixels(0, 0, 384, 128));


            texture.Apply();

            material.mainTexture = texture;
        }

        private Texture2D GetHeadBaseTexture()
        {
            var texture = new Texture2D(384, 128, TextureFormat.RGBA32, true);

            ColorUtility.TryParseHtmlString(_playerBody.skinColor, out Color skinColor);

            var baseHeadPixels =
                Resources.Load<Texture2D>("Textures/Head/head_Skin")
                    .GetPixels(0, 0, 256, 128); //baseHeadTexture.GetPixels(0, 0, 256, 128);
            var headSkinMaskPixels =
                Resources.Load<Texture2D>("Textures/Head/head_SkinMask")
                    .GetPixels(0, 0, 256, 128); //baseHeadMaskTexture.GetPixels(0, 0, 256, 128);
            var headsize = 128 * 128;
            for (var i = 0; i < baseHeadPixels.Length / headsize; i++)
            {
                TintColorArraysInsideMask(baseHeadPixels, skinColor, headSkinMaskPixels);

                ColorUtility.TryParseHtmlString(_playerBody.hairColor, out Color hairColor);

                var hasHair = _playerBody.hairIndex != -1;
                if (hasHair)
                {
                    var hairIndex = _playerBody.hairIndex;
                    // TODO: select hairtexture by hair index
                    var hairPixels = Resources.Load<Texture2D>("Textures/Hair/hair_" + hairIndex)
                        .GetPixels(0, 0, 256, 128); //hairTexture.GetPixels(0, 0, 256, 128);
                    TintColorArray(hairPixels, hairColor);
                    MergeCollorArrays(baseHeadPixels, hairPixels);
                }

                var hasBeard = _playerBody.beardIndex != -1;
                if (hasBeard)
                {
                    var beardIndex = _playerBody.beardIndex;
                    var beardPixels = Resources.Load<Texture2D>("Textures/Beard/beard_" + beardIndex)
                        .GetPixels(0, 0, 256, 128); //beardTexture.GetPixels(0, 0, 256, 128);
                    TintColorArray(beardPixels, hairColor);
                    MergeCollorArrays(baseHeadPixels, beardPixels);
                }
            }

            texture.SetPixels(0, 0, 256, 128, baseHeadPixels);

            texture.Apply();

            return texture;
        }

        public Texture2D GetBodyTexture()
        {
            var texture = new Texture2D(384, 128, TextureFormat.RGBA32, true);

            //var bodyIndex = _playerBody.body;
            /*var bodyPixels =
                Resources.Load<Texture2D>("Textures/Body/body_" + bodyIndex)
                    .GetPixels(0, 0, 256, 128); //bodyTextureWhite.GetPixels(128 * bodyIndex, 0, 128, 128);
            var bodyMaskPixels = Resources.Load<Texture2D>("Textures/Body_Mask/body_mask_" + bodyIndex)
                .GetPixels(0, 0, 256, 128); //bodyTextureMask.GetPixels(128 * bodyIndex, 0, 128, 128);
            var primaryColor = _playerBody.bodyPrimaryColor;
            var primaryMaskColor = new Color(0, 1, 0);
            TintColorArraysInsideMask(bodyPixels, primaryColor, bodyMaskPixels, primaryMaskColor);
            var secondaryColor = _playerBody.bodySecondaryColor;
            var secondaryMaskColor = new Color(0, 0, 1);
            TintColorArraysInsideMask(bodyPixels, secondaryColor, bodyMaskPixels, secondaryMaskColor);
            texture.SetPixels(0, 0, 256, 128, bodyPixels);*/

            texture.Apply();

            return texture;
        }

        private void MergeCollorArrays(Color[] baseArray, Color[] overlay)
        {
            for (var i = 0; i < baseArray.Length; i++)
                if (overlay[i].a > 0)
                {
                    //Overlay has color
                    if (overlay[i].a >= 1)
                    {
                        //Fully replace
                        baseArray[i] = overlay[i];
                    }
                    else
                    {
                        //Interpolate colors
                        var alpha = overlay[i].a;
                        baseArray[i].r += (overlay[i].r - baseArray[i].r) * alpha;
                        baseArray[i].g += (overlay[i].g - baseArray[i].g) * alpha;
                        baseArray[i].b += (overlay[i].b - baseArray[i].b) * alpha;
                        baseArray[i].a += overlay[i].a;
                    }
                }
        }

        private void TintColorArray(Color[] baseArray, Color tint)
        {
            for (var i = 0; i < baseArray.Length; i++)
            {
                //Apply tint
                baseArray[i].r = baseArray[i].r * tint.r;
                baseArray[i].g = baseArray[i].g * tint.g;
                baseArray[i].b = baseArray[i].b * tint.b;
            }
        }

        private void TintColorArraysInsideMask(Color[] baseArray, Color tint, Color[] mask)
        {
            for (var i = 0; i < baseArray.Length; i++)
                if (mask[i].a > 0)
                {
                    //Apply tint
                    var baseColor = baseArray[i];
                    var fullyTintedColor = tint * baseColor;
                    var interpolateAmount = mask[i].a;
                    baseArray[i].r += (fullyTintedColor.r - baseColor.r) * interpolateAmount;
                    baseArray[i].g += (fullyTintedColor.g - baseColor.g) * interpolateAmount;
                    baseArray[i].b += (fullyTintedColor.b - baseColor.b) * interpolateAmount;
                }
        }

        private void TintColorArraysInsideMask(Color[] baseArray, Color tint, Color[] mask, Color maskColor)
        {
            for (var i = 0; i < baseArray.Length; i++)
                if (mask[i].a > 0 && mask[i] == maskColor)
                {
                    //Apply tint
                    var baseColor = baseArray[i];
                    var fullyTintedColor = tint * baseColor;
                    var interpolateAmount = mask[i].a;
                    baseArray[i].r += (fullyTintedColor.r - baseColor.r) * interpolateAmount;
                    baseArray[i].g += (fullyTintedColor.g - baseColor.g) * interpolateAmount;
                    baseArray[i].b += (fullyTintedColor.b - baseColor.b) * interpolateAmount;
                }
        }

        public Inventory GetInventory()
        {
            return _inventory;
        }

        public void SetPlayerBody(Body playerBody)
        {
            _playerBody = playerBody;
        }

        public void SetCharacterEquipmentManager(CharacterEquipmentManager equipmentManager)
        {
            _equipmentManager = equipmentManager;
            _equipmentManager.OnEquipmentChanged += UpdateEquipmentTexture;
        }
    }
}