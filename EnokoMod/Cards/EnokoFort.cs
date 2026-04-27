using EnokoMod.BattleActions;
using EnokoMod.Cards.Templates;
using EnokoMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnokoMod.Cards
{
    public sealed class EnokoFortDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Common;
            config.Type = CardType.Defense;
            config.Colors = new List<ManaColor>() { ManaColor.White };
            config.Cost = new ManaGroup() { White = 2 };
            config.TargetType = TargetType.Nobody;
            config.Block = 13;
            config.UpgradedBlock = 18;
            config.Keywords = Keyword.Debut;
            config.UpgradedKeywords = Keyword.Debut;
            config.RelativeKeyword = Keyword.Block;
            config.UpgradedRelativeKeyword = Keyword.Block;
            config.RelativeCards = new List<string>() { "EnokoBearTrap" };
            config.UpgradedRelativeCards = new List<string>() { "EnokoBearTrap" };
            config.RelativeEffects = new List<string>() { nameof(TrapCardDisc) };
            config.UpgradedRelativeEffects = config.RelativeEffects;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoFortDef))]
    public sealed class EnokoFort : Card
    {

        protected override string GetBaseDescription()
        {
            if (DebutActive) return base.GetBaseDescription();
            return base.GetExtraDescription1;
        }

        private string Header => this.LocalizeProperty("Header");

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return DefenseAction();
            if(this.IsUpgraded)
            {
                List<Card> list = base.Battle.HandZone.Where((Card hand) => hand is TrapCard).ToList<Card>();
                if (list.Count > 0)
                {
                    SelectHandInteraction interaction = new SelectHandInteraction(0, 1, list)
                    {
                        Source = this,
                        Description = Header
                    };
                    yield return new InteractionAction(interaction);
                    if (interaction.SelectedCards.Count > 0) yield return new TriggerTrapAction(interaction.SelectedCards[0] as TrapCard);
                }
            }
            if (TriggeredAnyhow)
            {
                yield return new AddCardsToHandAction(new Card[] { Library.CreateCard<EnokoBearTrap>() });
            }
            yield break;
        }
    }
}
