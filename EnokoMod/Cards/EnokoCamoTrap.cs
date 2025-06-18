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
    public sealed class EnokoCamoTrapDef : TrapTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetTrapDefaultConfig();
            config.Rarity = Rarity.Common;
            config.Type = CardType.Defense;
            config.TargetType = TargetType.Self;
            config.Cost = new ManaGroup() { Any = 1 };
            config.Block = 7;
            config.UpgradedBlock = 10;
            config.RelativeKeyword = Keyword.Block;
            config.UpgradedRelativeKeyword = Keyword.Block;
            config.Index = CardIndexGenerator.GetUniqueIndex(config, true);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoCamoTrapDef))]
    public sealed class EnokoCamoTrap : TrapCard
    {
        public override IEnumerable<BattleAction> OnTurnEndingInHand()
        {
            yield return new TriggerTrapAction(this);
            yield break;
        }

        public override Unit[] DefaultTarget => TrapTools.SelectUnit(base.Battle.Player);

        public override IEnumerable<BattleAction> TrapTriggered(Unit[] selector)
        {
            yield return DefenseAction();
            yield break;
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new TriggerTrapAction(this);
            yield break;
        }
    }
}
