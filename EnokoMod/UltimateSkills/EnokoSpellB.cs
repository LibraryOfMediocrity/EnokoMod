using EnokoMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace EnokoMod.UltimateSkills
{
    public sealed class EnokoUltBDef : EnokoUltTemplate
    {
        public override UltimateSkillConfig MakeConfig()
        {
            UltimateSkillConfig config = GetDefaulUltConfig();
            config.Damage = 18;
            config.Value1 = 6;
            config.RelativeEffects = new List<string>() { "EnokoConstrainSe" };
            return config;
        }
    }

    [EntityLogic(typeof(EnokoUltBDef))]
    public sealed class EnokoUltB : UltimateSkill
    {
        public EnokoUltB()
        {
            TargetType = TargetType.AllEnemies;
            GunName = "Simple2";
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector)
        {
            yield return PerformAction.Spell(base.Battle.Player, "EnokoUltB");
            Unit[] targets = selector.GetUnits(base.Battle);
            yield return new DamageAction(Owner, targets, Damage, GunName, GunType.Single);
            foreach (Unit target in targets) 
            {
                yield return new ApplyStatusEffectAction<EnokoConstrainSe>(target, Value1);
            }
            yield break;
        }
    }
}
