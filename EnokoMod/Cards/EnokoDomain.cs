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
using EnokoMod.StatusEffects;
using LBoL.Core.Battle.Interactions;
using System.Linq;

namespace EnokoMod.Cards
{
    public sealed class EnokoDomainDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Rare;
            config.Type = CardType.Ability;
            config.Colors = new List<ManaColor>() { ManaColor.White };
            config.Cost = new ManaGroup() { Any = 1, White = 2 };
            config.TargetType = TargetType.Nobody;
            config.Value1 = 12;
            config.Value2 = 1;
            config.UpgradedValue2 = 2;
            config.RelativeKeyword = Keyword.Copy;
            config.UpgradedRelativeKeyword = Keyword.Copy;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoDomainDef))]
    public sealed class EnokoDomain : Card
    {

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<EnokoDomainSe>();
            List<Card> list = base.Battle.HandZone.Where((Card hand) => hand != this && hand.CanBeDuplicated).ToList<Card>();
            if (list.Count <= 0)
            {
                yield break;
            }
            SelectHandInteraction interaction = new SelectHandInteraction(0, Value2, list);
            yield return new InteractionAction(interaction);
            List<Card> copies = new List<Card>();
            foreach (Card item in interaction.SelectedCards)
            {
                copies.Add(item.CloneBattleCard());
            }
            yield return new AddCardsToHandAction(copies);
            yield break;
        }
    }
}
