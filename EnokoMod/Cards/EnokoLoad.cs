using EnokoMod.Cards.Templates;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.EntityLib.Cards.Character.Cirno;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnokoMod.Cards
{
    public sealed class EnokoLoadDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Skill;
            config.Colors = new List<ManaColor>() { ManaColor.White };
            config.Cost = new ManaGroup() { Any = 1, White = 1 };
            config.Value1 = 20;
            config.UpgradedValue1 = 17;
            config.Value2 = 3;
            config.Mana = new ManaGroup() { Philosophy = 3 };
            config.TargetType = TargetType.Nobody;
            config.RelativeKeyword = Keyword.Exile|Keyword.Philosophy;
            config.UpgradedRelativeKeyword = Keyword.Exile|Keyword.Philosophy;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoLoadDef))]
    public sealed class EnokoLoad : Card
    {

        public override Interaction Precondition()
        {
            List<Card> allcards = Battle.DrawZoneToShow.Concat(Battle.HandZone).Concat(Battle.DiscardZone).ToList();
            if (allcards.Count >= Value1)
            {
                List<EnokoLoad> list = Library.CreateCards<EnokoLoad>(2, this.IsUpgraded).ToList<EnokoLoad>();
                EnokoLoad enokoLoad = list[0];
                EnokoLoad enokoLoad2 = list[1];
                enokoLoad.ChoiceCardIndicator = 1;
                enokoLoad2.ChoiceCardIndicator = 2;
                enokoLoad.SetBattle(base.Battle);
                enokoLoad2.SetBattle(base.Battle);
                return new MiniSelectCardInteraction(list);
            }
            else
            {
                return null;
            }
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            MiniSelectCardInteraction miniSelectCardInteraction = (MiniSelectCardInteraction)precondition;
            Card card = (miniSelectCardInteraction?.SelectedCard);
            List<Card> allcards = Battle.DrawZoneToShow.Concat(Battle.HandZone).Concat(Battle.DiscardZone).ToList();
            if (card != null && card.ChoiceCardIndicator == 2) 
            {
                SelectCardInteraction interaction = new SelectCardInteraction(0, Value2, allcards);
                yield return new InteractionAction(interaction);
                if (interaction.SelectedCards.Count > 0)
                {
                    yield return new ExileManyCardAction(interaction.SelectedCards);
                }
            }
            else
            {
                yield return new GainManaAction(Mana);
            }
            yield break;
        }
    }
}
