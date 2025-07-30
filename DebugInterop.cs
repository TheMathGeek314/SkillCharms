using System;
using System.Reflection;
using MonoMod.RuntimeDetour;

namespace SkillCharms {
    public class DebugInterop {
        private static Action orig_ToggleMothwingCloak, orig_ToggleMantisClaw, orig_ToggleCrystalHeart, orig_ToggleMonarchWings,
            orig_ToggleIsmasTear, orig_ToggleDreamNail, orig_ToggleDreamGate, orig_ToggleLantern, orig_IncreaseFireballLevel, orig_IncreaseQuakeLevel,
            orig_IncreaseScreamLevel, orig_ToggleCycloneSlash, orig_ToggleDashSlash, orig_ToggleGreatSlash, orig_GiveAllSkills, orig_GiveAllCharms;
        private static Hook dashHook, clawHook, cdashHook, wingsHook, ismasHook, dnailHook, dgateHook, lanternHook, vsHook, diveHook, screamHook, cycloneHook, dashslashHook, greatslashHook, allskillsHook, allcharmsHook;

        public static void HookDebug() {
            foreach((string debugMethod, string myMethod, Action<Hook> assignHook) in new (string, string, Action<Hook>)[] {
                ("ToggleMothwingCloak", nameof(doDash), h => dashHook = h),
                ("ToggleMantisClaw", nameof(doClaw), h => clawHook = h),
                ("ToggleCrystalHeart", nameof(doCdash), h => cdashHook = h),
                ("ToggleMonarchWings", nameof(doWings), h => wingsHook = h),
                ("ToggleIsmasTear", nameof(doIsmas), h => ismasHook = h),
                ("ToggleDreamNail", nameof(doDnail), h => dnailHook = h),
                ("ToggleDreamGate", nameof(doDgate), h => dgateHook = h),
                ("ToggleLantern", nameof(doLantern), h => lanternHook = h),
                ("IncreaseFireballLevel", nameof(doVs), h => vsHook = h),
                ("IncreaseQuakeLevel", nameof(doDive), h => diveHook = h),
                ("IncreaseScreamLevel", nameof(doScream), h => screamHook = h),
                ("ToggleCycloneSlash", nameof(doCyclone), h => cycloneHook = h),
                ("ToggleDashSlash", nameof(doDashslash), h => dashslashHook = h),
                ("ToggleGreatSlash", nameof(doGreatslash), h => greatslashHook = h),
                ("GiveAllSkills", nameof(doAllSkills), h => allskillsHook = h),
                ("GiveAllCharms", nameof(doAllCharms), h => allcharmsHook = h)
            }) {
                MethodInfo debugInfo = typeof(DebugMod.BindableFunctions).GetMethod(debugMethod, BindingFlags.Public | BindingFlags.Static);
                MethodInfo myInfo = typeof(DebugInterop).GetMethod(myMethod, BindingFlags.Static | BindingFlags.Public);
                Delegate hookDelegate = Delegate.CreateDelegate(typeof(Action), myInfo);
                Hook hook = new(debugInfo, hookDelegate);
                assignHook(hook);

                FieldInfo origField = typeof(DebugInterop).GetField($"orig_{debugMethod}", BindingFlags.NonPublic | BindingFlags.Static);
                if(origField != null) {
                    Delegate origDelegate = hook.GenerateTrampoline<Action>();
                    origField.SetValue(null, origDelegate);
                }
            }
        }

        public static void doDash() {
            orig_ToggleMothwingCloak();
            if(PlayerData.instance.hasShadowDash)
                SkillCharms.Charms[Consts.shadecloak].TryGrantCharm(false);
            else if(PlayerData.instance.hasDash)
                SkillCharms.Charms[Consts.dash].TryGrantCharm(false);
            else {
                SkillCharms.Charms[Consts.dash].TryRevokeCharm();
                SkillCharms.Charms[Consts.shadecloak].TryRevokeCharm();
            }
        }

        public static void doClaw() {
            orig_ToggleMantisClaw();
            if(PlayerData.instance.hasWalljump)
                SkillCharms.Charms[Consts.claw].TryGrantCharm(false);
            else
                SkillCharms.Charms[Consts.claw].TryRevokeCharm();
        }

        public static void doCdash() {
            orig_ToggleCrystalHeart();
            if(PlayerData.instance.hasSuperDash)
                SkillCharms.Charms[Consts.cdash].TryGrantCharm(false);
            else
                SkillCharms.Charms[Consts.cdash].TryRevokeCharm();
        }

        public static void doWings() {
            orig_ToggleMonarchWings();
            if(PlayerData.instance.hasDoubleJump)
                SkillCharms.Charms[Consts.wings].TryGrantCharm(false);
            else
                SkillCharms.Charms[Consts.wings].TryRevokeCharm();
        }

        public static void doIsmas() {
            orig_ToggleIsmasTear();
            if(PlayerData.instance.hasAcidArmour)
                SkillCharms.Charms[Consts.ismas].TryGrantCharm(false);
            else
                SkillCharms.Charms[Consts.ismas].TryRevokeCharm();
        }

        public static void doDnail() {
            orig_ToggleDreamNail();
            if(PlayerData.instance.dreamNailUpgraded)
                SkillCharms.Charms[Consts.wokenail].TryGrantCharm(false);
            else if(PlayerData.instance.hasDreamNail)
                SkillCharms.Charms[Consts.dnail].TryGrantCharm(false);
            else {
                SkillCharms.Charms[Consts.dnail].TryRevokeCharm();
                SkillCharms.Charms[Consts.wokenail].TryRevokeCharm();
            }
        }

        public static void doDgate() {
            orig_ToggleDreamGate();
            if(PlayerData.instance.hasDreamGate)
                SkillCharms.Charms[Consts.dgate].TryGrantCharm(false);
            else
                SkillCharms.Charms[Consts.dgate].TryRevokeCharm();
        }

        public static void doLantern() {
            orig_ToggleLantern();
            if(PlayerData.instance.hasLantern)
                SkillCharms.Charms[Consts.lantern].TryGrantCharm(false);
            else
                SkillCharms.Charms[Consts.lantern].TryRevokeCharm();
        }

        public static void doVs() {
            orig_IncreaseFireballLevel();
            if(PlayerData.instance.fireballLevel == 2)
                SkillCharms.Charms[Consts.shadesoul].TryGrantCharm(false);
            else if(PlayerData.instance.fireballLevel == 1)
                SkillCharms.Charms[Consts.vs].TryGrantCharm(false);
            else {
                SkillCharms.Charms[Consts.vs].TryRevokeCharm();
                SkillCharms.Charms[Consts.shadesoul].TryRevokeCharm();
            }
        }

        public static void doDive() {
            orig_IncreaseQuakeLevel();
            if(PlayerData.instance.quakeLevel == 2)
                SkillCharms.Charms[Consts.ddark].TryGrantCharm(false);
            else if(PlayerData.instance.quakeLevel == 1)
                SkillCharms.Charms[Consts.dive].TryGrantCharm(false);
            else {
                SkillCharms.Charms[Consts.dive].TryRevokeCharm();
                SkillCharms.Charms[Consts.ddark].TryRevokeCharm();
            }
        }

        public static void doScream() {
            orig_IncreaseScreamLevel();
            if(PlayerData.instance.screamLevel == 2)
                SkillCharms.Charms[Consts.shriek].TryGrantCharm(false);
            else if(PlayerData.instance.screamLevel == 1)
                SkillCharms.Charms[Consts.wraiths].TryGrantCharm(false);
            else {
                SkillCharms.Charms[Consts.wraiths].TryRevokeCharm();
                SkillCharms.Charms[Consts.shriek].TryRevokeCharm();
            }
        }

        public static void doCyclone() {
            orig_ToggleCycloneSlash();
            if(PlayerData.instance.hasCyclone)
                SkillCharms.Charms[Consts.cyclone].TryGrantCharm(false);
            else
                SkillCharms.Charms[Consts.cyclone].TryRevokeCharm();
        }

        public static void doDashslash() {
            orig_ToggleDashSlash();
            if(PlayerData.instance.hasUpwardSlash)
                SkillCharms.Charms[Consts.dashslash].TryGrantCharm(false);
            else
                SkillCharms.Charms[Consts.dashslash].TryRevokeCharm();
        }

        public static void doGreatslash() {
            orig_ToggleGreatSlash();
            if(PlayerData.instance.hasDashSlash)
                SkillCharms.Charms[Consts.greatslash].TryGrantCharm(false);
            else
                SkillCharms.Charms[Consts.greatslash].TryRevokeCharm();
        }

        public static void doAllSkills() {
            orig_GiveAllSkills();
            foreach(string charm in Consts.getPdVar.Keys) {
                if(charm != Consts.lantern) {
                    SkillCharms.Charms[charm].TryGrantCharm(false);
                }
            }
        }

        public static void doAllCharms() {
            orig_GiveAllCharms();
            PlayerData.instance.charmSlots += sCharm.additionalNotches;
        }
    }
}
