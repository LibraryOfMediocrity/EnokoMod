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
    public sealed class EnokoAdaptDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Common;
            config.Type = CardType.Skill;
            config.Colors = new List<ManaColor>() { ManaColor.White };
            config.Cost = new ManaGroup() { Any = 1, White = 1 };
            config.UpgradedCost = new ManaGroup() { Any = 1 };
            config.RelativeCards = new List<string>() { "EnokoClawTrap", "EnokoCamoTrap", "EnokoEyeTrap" };
            config.UpgradedRelativeCards = new List<string>() { "EnokoClawTrap", "EnokoCamoTrap", "EnokoEyeTrap" };
            config.Keywords = Keyword.Exile;
            config.UpgradedKeywords = Keyword.Exile;
            config.TargetType = TargetType.Nobody;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoAdaptDef))]
    public sealed class EnokoAdapt : Card
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
            List<Card> cards = new List<Card>() { Library.CreateCard<EnokoClawTrap>(), Library.CreateCard<EnokoCamoTrap>(), Library.CreateCard<EnokoEyeTrap>() };
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
                if(((SelectCardInteraction)precondition).SelectedCards.Count > 0)
                {
                    yield return new AddCardsToHandAction(((SelectCardInteraction)precondition).SelectedCards);
                }
            }
            yield break;
        }
    }
}
