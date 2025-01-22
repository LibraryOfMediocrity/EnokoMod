using EnokoMod.BattleActions;
using EnokoMod.Cards.Templates;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace EnokoMod.Cards
{
    public sealed class EnokoTestTrapDef: TrapTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetTrapDefaultConfig();
            config.Cost = new ManaGroup() { Any = 1 };
            config.Damage = 8;
            config.UpgradedDamage = 10;
            config.Value1 = 1;
            config.Rarity = Rarity.Common;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoTestTrapDef))]
    public sealed class EnokoTestTrap : TrapCard
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new TriggerTrapAction(this, selector.GetUnits(Battle));
        }

        public override IEnumerable<BattleAction> TrapTriggered(Unit[] selector)
        {
            yield return AttackAction(selector, this.GunName, base.Damage);
            foreach(var action in DebuffAction<TempFirepowerNegative>(selector, base.Value1)) yield return action;
            yield break;
        }

    }
}
