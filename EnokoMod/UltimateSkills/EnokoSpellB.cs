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
            config.Damage = 50;
            return config;
        }
    }

    [EntityLogic(typeof(EnokoUltBDef))]
    public sealed class EnokoUltB : UltimateSkill
    {
        public EnokoUltB()
        {
            TargetType = TargetType.SingleEnemy;
            GunName = "Simple2";
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector)
        {
            yield return PerformAction.Spell(base.Battle.Player, "EnokoUltB");
            EnemyUnit enemy = selector.GetEnemy(Battle);
            yield return new DamageAction(Owner, enemy, Damage, GunName, GunType.Single);
            yield break;
        }
    }
}
