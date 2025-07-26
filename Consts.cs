using System.Collections.Generic;

namespace SkillCharms {
    public static class Consts {
        public const string cdash = "Crystal Heart";
        public const string claw = "Mantis Claw";
        public const string cyclone = "Cyclone Slash";
        public const string dash = "Mothwing Cloak";
        public const string dashslash = "Dash Slash";
        public const string ddark = "Descending Dark";
        public const string dgate = "Dreamgate";
        public const string dive = "Desolate Dive";
        public const string dnail = "Dream Nail";
        public const string greatslash = "Great Slash";
        public const string ismas = "Isma's Tear";
        public const string lantern = "Lumafly Lantern";
        public const string shadecloak = "Shade Cloak";
        public const string shadesoul = "Shade Soul";
        public const string shriek = "Abyss Shriek";
        public const string vs = "Vengeful Spirit";
        public const string wings = "Monarch Wings";
        public const string wokenail = "Awoken Dream Nail";
        public const string wraiths = "Howling Wraiths";


        public static readonly Dictionary<string, string> getPdVar = new() {
            { dash, nameof(PlayerData.hasDash) },
            { claw, nameof(PlayerData.hasWalljump) },
            { cdash, nameof(PlayerData.hasSuperDash) },
            { wings, nameof(PlayerData.hasDoubleJump) },
            { shadecloak, nameof(PlayerData.hasShadowDash) },
            { ismas, nameof(PlayerData.hasAcidArmour) },
            { dnail, nameof(PlayerData.hasDreamNail) },
            { dgate, nameof(PlayerData.hasDreamGate) },
            { wokenail, nameof(PlayerData.dreamNailUpgraded) },
            { lantern, nameof(PlayerData.hasLantern) },
            { vs, nameof(PlayerData.fireballLevel) },
            { shadesoul, nameof(PlayerData.fireballLevel) },
            { dive, nameof(PlayerData.quakeLevel) },
            { ddark, nameof(PlayerData.quakeLevel) },
            { wraiths, nameof(PlayerData.screamLevel) },
            { shriek, nameof(PlayerData.screamLevel) },
            { cyclone, nameof(PlayerData.hasCyclone) },
            { dashslash, nameof(PlayerData.hasUpwardSlash) },
            { greatslash, nameof(PlayerData.hasDashSlash) }
        };

        public static readonly Dictionary<string, string> getBoolCharm = new() {
            { nameof(PlayerData.hasDash), dash },
            { nameof(PlayerData.hasWalljump), claw },
            { nameof(PlayerData.hasSuperDash), cdash },
            { nameof(PlayerData.hasDoubleJump), wings },
            { nameof(PlayerData.hasShadowDash), shadecloak },
            { nameof(PlayerData.hasAcidArmour), ismas },
            { nameof(PlayerData.hasDreamNail), dnail },
            { nameof(PlayerData.hasDreamGate), dgate },
            { nameof(PlayerData.dreamNailUpgraded), wokenail },
            { nameof(PlayerData.hasLantern), lantern },
            { nameof(PlayerData.hasCyclone), cyclone },
            { nameof(PlayerData.hasUpwardSlash), dashslash },
            { nameof(PlayerData.hasDashSlash), greatslash }
        };

        public static readonly Dictionary<string, Dictionary<int, string>> getIntCharm = new() {
            { nameof(PlayerData.fireballLevel), new() {
                { 1, vs },
                { 2, shadesoul }
            } },
            { nameof(PlayerData.quakeLevel), new() {
                { 1, dive },
                { 2, ddark }
            } },
            { nameof(PlayerData.screamLevel), new() {
                { 1, wraiths },
                { 2, shriek }
            } }
        };
    }
}
