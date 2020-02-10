namespace RPG.Core
{
    public struct Constant
    {
        public const int maxSpellLevel = 9;

        public const float closeRange = 7.62f;
        public const float closeRangeBonus = 1.524f;
        public const float longRange = 121.92f;
        public const float longRangeBonus = 12.192f;
        public const float mediumRange = 30.48f;
        public const float mediumRangeBonus = 3.048f;
        public const float meleeRound = 6f;
        public const float speed = 20f;
        public const float touchRange = 1.25f;

        public const string defaultSaveFile = "Save";
        public const string generatorObjectName = "NumberGenerator";
        public const string temperaryGameObjectName = "Temperary Objects";
        public const string weaponName = "Weapon";
    }

    public struct ResourcePath
    {
        public const string spellProgresssionPath = "Progression Tables/SpellProgression";
        public const string spellResourcePath = "Spells/";
        public const string statProgresssionPath = "Progression Tables/StatProgression";
        internal static string weaponResourcePath = "Weapons/";
        public const string xPRewardsPath = "Progression Tables/XP Rewards";
    }

    public struct TimeDefination
    {
        public const float minute   =       60f;
        public const float hour     =     3600f;
        public const float day      =    86400f;
        public const float week     =   604800f;
        public const float month    =  2629845.8136288f;
        public const float year     = 31558149.7635456f;
    }

    public struct SceneNames
    {
        public const string splash = "00_Splash";
        public const string transitional = "01_Transition Scene";
        public const string camp = "02_Camp";
        public const string main = "05_Main Scene";
    }
}