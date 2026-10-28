using EnokoMod.BattleActions;
using EnokoMod.Cards.Templates;
using EnokoMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace EnokoMod.Cards
{
    public sealed class EnokoRushdownDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Skill;
            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Red = 1 };
            config.UpgradedCost = new ManaGroup() { Any = 1 };
            config.Value1 = 3;
            config.UpgradedValue1 = 5;
            config.Value2 = 2;
            config.TargetType = TargetType.SingleEnemy;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoRushdownDef))]
    public sealed class EnokoRushdown : Card 
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
            yield return DebuffAction<EnokoConstrainSe>(selector.SelectedEnemy, level: Value1);
            if (precondition != null)
            {
                if (((SelectHandInteraction)precondition).SelectedCards.First() is TrapCard card)
                {
                    for (int i = 0; i < Value2; i++)
                    {
                        yield return new TriggerTrapAction(card, selector.SelectedEnemy);
                    }
                }
            }
            yield break;
        }
    }
}
