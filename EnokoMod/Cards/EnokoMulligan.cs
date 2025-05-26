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
using LBoL.Base.Extensions;
using LBoL.Core.Battle.Interactions;
using System.Linq;

namespace EnokoMod.Cards
{
    public sealed class EnokoMulliganDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Common;
            config.Type = CardType.Skill;
            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Black = 1 };
            config.UpgradedCost = new ManaGroup() { Any = 1 };
            config.Value1 = 1;
            config.UpgradedKeywords = Keyword.Replenish;
            config.TargetType = TargetType.Nobody;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoMulliganDef))]
    public sealed class EnokoMulligan : Card
    {
        private string Header1
        {
            get
            {
                return this.LocalizeProperty("Header1");
            }
        }

        private string Header2
        {
            get
            {
                return this.LocalizeProperty("Header2");
            }
        }

        public override Interaction Precondition()
        {
            if (base.Battle.HandZone.Where((Card hand) => hand != this).Count() > 0)
            {
                SelectCardInteraction interaction1 = new SelectCardInteraction(1, 1, base.Battle.HandZone.Where((Card hand) => hand != this))
                {
                    Source = this,
                    Description = Header1
                };
                return interaction1;
            }
            return null;
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (precondition != null)
            {
                Card card1 = ((SelectCardInteraction)precondition).SelectedCards.First();
                if (base.Battle.DrawZone.Where((Card card) => card.CardType == card1.CardType).Count() > 0)
                {
                    SelectCardInteraction interaction2 = new SelectCardInteraction(1, 1, base.Battle.DrawZoneToShow.Where((Card card) => card.CardType == card1.CardType))
                    {
                        Source = this,
                        Description = Header2
                    };
                    yield return new InteractionAction(interaction2, false);
                    Card card2 = interaction2.SelectedCards.First();
                    int index = base.Battle.DrawZone.IndexOf(card2);
                    for (int i = 0; i < index; i++)
                    {
                        base.Battle.MoveCardToDrawZone(base.Battle.DrawZone.First(), DrawZoneTarget.Bottom);
                    }
                    yield return new MoveCardToDrawZoneAction(card1, DrawZoneTarget.Bottom);
                    yield return new MoveCardAction(card2, CardZone.Hand);
                    for (int i = 0; i <= index; i++)
                    {
                        base.Battle.MoveCardToDrawZone(base.Battle.DrawZone.Last(), DrawZoneTarget.Top);
                    }
                }
                else
                {
                    yield return new MoveCardToDrawZoneAction(card1, DrawZoneTarget.Random);
                    yield return new DrawCardAction();
                }
            }
            yield break;
        }
    }
}
