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
using LBoL.EntityLib.Cards.Enemy;

namespace EnokoMod.Cards
{
    public sealed class EnokoTrueDrawDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Skill;
            config.Colors = new List<ManaColor>() { ManaColor.White, ManaColor.Black };
            config.Cost = new ManaGroup() { Any = 0 };
            config.UpgradedKeywords = Keyword.Initial;
            config.RelativeCards = new List<string>() { nameof(Xuanguang) };
            config.UpgradedRelativeCards = new List<string>() { nameof(Xuanguang) };
            config.TargetType = TargetType.Nobody;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoTrueDrawDef))]
    public sealed class EnokoTrueDraw : Card
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
            if (base.Battle.DrawZone.Count > 0)
            {
                SelectCardInteraction interaction = new SelectCardInteraction(1, 1, base.Battle.DrawZoneToShow)
                {
                    Source = this,
                    Description = Header
                };
                return interaction;
            }
            return null;
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (precondition != null)
            {
                Card card = ((SelectCardInteraction)precondition).SelectedCards.First();
                int index = base.Battle.DrawZone.IndexOf(card);
                for (int i = 0; i < index; i++)
                {
                    base.Battle.MoveCardToDrawZone(base.Battle.DrawZone.First(), DrawZoneTarget.Bottom);
                }
                yield return new AddCardsToDrawZoneAction(Library.CreateCards<Xuanguang>(1), DrawZoneTarget.Bottom);
                yield return new MoveCardAction(card, CardZone.Hand);
                for (int i = 0; i <= index; i++)
                {
                    base.Battle.MoveCardToDrawZone(base.Battle.DrawZone.Last(), DrawZoneTarget.Top);
                }
            }
            yield break;
        }
    }
}
