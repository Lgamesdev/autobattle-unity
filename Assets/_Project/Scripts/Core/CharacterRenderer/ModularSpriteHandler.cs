using LGamesDev.Core.Player;
using UnityEngine;

namespace LGamesDev.Core.CharacterRenderer
{
    public class ModularSpriteHandler : MonoBehaviour
    {
        [SerializeField] private Texture2D baseTexture;
        [SerializeField] private Texture2D headTexture;

        [SerializeField] private Texture2D bodyTexture;
        [SerializeField] private Texture2D bodyTextureWhite;
        [SerializeField] private Texture2D bodyTextureMask;

        [SerializeField] private Texture2D hairTexture;
        [SerializeField] private Texture2D beardTexture;

        [SerializeField] private Texture2D baseHeadTexture;
        [SerializeField] private Texture2D baseHeadMaskTexture;
        [SerializeField] private Texture2D handTexture;

        [SerializeField] private Material quadMaterial;
        [SerializeField] private Material guestMaterial;
        private Texture2D[] beards;
        private Texture2D[] body_masks;
        private Texture2D[] bodys;

        private GuestSpritesheetData guestSpritesheetData;

        private Texture2D[] hairs;

        private void Awake()
        {
            hairs = Resources.LoadAll<Texture2D>("Textures/Hair");
            beards = Resources.LoadAll<Texture2D>("Textures/Beard");
            bodys = Resources.LoadAll<Texture2D>("Textures/Body");
            body_masks = Resources.LoadAll<Texture2D>("Textures/Body_Mask");

            guestSpritesheetData = GuestSpritesheetData.Load_Static();

            if (guestSpritesheetData == null) guestSpritesheetData = GuestSpritesheetData.GenerateRandom();

            SetGuestSpritesheetData(guestSpritesheetData);
        }

        public void SaveTexture()
        {
            guestSpritesheetData.Save();
        }

        public void LoadSavedTexture()
        {
            guestSpritesheetData = GuestSpritesheetData.Load_Static();
            SetGuestSpritesheetData(guestSpritesheetData);
        }

        public void SetRandomTexture()
        {
            guestSpritesheetData = GuestSpritesheetData.GenerateRandom();
            SetGuestSpritesheetData(guestSpritesheetData);
        }

        public void NextHair()
        {
            if (hairs.Length > guestSpritesheetData.hairIndex + 1)
                guestSpritesheetData.hairIndex += 1;
            else
                guestSpritesheetData.hairIndex = 0;

            SetGuestSpritesheetData(guestSpritesheetData);
        }

        public void NextBeard()
        {
            if (beards.Length > guestSpritesheetData.beardIndex + 1)
                guestSpritesheetData.beardIndex += 1;
            else
                guestSpritesheetData.beardIndex = 0;

            SetGuestSpritesheetData(guestSpritesheetData);
        }

        public void NextBody()
        {
            if (bodys.Length > guestSpritesheetData.bodyIndex + 1)
                guestSpritesheetData.bodyIndex += 1;
            else
                guestSpritesheetData.bodyIndex = 0;

            SetGuestSpritesheetData(guestSpritesheetData);
        }

        private void SetGuestSpritesheetData(GuestSpritesheetData guestSpritesheetData)
        {
            var texture = GetTexture(guestSpritesheetData);

            guestMaterial.mainTexture = texture;
            quadMaterial.mainTexture = texture;
        }

        public Texture2D GetTexture(GuestSpritesheetData guestSpritesheetData)
        {
            var texture = new Texture2D(512, 512, TextureFormat.RGBA32, true);

            var spritesheetBasePixels =
                Resources.Load<Texture2D>("Textures/Base/GuestSpriteSheet_Base").GetPixels(0, 0, 512, 512);
            texture.SetPixels(0, 0, 512, 512, spritesheetBasePixels);

            var skinColor = guestSpritesheetData.skinColor;

            var handPixels = Resources.Load<Texture2D>("Textures/Base/SpriteSheet_Hand").GetPixels(0, 0, 64, 64);
            TintColorArray(handPixels, skinColor);
            texture.SetPixels(384, 448, 64, 64, handPixels);

            texture.SetPixels(0, 384, 384, 128, GetHeadBaseTexture().GetPixels(0, 0, 384, 128));

            texture.SetPixels(0, 256, 384, 128, GetBodyTexture().GetPixels(0, 0, 384, 128));


            texture.Apply();

            return texture;
        }

        private Texture2D GetHeadBaseTexture()
        {
            var texture = new Texture2D(384, 128, TextureFormat.RGBA32, true);

            var skinColor = guestSpritesheetData.skinColor;

            var baseHeadPixels = Resources.Load<Texture2D>("Textures/Head/head_Skin").GetPixels(0, 0, 256, 128);
            var headSkinMaskPixels = Resources.Load<Texture2D>("Textures/Head/head_SkinMask").GetPixels(0, 0, 256, 128);
            var headsize = 128 * 128;
            for (var i = 0; i < baseHeadPixels.Length / headsize; i++)
            {
                TintColorArraysInsideMask(baseHeadPixels, skinColor, headSkinMaskPixels);

                var hairColor = guestSpritesheetData.hairColor;

                var hasHair = guestSpritesheetData.hairIndex != -1;
                if (hasHair)
                {
                    var hairIndex = guestSpritesheetData.hairIndex;
                    var hairPixels = hairs[hairIndex].GetPixels(0, 0, 256, 128);
                    TintColorArray(hairPixels, hairColor);
                    MergeCollorArrays(baseHeadPixels, hairPixels);
                }

                var hasBeard = guestSpritesheetData.beardIndex != -1;
                if (hasBeard)
                {
                    var beardIndex = guestSpritesheetData.beardIndex;
                    var beardPixels = beards[beardIndex].GetPixels(0, 0, 256, 128);
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

            var bodyIndex = guestSpritesheetData.bodyIndex;
            var bodyPixels = bodys[bodyIndex].GetPixels(0, 0, 256, 128);
            var bodyMaskPixels = body_masks[bodyIndex].GetPixels(0, 0, 256, 128);
            var primaryColor = guestSpritesheetData.bodyPrimaryColor;
            var primaryMaskColor = new Color(0, 1, 0);
            TintColorArraysInsideMask(bodyPixels, primaryColor, bodyMaskPixels, primaryMaskColor);
            var secondaryColor = guestSpritesheetData.bodySecondaryColor;
            var secondaryMaskColor = new Color(0, 0, 1);
            TintColorArraysInsideMask(bodyPixels, secondaryColor, bodyMaskPixels, secondaryMaskColor);
            texture.SetPixels(0, 0, 256, 128, bodyPixels);

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
    }
}