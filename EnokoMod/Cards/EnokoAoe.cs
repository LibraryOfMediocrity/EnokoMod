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
using System.Linq;
using LBoL.Core.Battle.Interactions;
using EnokoMod.BattleActions;
using EnokoMod.TrapToolBox;

namespace EnokoMod.Cards
{
    public sealed class EnokoAoeDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Common;
            config.Type = CardType.Attack;
            config.Colors = new List<ManaColor>() { ManaColor.White };
            config.Cost = new ManaGroup() { Any = 2, White = 1 };
            config.UpgradedCost = new ManaGroup() { Any = 1, White = 1 };
            config.TargetType = TargetType.AllEnemies;
            config.Damage = 15;
            config.UpgradedDamage = 17;
            config.RelativeEffects = new List<string>() { "TrapCardDisc" };
            config.UpgradedRelativeEffects = new List<string>() { "TrapCardDisc" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoAoeDef))]
    public sealed class EnokoAoe : Card
    {
        public override Interaction Precondition()
        {
            List<Card> cards = base.Battle.HandZone.Where((Card card) => card is TrapCard && card.CardType == CardType.Attack).ToList();
            if (cards.Count > 0)
            {
                return new SelectHandInteraction(1, 1, cards);
            }
            return null;
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return AttackAction(selector);
            if (precondition != null)
            {
                if (((SelectHandInteraction)precondition).SelectedCards[0] is TrapCard card) yield return new TriggerTrapAction(card, base.Battle.AllAliveEnemies.ToArray());
            }
            yield break;
        }
    }
}
