using EnokoMod.Cards.Templates;
using EnokoMod.StatusEffects;
using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnokoMod.Cards
{
    public sealed class EnokoGraveDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Rare;
            config.Type = CardType.Ability;
            config.Colors = new List<ManaColor>() { ManaColor.White };
            config.Cost = new ManaGroup() { White = 3 };
            config.UpgradedCost = new ManaGroup() { White = 2 };
            config.Value1 = 1;
            config.Keywords = Keyword.Initial;
            config.UpgradedKeywords = Keyword.Initial;
            config.RelativeKeyword = Keyword.Exile;
            config.UpgradedRelativeKeyword = Keyword.Exile;
            config.RelativeCards = new List<string>() { nameof(EnokoSpade) };
            config.UpgradedRelativeCards = config.RelativeCards;
            config.TargetType = TargetType.Nobody;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoGraveDef))]
    public sealed class EnokoGrave : Card
    {

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (Battle.DrawZone.Count > 0)
            {
                int num = (int)Math.Ceiling(Battle.DrawZone.Count / 2f);
                List<Card> priority = Battle.DrawZone.Where((Card c) => !c.IsExile).ToList();
                List<Card> other = Battle.DrawZone.Where((Card c) => c.IsExile).ToList();
                List<Card> exiles = priority.SampleManyOrAll(num, base.GameRun.BattleRng).ToList();
                if (exiles.Count < num)
                {
                    exiles.AddRange(other.SampleManyOrAll(num - exiles.Count, base.GameRun.BattleRng));
                }
                yield return new ExileManyCardAction(exiles);
            }
            if (IsUpgraded) yield return new AddCardsToHandAction(Library.CreateCard<EnokoSpade>());
            yield return BuffAction<EnokoGraveSe>(Value1);
            yield break;
        }
    }

    public sealed class EnokoGraveSeDef : EnokoStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            return config;
        }
    }

    [EntityLogic(typeof(EnokoGraveSeDef))]
    public sealed class EnokoGraveSe : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<UnitEventArgs>(Battle.Player.TurnStarting, OnTurnStarting);
        }

        private IEnumerable<BattleAction> OnTurnStarting(UnitEventArgs args)
        {
            yield return new AddCardsToHandAction(Library.CreateCards<EnokoSpade>(Level));
            yield break;
        }
    }

    public sealed class EnokoSpadeDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.HideMesuem = true;
            config.IsPooled = false;
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Skill;
            config.Colors = new List<ManaColor>() { ManaColor.White };
            config.Cost = new ManaGroup() { Any = 0 };
            config.Mana = new ManaGroup() { Colorless = 1 };
            config.Keywords = Keyword.Exile | Keyword.Retain;
            config.UpgradedKeywords = Keyword.Exile | Keyword.Retain;
            config.RelativeEffects = new List<string>() { nameof(TrapCardDisc) };
            config.UpgradedRelativeEffects = config.RelativeEffects;
            config.TargetType = TargetType.Nobody;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoSpadeDef))]
    public sealed class EnokoSpade : Card
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
            List<Card> cards = base.Battle.ExileZone.Where((Card exile) => exile is TrapCard || !exile.IsExile).ToList();
            if (cards != null && cards.Count > 0)
            {
                SelectCardInteraction interaction = new SelectCardInteraction(1, 1/*Value1*/, cards)
                {
                    Source = this,
                    Description = Header
                };
                yield return new InteractionAction(interaction);
                if (interaction.SelectedCards != null && interaction.SelectedCards.Count > 0)
                {
                    yield return new MoveCardAction(interaction.SelectedCards[0], CardZone.Hand);
                }
            }
            if (this.IsUpgraded) yield return new GainManaAction(Mana);
            yield break;
        }
    }
}
