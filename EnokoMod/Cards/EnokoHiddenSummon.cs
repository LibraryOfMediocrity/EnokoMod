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
using LBoL.Core.Battle.Interactions;

namespace EnokoMod.Cards
{
    public sealed class EnokoHiddenSummonDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Skill;
            config.Colors = new List<ManaColor>() { ManaColor.White, ManaColor.Black };
            config.Cost = new ManaGroup() { Any = 1, White = 1, Black = 1 };
            config.UpgradedCost = new ManaGroup() { Any = 2, Hybrid = 1, HybridColor = 1 };
            config.RelativeCards = new List<string>() { nameof(EnokoHiddenTrap), nameof(EnokoBunkerTrap) };
            config.UpgradedRelativeCards = new List<string>() { nameof(EnokoHiddenTrap), nameof(EnokoBunkerTrap) };
            config.TargetType = TargetType.Nobody;
            config.Keywords = Keyword.Exile;
            config.UpgradedKeywords = Keyword.Exile;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoHiddenSummonDef))]
    public sealed class EnokoHiddenSummon : Card
    {
        private string Header
        {
            get
            {
                return this.LocalizeProperty("Header");
            }
        }

        public override Interaction Precondition()
        {
            List<Card> cards = new List<Card>() { Library.CreateCard<EnokoHiddenTrap>(), Library.CreateCard<EnokoBunkerTrap>() };
            return new SelectCardInteraction(1, 1, cards)
            {
                Source = this,
                Description = Header
            };
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (precondition != null)
            {
                if (((SelectCardInteraction)precondition).SelectedCards.Count > 0)
                {
                    yield return new AddCardsToHandAction(((SelectCardInteraction)precondition).SelectedCards);
                }
            }
            yield break;
        }
    }
}
