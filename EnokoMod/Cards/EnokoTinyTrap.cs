using EnokoMod.BattleActions;
using EnokoMod.Cards.Templates;
using EnokoMod.StatusEffects;
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
    public sealed class EnokoTinyTrapDef : TrapTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetTrapDefaultConfig();
            config.Rarity = Rarity.Common;
            config.Cost = new ManaGroup() { Any = 1 };
            config.Damage = 5;
            config.Value1 = 2;
            config.Value2 = 3;
            config.UpgradedValue2 = 4;
            config.Index = CardIndexGenerator.GetUniqueIndex(config, true);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoTinyTrapDef))]
    public sealed class EnokoTinyTrap : TrapCard
    {
        public override IEnumerable<BattleAction> OnTurnEndingInHand()
        {
            if(base.Battle.BattleShouldEnd) yield break; 
            yield return new TriggerTrapAction(this, DefaultTarget);
            yield break;
        }

        public override int AdditionalDamage => base.GetSeLevel<EnokoTinySe>();

        public override IEnumerable<BattleAction> TrapTriggered(Unit[] selector)
        {
            for (int i = 0; i < Value1; i++)
            {
                yield return AttackAction(selector, this.GunName, base.Damage);
            }
            yield break;
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new TriggerTrapAction(this, selector.SelectedEnemy);
            yield return BuffAction<EnokoTinySe>(Value2);
            yield break;
        }
    }

    public sealed class EnokoTinySeDef : EnokoStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            return config;
        }
    }

    [EntityLogic(typeof(EnokoTinySeDef))]
    public sealed class EnokoTinySe : StatusEffect
    {

    }
}
