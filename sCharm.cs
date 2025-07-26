using System.Collections.Generic;
using UnityEngine;
using Satchel;
using SFCore;

namespace SkillCharms {
    public abstract class sCharm: EasyCharm {
        public static Dictionary<string, string> boolOverrides = new();
        public static Dictionary<string, List<string>> intOverrides = new();

        protected override string GetName() => name;
        protected override int GetCharmCost() => cost;
        private string name;
        public int cost;

        public sCharm(string name, pdType type, int cost = 1) {
            this.name = name;
            this.cost = cost;
            string pdVar = Consts.getPdVar[name];
            switch(type) {
                case pdType.Int:
                    if(!intOverrides.ContainsKey(pdVar))
                        intOverrides.Add(pdVar, new());
                    intOverrides[pdVar].Add(name);
                    break;
                case pdType.Bool:
                default:
                    boolOverrides.Add(pdVar, name);
                    break;
            }
        }

        public void TryGrantCharm(bool equip) {
            sCharm charm = SkillCharms.Charms[name];
            if(!charm.GotCharm) {
                charm.GiveCharm(true);
                if(name is Consts.shadesoul or Consts.ddark or Consts.shriek) {
                    PlayerData.instance.charmSlots += 1;
                    if(equip) {
                        int lowerId = SkillCharms.Charms[name switch {
                            Consts.shadesoul => Consts.vs,
                            Consts.ddark => Consts.dive,
                            Consts.shriek => Consts.wraiths,
                            _ => null
                        }].Id;
                        PlayerData.instance.SetBool($"equippedCharm_{lowerId}", value: false);
                    }
                }
                else {
                    PlayerData.instance.charmSlots += charm.cost;
                }
                PlayerData.instance.SetBool($"equippedCharm_{Id}", value: equip);
            }

        }

        public void TryRevokeCharm() {
            sCharm charm = SkillCharms.Charms[name];
            if(charm.GotCharm) {
                bool levelSkill = (name is Consts.shadesoul or Consts.ddark or Consts.shriek);
                PlayerData.instance.charmSlots -= (levelSkill ? 1 : charm.cost);
                charm.TakeCharm();
            }
        }
    }

    public enum pdType {
        Bool,
        Int
    }

    public class dashCharm: sCharm {
        protected override Sprite GetSpriteInternal() => AssemblyUtils.GetSpriteFromResources("mdash.png");
        protected override string GetDescription() => Language.Language.Get("INV_DESC_DASH", "UI");
        public dashCharm(int cost) : base(Consts.dash, pdType.Bool, cost) { }
    }

    public class clawCharm: sCharm {
        protected override Sprite GetSpriteInternal() => AssemblyUtils.GetSpriteFromResources("claw.png");
        protected override string GetDescription() => Language.Language.Get("INV_DESC_WALLJUMP", "UI");
        public clawCharm(int cost) : base(Consts.claw, pdType.Bool, cost) { }
    }

    public class cdashCharm: sCharm {
        protected override Sprite GetSpriteInternal() => AssemblyUtils.GetSpriteFromResources("cdash.png");
        protected override string GetDescription() => Language.Language.Get("INV_DESC_SUPERDASH", "UI");
        public cdashCharm(int cost) : base(Consts.cdash, pdType.Bool, cost) { }
    }

    public class wingsCharm: sCharm {
        protected override Sprite GetSpriteInternal() => AssemblyUtils.GetSpriteFromResources("wings.png");
        protected override string GetDescription() => Language.Language.Get("INV_DESC_DOUBLEJUMP", "UI");
        public wingsCharm(int cost) : base(Consts.wings, pdType.Bool, cost) { }
    }

    public class shadecloakCharm: sCharm {
        protected override Sprite GetSpriteInternal() => AssemblyUtils.GetSpriteFromResources("shadecloak.png");
        protected override string GetDescription() => Language.Language.Get("INV_DESC_SHADOWDASH", "UI");
        public shadecloakCharm(int cost) : base(Consts.shadecloak, pdType.Bool, cost) { }
    }

    public class ismasCharm: sCharm {
        protected override Sprite GetSpriteInternal() => AssemblyUtils.GetSpriteFromResources("ismas.png");
        protected override string GetDescription() => Language.Language.Get("INV_DESC_ACIDARMOUR", "UI");
        public ismasCharm(int cost) : base(Consts.ismas, pdType.Bool, cost) { }
    }

    public class dnailCharm: sCharm {
        protected override Sprite GetSpriteInternal() => AssemblyUtils.GetSpriteFromResources("dnail.png");
        protected override string GetDescription() => Language.Language.Get("INV_DESC_DREAMNAIL_A", "UI");
        public dnailCharm(int cost) : base(Consts.dnail, pdType.Bool, cost) { }
    }

    public class dgateCharm: sCharm {
        protected override Sprite GetSpriteInternal() => AssemblyUtils.GetSpriteFromResources("dgate.png");
        protected override string GetDescription() => Language.Language.Get("INV_DESC_DREAMGATE", "UI");
        public dgateCharm(int cost) : base(Consts.dgate, pdType.Bool, cost) { }
    }

    public class wokenailCharm: sCharm {
        protected override Sprite GetSpriteInternal() => AssemblyUtils.GetSpriteFromResources("wokenail.png");
        protected override string GetDescription() => Language.Language.Get("INV_DESC_DREAMNAIL_B", "UI");
        public wokenailCharm(int cost) : base(Consts.wokenail, pdType.Bool, cost) { }
    }

    public class lanternCharm: sCharm {
        protected override Sprite GetSpriteInternal() => AssemblyUtils.GetSpriteFromResources("lantern.png");
        protected override string GetDescription() => Language.Language.Get("INV_DESC_LANTERN", "UI");
        public lanternCharm(int cost) : base(Consts.lantern, pdType.Bool, cost) { }
    }

    public class vsCharm: sCharm {
        protected override Sprite GetSpriteInternal() => AssemblyUtils.GetSpriteFromResources("vs.png");
        protected override string GetDescription() => Language.Language.Get("INV_DESC_SPELL_FIREBALL1", "UI");
        public vsCharm(int cost) : base(Consts.vs, pdType.Int, cost) { }
    }

    public class shadesoulCharm: sCharm {
        protected override Sprite GetSpriteInternal() => AssemblyUtils.GetSpriteFromResources("shadesoul.png");
        protected override string GetDescription() => Language.Language.Get("INV_DESC_SPELL_FIREBALL2", "UI");
        public shadesoulCharm(int cost) : base(Consts.shadesoul, pdType.Int, cost) { }
    }

    public class diveCharm: sCharm {
        protected override Sprite GetSpriteInternal() => AssemblyUtils.GetSpriteFromResources("dive.png");
        protected override string GetDescription() => Language.Language.Get("INV_DESC_SPELL_QUAKE1", "UI");
        public diveCharm(int cost) : base(Consts.dive, pdType.Int, cost) { }
    }

    public class ddarkCharm: sCharm {
        protected override Sprite GetSpriteInternal() => AssemblyUtils.GetSpriteFromResources("ddark.png");
        protected override string GetDescription() => Language.Language.Get("INV_DESC_SPELL_QUAKE2", "UI");
        public ddarkCharm(int cost) : base(Consts.ddark, pdType.Int, cost) { }
    }

    public class wraithsCharm: sCharm {
        protected override Sprite GetSpriteInternal() => AssemblyUtils.GetSpriteFromResources("wraiths.png");
        protected override string GetDescription() => Language.Language.Get("INV_DESC_SPELL_SCREAM1", "UI");
        public wraithsCharm(int cost) : base(Consts.wraiths, pdType.Int, cost) { }
    }

    public class shriekCharm: sCharm {
        protected override Sprite GetSpriteInternal() => AssemblyUtils.GetSpriteFromResources("shriek.png");
        protected override string GetDescription() => Language.Language.Get("INV_DESC_SPELL_SCREAM2", "UI");
        public shriekCharm(int cost) : base(Consts.shriek, pdType.Int, cost) { }
    }

    public class cycloneCharm: sCharm {
        protected override Sprite GetSpriteInternal() => AssemblyUtils.GetSpriteFromResources("cyclone.png");
        protected override string GetDescription() => Language.Language.Get("INV_DESC_ART_CYCLONE", "UI");
        public cycloneCharm(int cost) : base(Consts.cyclone, pdType.Bool, cost) { }
    }

    public class dashslashCharm: sCharm {
        protected override Sprite GetSpriteInternal() => AssemblyUtils.GetSpriteFromResources("dashslash.png");
        protected override string GetDescription() => Language.Language.Get("INV_DESC_ART_UPPER", "UI");
        public dashslashCharm(int cost) : base(Consts.dashslash, pdType.Bool, cost) { }
    }

    public class greatslashCharm: sCharm {
        protected override Sprite GetSpriteInternal() => AssemblyUtils.GetSpriteFromResources("greatslash.png");
        protected override string GetDescription() => Language.Language.Get("INV_DESC_ART_DASH", "UI");
        public greatslashCharm(int cost) : base(Consts.greatslash, pdType.Bool, cost) { }
    }
}
