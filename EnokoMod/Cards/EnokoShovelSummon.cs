using EnokoMod.Cards.Templates;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace EnokoMod.Cards
{
    public sealed class EnokoShovelSummonDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Skill;
            config.Colors = new List<ManaColor>() { ManaColor.White };
            config.Cost = new ManaGroup() { White = 1 };
            config.TargetType = TargetType.Nobody;
            config.RelativeCards = new List<string>() { nameof(EnokoShovelTrap) };
            config.UpgradedRelativeCards = new List<string>() { nameof(EnokoShovelTrap) };
            config.RelativeKeyword = Keyword.Exile;
            config.UpgradedRelativeKeyword = Keyword.Exile;
            config.UpgradedKeywords = Keyword.Replenish;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoShovelSummonDef))]
    public sealed class EnokoShovelSummon : Card
    {
        private Card _exileTarget;

        private string Header { get => this.LocalizeProperty("Header"); }
        

        public override Interaction Precondition()
        {
            List<Card> cards = base.Battle.HandZone.Where((Card card) => card != this).ToList();
            if (cards.Count == 1)
            {
                _exileTarget = cards[0];
            }
            if (cards.Count < 1)
            {
                return null;
            }
            return new SelectHandInteraction(1, 1, cards)
            {
                Source = this,
                Description = Header
            }; 
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (precondition != null)
            {
                _exileTarget = ((SelectHandInteraction)precondition).SelectedCards[0];
            }
            if (_exileTarget != null)
            {
                yield return new ExileCardAction(_exileTarget);
            }
            if(base.Battle.HandZone.OfType<EnokoShovelTrap>().Count() > 0)
            {
                yield break;
            }
            yield return new AddCardsToHandAction(Library.CreateCard<EnokoShovelTrap>());
            yield break;
        }
    }
}
