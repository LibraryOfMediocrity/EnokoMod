using EnokoMod.Cards.Templates;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.EntityLib.Cards.Enemy;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace EnokoMod.Cards
{
    public sealed class EnokoWaitDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Defense;
            config.Colors = new List<ManaColor>() { ManaColor.White, ManaColor.Black };
            config.Cost = new ManaGroup() { Any = 1, Hybrid = 2, HybridColor = 1};
            config.Shield = 16;
            config.UpgradedShield = 20;
            config.Value1 = 1;
            config.RelativeKeyword = Keyword.Shield|Keyword.Echo;
            config.UpgradedRelativeKeyword = Keyword.Shield|Keyword.Echo;
            config.RelativeCards = new List<string>() { nameof(Xuanguang) };
            config.UpgradedRelativeCards = config.RelativeCards;
            config.TargetType = TargetType.Self;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoWaitDef))]
    public sealed class EnokoWait : Card
    {

        private string Header
        {
            get
            {
                return this.LocalizeProperty("Header");
            }
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return DefenseAction();
            List<Card> cards = Battle.HandZone.Concat(Battle.DrawZoneToShow.Concat(Battle.DiscardZone)).ToList();
            SelectCardInteraction interaction = new SelectCardInteraction(0, Value1, cards)
            {
                Source = this,
                Description = Header
            };
            yield return new InteractionAction(interaction);
            Card card = interaction.SelectedCards[0];
            if(card == null) yield break;
            card.IsEcho = true;
            card.NotifyChanged();
            yield return new MoveCardToDrawZoneAction(card, DrawZoneTarget.Top);
            yield return new AddCardsToDrawZoneAction(Library.CreateCards<Xuanguang>(1), DrawZoneTarget.Random);
            yield break;
        }
    }
}
