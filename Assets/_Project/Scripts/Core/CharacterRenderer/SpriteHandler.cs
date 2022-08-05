using LGamesDev.Core.Player;
using UnityEngine;

namespace LGamesDev.Core.CharacterRenderer
{
    public class SpriteHandler : MonoBehaviour
    {
        public static SpriteHandler instance;

        //[SerializeField] private Texture2D baseTexture;

        //[SerializeField] private Texture2D bodyTextureWhite;
        //[SerializeField] private Texture2D bodyTextureMask;

        //[SerializeField] private Texture2D hairTexture;
        //[SerializeField] private Texture2D beardTexture;

        //[SerializeField] private Texture2D baseHeadTexture;
        //[SerializeField] private Texture2D baseHeadMaskTexture;

        //[SerializeField] private Texture2D handTexture;

        [SerializeField] public Material guestMaterial;

        /*private Color primaryColor;
    private Color secondaryColor;*/

        private void Awake()
        {
            instance = this;

            var guestSpritesheetData = GuestSpritesheetData.Load_Static();
            SetGuestSpritesheetData(guestSpritesheetData);
        }

        private void SetGuestSpritesheetData(GuestSpritesheetData guestSpritesheetData)
        {
            var texture = GetTexture(guestSpritesheetData);

            guestMaterial.mainTexture = texture;
        }

        private Texture2D GetTexture(GuestSpritesheetData guestSpritesheetData)
        {
            var texture = new Texture2D(512, 512, TextureFormat.RGBA32, true);

            var spritesheetBasePixels =
                Resources.Load<Texture2D>("Textures/Base/GuestSpriteSheet_Base").GetPixels(0, 0, 512, 512);
            texture.SetPixels(0, 0, 512, 512, spritesheetBasePixels);

            var skinColor = guestSpritesheetData.skinColor;

            var handPixels = Resources.Load<Texture2D>("Textures/Base/SpriteSheet_Hand").GetPixels(0, 0, 64, 64);
            TintColorArray(handPixels, skinColor);
            texture.SetPixels(384, 448, 64, 64, handPixels);

            //TODO : multiple headsprite for angle
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
                    Debug.Log(hairIndex);
                    // TODO: select hairtexture by hair index
                    var hairPixels = Resources.Load<Texture2D>("Textures/Hair/hair_" + hairIndex)
                        .GetPixels(0, 0, 256, 128); //hairTexture.GetPixels(0, 0, 256, 128);
                    TintColorArray(hairPixels, hairColor);
                    MergeCollorArrays(baseHeadPixels, hairPixels);
                }

                var hasBeard = guestSpritesheetData.beardIndex != -1;
                if (hasBeard)
                {
                    var beardIndex = guestSpritesheetData.beardIndex;
                    var beardPixels = Resources.Load<Texture2D>("Textures/Beard/beard_" + beardIndex)
                        .GetPixels(0, 0, 256, 128); //beardTexture.GetPixels(0, 0, 256, 128);
                    TintColorArray(beardPixels, hairColor);
                    MergeCollorArrays(baseHeadPixels, beardPixels);
                }
            }

            texture.SetPixels(0, 384, 256, 128, baseHeadPixels);

            var bodyIndex = guestSpritesheetData.bodyIndex;
            var bodyPixels =
                Resources.Load<Texture2D>("Textures/Body/body_" + bodyIndex)
                    .GetPixels(0, 0, 256, 128); //bodyTextureWhite.GetPixels(128 * bodyIndex, 0, 128, 128);
            var bodyMaskPixels =
                Resources.Load<Texture2D>("Textures/Body/body_mask_" + bodyIndex)
                    .GetPixels(0, 0, 256, 128); //bodyTextureMask.GetPixels(128 * bodyIndex, 0, 128, 128);
            var primaryColor = guestSpritesheetData.bodyPrimaryColor;
            var primaryMaskColor = new Color(0, 1, 0);
            TintColorArraysInsideMask(bodyPixels, primaryColor, bodyMaskPixels, primaryMaskColor);
            var secondaryColor = guestSpritesheetData.bodySecondaryColor;
            var secondaryMaskColor = new Color(0, 0, 1);
            TintColorArraysInsideMask(bodyPixels, secondaryColor, bodyMaskPixels, secondaryMaskColor);
            texture.SetPixels(0, 256, 256, 128, bodyPixels);

            texture.Apply();

            return texture;
        }

        public Texture2D GetBodyTexture(GuestSpritesheetData guestSpritesheetData)
        {
            var texture = new Texture2D(384, 128, TextureFormat.RGBA32, true);

            var bodyIndex = guestSpritesheetData.bodyIndex;
            var bodyPixels =
                Resources.Load<Texture2D>("Textures/Body/body_" + bodyIndex)
                    .GetPixels(0, 0, 256, 128); //bodyTextureWhite.GetPixels(128 * bodyIndex, 0, 128, 128);
            var bodyMaskPixels =
                Resources.Load<Texture2D>("Textures/Body/body_mask_" + bodyIndex)
                    .GetPixels(0, 0, 256, 128); //bodyTextureMask.GetPixels(128 * bodyIndex, 0, 128, 128);
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

        public Texture2D GetHeadTexture(GuestSpritesheetData guestSpritesheetData)
        {
            var texture = new Texture2D(384, 128, TextureFormat.RGBA32, true);

            var skinColor = guestSpritesheetData.skinColor;

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

                var hairColor = guestSpritesheetData.hairColor;

                var hasHair = guestSpritesheetData.hairIndex != -1;
                if (hasHair)
                {
                    var hairIndex = guestSpritesheetData.hairIndex;
                    // TODO: select hairtexture by hair index
                    var hairPixels = Resources.Load<Texture2D>("Textures/Hair/hair_" + hairIndex)
                        .GetPixels(0, 0, 256, 128); //hairTexture.GetPixels(0, 0, 256, 128);
                    TintColorArray(hairPixels, hairColor);
                    MergeCollorArrays(baseHeadPixels, hairPixels);
                }

                var hasBeard = guestSpritesheetData.beardIndex != -1;
                if (hasBeard)
                {
                    var beardIndex = guestSpritesheetData.beardIndex;
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