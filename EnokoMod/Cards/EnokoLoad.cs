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

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            List<Card> allcards = Battle.DrawZoneToShow.Concat(Battle.HandZone).Concat(Battle.DiscardZone).ToList();
            if (allcards.Count > Value1)
            {
                SelectCardInteraction interaction = new SelectCardInteraction(0, Value2, allcards);
                yield return new InteractionAction(interaction);
                if(interaction.SelectedCards.Count > 0)
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
