using Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Satchel;
using SFCore;

namespace SkillCharms
{
    public class SkillCharms : Mod, ILocalSettings<Settings>
    {
        new public string GetName() => "SkillCharms";
        public override string GetVersion() => "1.0.2.0";

        private bool isCountingGameCompletion = false;

        internal Settings localSettings = new();
        internal static Dictionary<string, sCharm> Charms = new() {
            { Consts.dash, new dashCharm(1) },
            { Consts.claw, new clawCharm(2) },
            { Consts.cdash, new cdashCharm(1) },
            { Consts.wings, new wingsCharm(1) },
            { Consts.shadecloak, new shadecloakCharm(1) },
            { Consts.ismas, new ismasCharm(0) },
            { Consts.dnail, new dnailCharm(1) },
            { Consts.dgate, new dgateCharm(1) },
            { Consts.wokenail, new wokenailCharm(0) },
            { Consts.lantern, new lanternCharm(0) },
            { Consts.vs, new vsCharm(1) },
            { Consts.shadesoul, new shadesoulCharm(2) },
            { Consts.dive, new diveCharm(1) },
            { Consts.ddark, new ddarkCharm(2) },
            { Consts.wraiths, new wraithsCharm(1) },
            { Consts.shriek, new shriekCharm(2) },
            { Consts.cyclone, new cycloneCharm(1) },
            { Consts.dashslash, new dashslashCharm(1) },
            { Consts.greatslash, new greatslashCharm(1) }
        };

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects) {
            if(ModHooks.GetMod("Randomizer 4") is Mod) {
                Log("SkillCharms may not work properly with Rando. Randomized Skills™ may not be granted correctly.");
            }
            ModHooks.AfterSavegameLoadHook += LoadSave;
            On.PlayerData.GetBool += GetCharmBool;
            On.PlayerData.GetInt += GetCharmInt;
            On.PlayMakerFSM.OnEnable += EditFsm;
            On.HutongGames.PlayMaker.Actions.SetPlayerDataBool.OnEnter += EditSetBool;
            On.HutongGames.PlayMaker.Actions.SetPlayerDataInt.OnEnter += EditSetInt;

            if(ModHooks.GetMod("DebugMod") is Mod) {
                DebugInterop.HookDebug();
            }
        }

        private void LoadSave(SaveGameData data) {
            foreach(string boolCharm in Consts.getBoolCharm.Keys) {
                if(PlayerData.instance.GetBoolInternal(boolCharm) && !Charms[Consts.getBoolCharm[boolCharm]].GotCharm) {
                    Charms[Consts.getBoolCharm[boolCharm]].TryGrantCharm(true);
                }
            }
            foreach(string intCharmKey in Consts.getIntCharm.Keys) {
                int pdInt = PlayerData.instance.GetIntInternal(intCharmKey);
                foreach(int intCharm in Consts.getIntCharm[intCharmKey].Keys) {
                    if(pdInt >= intCharm && !Charms[Consts.getIntCharm[intCharmKey][intCharm]].GotCharm) {
                        Charms[Consts.getIntCharm[intCharmKey][intCharm]].TryGrantCharm(true);
                    }
                }
            }
        }

        private bool GetCharmBool(On.PlayerData.orig_GetBool orig, PlayerData self, string boolName) {
            if(boolName == "mothDeparted" && Environment.StackTrace.Contains("CountGameCompletion"))
                isCountingGameCompletion = false;
            if(isCountingGameCompletion)
                return orig(self, boolName);
            if(boolName == nameof(PlayerData.hasNailArt)) {
                foreach(string art in new string[] { Consts.cyclone, Consts.dashslash, Consts.greatslash }) {
                    if(Charms[art].IsEquipped)
                        return true;
                }
                return false;
            }
            if(boolName == nameof(PlayerData.canDash)) {
                return Charms[Consts.dash].IsEquipped;
            }
            if(sCharm.boolOverrides.ContainsKey(boolName)) {
                return Charms[sCharm.boolOverrides[boolName]].IsEquipped;
            }
            return orig(self, boolName);
        }

        private int GetCharmInt(On.PlayerData.orig_GetInt orig, PlayerData self, string intName) {
            if(intName == "fireballLevel" && Environment.StackTrace.Contains("CountGameCompletion")) {
                isCountingGameCompletion = true;
            }
            if(isCountingGameCompletion)
                return orig(self, intName);
            if(sCharm.intOverrides.ContainsKey(intName)) {
                var equipped = sCharm.intOverrides[intName].Where(name => Charms[name].IsEquipped);
                return equipped.Any() ? Charms[equipped.First()].cost : 0;
            }
            return orig(self, intName);
        }

        private void EditFsm(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self) {
            orig(self);
            if(self.FsmName == "UI Charms") {
                self.GetValidState("Slot Open?").InsertCustomAction(() => {
                    int itemNumber = self.FsmVariables.GetFsmInt("Current Item Number").Value;
                    foreach((string name, string name2) in new (string, string)[] {
                        (Consts.vs, Consts.shadesoul),
                        (Consts.shadesoul, Consts.vs),
                        (Consts.dive, Consts.ddark),
                        (Consts.ddark, Consts.dive),
                        (Consts.wraiths, Consts.shriek),
                        (Consts.shriek, Consts.wraiths)
                    }) {
                        if(Charms[name].Id == itemNumber && Charms[name2].IsEquipped) {
                            self.SendEvent("CANCEL");
                            break;
                        }
                    }
                }, 0);
            }
            else if(self.FsmName == "Shiny Control" && ModHooks.GetMod("ItemChanger") is Mod) {
                if(self.GetValidState("Trink Flash").GetFirstActionOfType<ItemChanger.FsmStateActions.Lambda>() != null) {
                    //If I ever figure out rando integration, it would probably go here
                }
            }
        }

        private void EditSetBool(On.HutongGames.PlayMaker.Actions.SetPlayerDataBool.orig_OnEnter orig, HutongGames.PlayMaker.Actions.SetPlayerDataBool self) {
            orig(self);
            if(self.value.Value) {
                if(Consts.getBoolCharm.TryGetValue(self.boolName.Value, out string charm)) {
                    Charms[charm].TryGrantCharm(true);
                }
            }
        }

        private void EditSetInt(On.HutongGames.PlayMaker.Actions.SetPlayerDataInt.orig_OnEnter orig, HutongGames.PlayMaker.Actions.SetPlayerDataInt self) {
            orig(self);
            int val = self.value.Value;
            if(Consts.getIntCharm.TryGetValue(self.intName.Value, out Dictionary<int, string> spell)) {
                sCharm charm1 = Charms[spell[1]];
                sCharm charm2 = Charms[spell[2]];
                if(val == 1)
                    charm1.TryGrantCharm(true);
                else if(val == 2) {
                    if(charm1.GotCharm) {
                        charm2.TryGrantCharm(charm1.IsEquipped);
                    }
                    else {
                        charm1.TryGrantCharm(false);
                        charm2.TryGrantCharm(true);
                    }
                }
            }
        }

        public void OnLoadLocal(Settings s) {
            localSettings = s;
            if(s.Charms != null) {
                foreach(var kvp in s.Charms) {
                    if(Charms.TryGetValue(kvp.Key, out sCharm m)) {
                        m.RestoreCharmState(kvp.Value);
                    }
                }
            }
        }

        public Settings OnSaveLocal() {
            localSettings.Charms = new();
            foreach(var kvp in Charms) {
                if(Charms.TryGetValue(kvp.Key, out sCharm m)) {
                    localSettings.Charms[kvp.Key] = m.GetCharmState();
                }
            }
            return localSettings;
        }
    }

    public class Settings {
        public Dictionary<string, EasyCharmState> Charms;
    }
}