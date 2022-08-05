using UnityEngine;

namespace LGamesDev.Core.CharacterRenderer
{
    public class UnitType
    {
        public static UnitType dHeadDown;
        public static UnitType dHeadUp;
        public static UnitType dHeadLeft;
        public static UnitType dHeadRight;

        public static UnitType dBodyDown;
        public static UnitType dBodyUp;
        public static UnitType dBodyLeft;
        public static UnitType dBodyRight;

        public static UnitType dHand;
        public static UnitType dFoot;

        public static UnitType dWeapon;
        public static UnitType dWeapon_InvertH;
        public static UnitType dSecondary;

        public static UnitType dRifle;
        public static UnitType dRifle_InvertH;
        public static UnitType dRifle_InvertV;
        public static UnitType dMuzzleFlash;
        public static UnitType dAxe;
        public static UnitType dAxe_InvertH;
        public static UnitType dSpartan_HeadDown;
        public static UnitType dSpartan_HeadUp;
        public static UnitType dSpartan_HeadLeft;
        public static UnitType dSpartan_HeadRight;

        public static UnitType dSpartan_BodyDown;
        public static UnitType dSpartan_BodyUp;
        public static UnitType dSpartan_BodyLeft;
        public static UnitType dSpartan_BodyRight;

        public static UnitType dSpartan_Hand;
        public static UnitType dSpartan_Foot;
        public static UnitType dSpartan_Spear;

        public static UnitType dSpartan_Shield;

        //public int preset;
        public string customName;
        public int id;
        public int textureHeight;
        public string textureName;
        public int textureWidth;
        public Vector2[] uvs;

        public UnitType(string customName, float x0, float y0, float x1, float y1, string textureName, int textureWidth,
            int textureHeight)
        {
            //preset = Custom;
            uvs = new[]
            {
                new(x0, y1),
                new Vector2(x1, y1),
                new Vector2(x0, y0),
                new Vector2(x1, y0)
            };
            this.customName = customName;
            this.textureName = textureName;
            this.textureWidth = textureWidth;
            this.textureHeight = textureHeight;
            id = Random.Range(10000, 99999);
        }

        public UnitType(Vector2[] uvs, string customName, int id, string textureName, int textureWidth, int textureHeight)
        {
            //this.preset = preset;
            this.uvs = uvs;
            this.customName = customName;
            this.id = id;
            this.textureName = textureName;
            this.textureWidth = textureWidth;
            this.textureHeight = textureHeight;
        }

        public override string ToString()
        {
            return customName;
            //return id + " " + customName;
        }

        public static void Init()
        {
            float defaultTextureWidth = 512;
            float defaultTextureHeight = 512;
            int x0, x1, y0, y1;

            x0 = 0;
            x1 = 128;
            y0 = 384;
            y1 = 512;
            dHeadDown = new UnitType("Default Head Down", x0 / defaultTextureWidth, y0 / defaultTextureHeight,
                x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth,
                (int)defaultTextureHeight) { id = 100 };

            x0 = 256;
            x1 = 384;
            y0 = 384;
            y1 = 512;
            dHeadUp = new UnitType("Default Head Up", x0 / defaultTextureWidth, y0 / defaultTextureHeight,
                x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth,
                (int)defaultTextureHeight) { id = 101 };

            x0 = 256;
            x1 = 128;
            y0 = 384;
            y1 = 512;
            dHeadLeft = new UnitType("Default Head Left", x0 / defaultTextureWidth, y0 / defaultTextureHeight,
                x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth,
                (int)defaultTextureHeight) { id = 102 };

            x0 = 128;
            x1 = 256;
            y0 = 384;
            y1 = 512;
            dHeadRight = new UnitType("Default Head Right", x0 / defaultTextureWidth, y0 / defaultTextureHeight,
                x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth,
                (int)defaultTextureHeight) { id = 103 };

            x0 = 0;
            x1 = 128;
            y0 = 256;
            y1 = 384;
            dBodyDown = new UnitType("Default Body Down", x0 / defaultTextureWidth, y0 / defaultTextureHeight,
                x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth,
                (int)defaultTextureHeight) { id = 104 };

            x0 = 256;
            x1 = 384;
            y0 = 256;
            y1 = 384;
            dBodyUp = new UnitType("Default Body Up", x0 / defaultTextureWidth, y0 / defaultTextureHeight,
                x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth,
                (int)defaultTextureHeight) { id = 105 };

            x0 = 256;
            x1 = 128;
            y0 = 256;
            y1 = 384;
            dBodyLeft = new UnitType("Default Body Left", x0 / defaultTextureWidth, y0 / defaultTextureHeight,
                x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth,
                (int)defaultTextureHeight) { id = 106 };

            x0 = 128;
            x1 = 256;
            y0 = 256;
            y1 = 384;
            dBodyRight = new UnitType("Default Body Right", x0 / defaultTextureWidth, y0 / defaultTextureHeight,
                x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth,
                (int)defaultTextureHeight) { id = 107 };

            x0 = 384;
            x1 = 448;
            y0 = 448;
            y1 = 512;
            dHand = new UnitType("Default Hand", x0 / defaultTextureWidth, y0 / defaultTextureHeight,
                x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth,
                (int)defaultTextureHeight) { id = 108 };

            x0 = 448;
            x1 = 512;
            y0 = 448;
            y1 = 512;
            dFoot = new UnitType("Default Foot", x0 / defaultTextureWidth, y0 / defaultTextureHeight,
                x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth,
                (int)defaultTextureHeight) { id = 109 };

            x0 = 0;
            x1 = 128;
            y0 = 128;
            y1 = 256;
            dWeapon = new UnitType("Default Weapon", x0 / defaultTextureWidth, y0 / defaultTextureHeight,
                x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth,
                (int)defaultTextureHeight) { id = 110 };

            x0 = 128;
            x1 = 256;
            y0 = 128;
            y1 = 256;
            dSecondary = new UnitType("Default Secondary", x0 / defaultTextureWidth, y0 / defaultTextureHeight,
                x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth,
                (int)defaultTextureHeight) { id = 111 };

            x0 = 128;
            x1 = 512;
            y0 = 0;
            y1 = 128;
            dRifle = new UnitType("Default Rifle", x0 / defaultTextureWidth, y0 / defaultTextureHeight,
                x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth,
                (int)defaultTextureHeight) { id = 112 };

            x0 = 512;
            x1 = 128;
            y0 = 0;
            y1 = 128;
            dRifle_InvertH = new UnitType("Default Rifle Invert H", x0 / defaultTextureWidth, y0 / defaultTextureHeight,
                x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth,
                (int)defaultTextureHeight) { id = 113 };

            x0 = 384;
            x1 = 512;
            y0 = 256;
            y1 = 384;
            dMuzzleFlash = new UnitType("Default Muzzle Flash", x0 / defaultTextureWidth, y0 / defaultTextureHeight,
                x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth,
                (int)defaultTextureHeight) { id = 114 };

            x0 = 384;
            x1 = 512;
            y0 = 0;
            y1 = 256;
            dAxe = new UnitType("Default Axe", x0 / defaultTextureWidth, y0 / defaultTextureHeight,
                x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth,
                (int)defaultTextureHeight) { id = 115 };

            x0 = 512;
            x1 = 384;
            y0 = 0;
            y1 = 256;
            dAxe_InvertH = new UnitType("Default Axe Invert H", x0 / defaultTextureWidth, y0 / defaultTextureHeight,
                x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth,
                (int)defaultTextureHeight) { id = 116 };

            x0 = 128;
            x1 = 0;
            y0 = 128;
            y1 = 256;
            dWeapon_InvertH = new UnitType("Default Weapon Invert H", x0 / defaultTextureWidth, y0 / defaultTextureHeight,
                x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth,
                (int)defaultTextureHeight) { id = 117 };

            x0 = 128;
            x1 = 512;
            y0 = 128;
            y1 = 0;
            dRifle_InvertV = new UnitType("Default Rifle Invert V", x0 / defaultTextureWidth, y0 / defaultTextureHeight,
                x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth,
                (int)defaultTextureHeight) { id = 118 };

            Init_Spartan();
            /*Init_Persian();
        Init_Ogre();
        Init_Marine();
        Init_Zombie();
        Init_Chicken();*/
        }


        private static void Init_Spartan()
        {
            float defaultTextureWidth = 1024;
            float defaultTextureHeight = 512;
            int x0, x1, y0, y1;

            x0 = 279;
            x1 = 376;
            y0 = 316;
            y1 = 467;
            dSpartan_HeadDown = new UnitType("Default Spartan Head Down", x0 / defaultTextureWidth,
                y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet",
                (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 200 };

            x0 = 610;
            x1 = 712;
            y0 = 316;
            y1 = 467;
            dSpartan_HeadUp = new UnitType("Default Spartan Head Up", x0 / defaultTextureWidth, y0 / defaultTextureHeight,
                x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth,
                (int)defaultTextureHeight) { id = 201 };

            x0 = 438;
            x1 = 556;
            y0 = 316;
            y1 = 467;
            dSpartan_HeadRight = new UnitType("Default Spartan Head Right", x0 / defaultTextureWidth,
                y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet",
                (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 202 };

            x0 = 556;
            x1 = 438;
            y0 = 316;
            y1 = 467;
            dSpartan_HeadLeft = new UnitType("Default Spartan Head Left", x0 / defaultTextureWidth,
                y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet",
                (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 203 };

            x0 = 247;
            x1 = 407;
            y0 = 54;
            y1 = 299;
            dSpartan_BodyDown = new UnitType("Default Spartan Body Down", x0 / defaultTextureWidth,
                y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet",
                (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 204 };

            x0 = 574;
            x1 = 743;
            y0 = 54;
            y1 = 299;
            dSpartan_BodyUp = new UnitType("Default Spartan Body Up", x0 / defaultTextureWidth, y0 / defaultTextureHeight,
                x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth,
                (int)defaultTextureHeight) { id = 205 };

            x0 = 414;
            x1 = 552;
            y0 = 54;
            y1 = 299;
            dSpartan_BodyRight = new UnitType("Default Spartan Body Right", x0 / defaultTextureWidth,
                y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet",
                (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 206 };

            x0 = 552;
            x1 = 414;
            y0 = 54;
            y1 = 299;
            dSpartan_BodyLeft = new UnitType("Default Spartan Body Left", x0 / defaultTextureWidth,
                y0 / defaultTextureHeight, x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet",
                (int)defaultTextureWidth, (int)defaultTextureHeight) { id = 207 };

            x0 = 886;
            x1 = 949;
            y0 = 389;
            y1 = 461;
            dSpartan_Hand = new UnitType("Default Spartan Hand", x0 / defaultTextureWidth, y0 / defaultTextureHeight,
                x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth,
                (int)defaultTextureHeight) { id = 208 };

            x0 = 886;
            x1 = 959;
            y0 = 258;
            y1 = 338;
            dSpartan_Foot = new UnitType("Default Spartan Foot", x0 / defaultTextureWidth, y0 / defaultTextureHeight,
                x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth,
                (int)defaultTextureHeight) { id = 209 };

            x0 = 775;
            x1 = 836;
            y0 = 44;
            y1 = 485;
            dSpartan_Spear = new UnitType("Default Spartan Spear", x0 / defaultTextureWidth, y0 / defaultTextureHeight,
                x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth,
                (int)defaultTextureHeight) { id = 210 };

            x0 = 31;
            x1 = 238;
            y0 = 168;
            y1 = 379;
            dSpartan_Shield = new UnitType("Default Spartan Shield", x0 / defaultTextureWidth, y0 / defaultTextureHeight,
                x1 / defaultTextureWidth, y1 / defaultTextureHeight, "defaultSpriteSheet", (int)defaultTextureWidth,
                (int)defaultTextureHeight) { id = 211 };
        }
    }
}