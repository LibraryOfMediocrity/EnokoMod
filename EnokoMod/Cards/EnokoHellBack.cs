using EnokoMod.Cards.Templates;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.EntityLib.Cards.Neutral.Colorless.YijiSkills;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;
using Unity.IO.Archive;
using Unity.Profiling;

namespace EnokoMod.Cards
{
    public sealed class EnokoHellBackDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Rare;
            config.Type = CardType.Attack;
            config.Colors = new List<ManaColor>() { ManaColor.White, ManaColor.Black };
            config.Cost = new ManaGroup() { Any = 1, Hybrid = 2, HybridColor = 1 };
            config.UpgradedCost = new ManaGroup() { Any = 1, Hybrid = 1, HybridColor = 1 };
            config.Damage = 16;
            config.Value1 = 1;
            config.Keywords = Keyword.Exile;
            config.UpgradedKeywords = Keyword.Exile;
            config.TargetType = TargetType.SingleEnemy;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoHellBackDef))]
    public sealed class EnokoHellBack : Card
    {

        static bool NuhUh = false;

        protected override string LocalizeProperty(string key, bool decorated = false, bool required = true)
        {
            if (!NuhUh && key is "DetailText") return null;
            else return base.LocalizeProperty(key, decorated, required);
        }

        private string Header1 => LocalizeProperty("Header1");

        private string Header2 => LocalizeProperty("Header2");

        private string Nope => LocalizeProperty("Nope");

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (RemoveFromBattleAfterPlay && !IsPlayTwiceToken)
            {
                yield return PerformAction.Chat(Battle.Player, Nope, chatTime: 2f, waitTime: 0.5f);
                yield break;
            }
            // note: check for no valid targets
            yield return AttackAction(selector);
            if(!Battle.ExileZone.Where((Card c) => !c.IsForbidden && !(c is EnokoHellBack)).Any()) yield break;
            SelectCardInteraction interaction = new SelectCardInteraction(0, 1, Battle.ExileZone.Where((Card c) => !c.IsForbidden && (!NuhUh || !(c is EnokoHellBack))))
            {
                Source = this,
                Description = Header1
            };
            yield return new InteractionAction(interaction);
            if (interaction.SelectedCards[0] == null) yield break;
            Card card = interaction.SelectedCards[0];
            if (card is EnokoHellBack hell)
            {
                NuhUh = true;
                hell.RemoveFromBattleAfterPlay = true;
                yield return new PlayCardAction(card);
                hell.RemoveFromBattleAfterPlay = false;
                interaction = new SelectCardInteraction(1, 1, Battle.ExileZone.Where((Card c) => !c.IsForbidden && !(c is EnokoHellBack)))
                {
                    Source = this,
                    Description = Header2
                };
                yield return new InteractionAction(interaction);
                if (interaction.SelectedCards[0] == null) yield break;
                card = interaction.SelectedCards[0];
            }
            bool unexile = false;
            if (!card.IsExile)
            {
                card.IsExile = true;
                unexile = true;
            }
            yield return new PlayCardAction(card, selector);
            if (unexile) card.IsExile = false;
            yield break;
        }
    }
}
