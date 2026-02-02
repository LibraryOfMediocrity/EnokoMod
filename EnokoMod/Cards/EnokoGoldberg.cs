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
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.Cards.Neutral.Blue;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace EnokoMod.Cards
{
    public sealed class EnokoGoldbergDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Rare;
            config.Type = CardType.Ability;
            config.Colors = new List<ManaColor>() { ManaColor.White };
            config.Cost = new ManaGroup() { White = 2 };
            config.UpgradedCost = new ManaGroup() { Any = 0 };
            config.Value1 = 2;
            config.RelativeEffects = new List<string>() { nameof(TrapCardDisc) };
            config.UpgradedRelativeEffects = config.RelativeEffects;
            config.TargetType = TargetType.Nobody;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoGoldbergDef))]
    public sealed class EnokoGoldberg : Card
    {

        public override Interaction Precondition()
        {
            SelectHandInteraction interaction = new SelectHandInteraction(1, 1, Battle.HandZone.Where((Card c) => c is TrapCard))
            {
                Source = this,
                Description = Header1
            };
            return interaction;
        }

        private string Header1 { get => this.LocalizeProperty("Header1"); }

        private string Header2 { get => this.LocalizeProperty("Header2"); }

        public override bool CanUse => Battle.HandZone.Where((Card c) => c is TrapCard).Count() > 1;

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if(precondition != null)
            {
                Card card = ((SelectHandInteraction)precondition).SelectedCards.FirstOrDefault();
                TrapCard first;
                TrapCard second;
                if(card != null && card is TrapCard trap1)
                {
                    first = trap1;
                }
                else yield break;
                
                SelectHandInteraction interaction = new SelectHandInteraction(1, 1, Battle.HandZone.Where((Card c) => c is TrapCard && c != first))
                {
                    Source = this,
                    Description = Header2
                };
                yield return new InteractionAction(interaction);
                card = interaction.SelectedCards.FirstOrDefault();
                if (card != null && card is TrapCard trap2)
                {
                    second = trap2;
                }
                else yield break;

                yield return BuffAction<EnokoGoldbergSe>(limit: IsUpgraded? 1 : 0);
                StatusEffect effect = Battle.Player.StatusEffects.LastOrDefault((StatusEffect s) => s.GetType() == typeof(EnokoGoldbergSe));
                if (effect != null && effect is EnokoGoldbergSe ability)
                {
                    ability.Card1 = first;
                    ability.Card2 = second;
                }
            }
            yield break;
        }
    }

    public sealed class EnokoGoldbergSeDef : EnokoStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.HasLevel = false;
            config.IsStackable = false;
            config.Type = StatusEffectType.Special;
            return config;
        }
    }

    [EntityLogic(typeof(EnokoGoldbergSeDef))]
    public sealed class EnokoGoldbergSe : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<CardEventArgs>(Battle.CardRemoved, OnCardRemoved);
            base.ReactOwnerEvent<TriggerTrapEventArgs>(EnokoGameEvents.PostTriggerEvent, OnTrapTriggered);
        }

        public TrapCard Card1 { get; set; }

        public TrapCard Card2 { get; set; }

        public string First { get => Card1.Name; }

        public string Second { get => Card2.Name; }

        private IEnumerable<BattleAction> OnTrapTriggered(TriggerTrapEventArgs args)
        {
            if (args.ActionSource is EnokoGoldbergSe || Card1 == null || Card2 == null) yield break;
            if(args.Card == Card1 && Card2.Zone == CardZone.Hand)
            {
                Unit[] units = args.Units.Where((Unit unit) => unit != null && !unit.IsDead).ToArray();
                yield return new TriggerTrapAction(Card2, units.Any()? units : Card2.DefaultTarget);
            }
            yield break;
        }

        private IEnumerable<BattleAction> OnCardRemoved(CardEventArgs args)
        {
            if(args.Card == Card1 || args.Card == Card2)
            {
                yield return new AddCardsToHandAction(Library.CreateCard<EnokoGoldberg>(Limit == 1));
                yield return new RemoveStatusEffectAction(this);
            }
            yield break;
        }
    }
}
