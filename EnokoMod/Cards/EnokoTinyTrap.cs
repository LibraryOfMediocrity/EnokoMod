using EnokoMod.Cards.Templates;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Units;
using EnokoMod.StatusEffects;
using EnokoMod.BattleActions;
using EnokoMod.TrapToolBox;

namespace EnokoMod.Cards
{
    public sealed class EnokoTinyTrapDef : TrapTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetTrapDefaultConfig();
            config.Rarity = Rarity.Common;
            config.Cost = new ManaGroup() { Any = 1 };
            config.Damage = 6;
            config.Value1 = 2;
            config.Value2 = 3;
            config.UpgradedValue1 = 3;
            config.Index = CardIndexGenerator.GetUniqueIndex(config, true);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoTinyTrapDef))]
    public sealed class EnokoTinyTrap : TrapCard
    {
        protected override void OnEnterBattle(BattleController battle)
        {
            base.ReactBattleEvent<UnitEventArgs>(base.Battle.Player.TurnEnding, new EventSequencedReactor<UnitEventArgs>(this.OnTurnEnding));
        }

        private IEnumerable<BattleAction>OnTurnEnding(UnitEventArgs args)
        {
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
}
