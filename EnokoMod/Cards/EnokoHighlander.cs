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
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace EnokoMod.Cards
{
    public sealed class EnokoHighlanderDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Rare;
            config.Type = CardType.Ability;
            config.Colors = new List<ManaColor>() { ManaColor.Blue };
            config.Cost = new ManaGroup() { Any = 2, Blue = 1 };
            config.Value1 = 1;
            config.UpgradedKeywords = Keyword.Retain;
            config.RelativeKeyword = Keyword.Exile;
            config.UpgradedRelativeKeyword = Keyword.Exile;
            config.RelativeEffects = new List<string>() { nameof(TrapCardDisc) };
            config.UpgradedRelativeEffects = config.RelativeEffects;
            config.TargetType = TargetType.Nobody;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoHighlanderDef))]
    public sealed class EnokoHighlander : Card 
    {

        public override Interaction Precondition()
        {
            if (Battle.Player.HasStatusEffect<EnokoHighlanderSe>())
            {
                return null;
            }
            else
            {
                SelectHandInteraction interaction = new SelectHandInteraction(1, 1, Battle.HandZone.Where((Card c) => c is TrapCard))
                {
                    Source = this,
                    Description = Header1
                };
                return interaction;
            }
        }

        private string Header1 { get => this.LocalizeProperty("Header1"); }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<EnokoHighlanderSe>(level: Value1);
            if(precondition != null)
            {
                Card card = ((SelectHandInteraction)precondition).SelectedCards.FirstOrDefault();
                if(card != null && card is TrapCard trapcard)
                {
                    Battle.Player.GetStatusEffect<EnokoHighlanderSe>().TrapCard = trapcard;
                    List<Card> trapcards = Battle.HandZone.Where(card => card is TrapCard).ToList();
                    foreach (Card c in trapcards)
                    {
                        yield return new ExileCardAction(card);
                        yield return new TriggerTrapAction(trapcard);
                    }
                }
            }
            yield break;
        }
    }

    public sealed class EnokoHighlanderSeDef : EnokoStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.Keywords = Keyword.Exile;
            return config;
        }
    }

    public sealed class EnokoHighlanderSe : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<CardsEventArgs>(Battle.CardsAddedToHand, this.OnCardAddedToHand);
            base.ReactOwnerEvent<CardEventArgs>(Battle.CardDrawn, this.OnCardDrawn);
            base.ReactOwnerEvent<CardMovingEventArgs>(Battle.CardMoved, this.OnCardMoved);
        }

        public TrapCard TrapCard {  get; set; }

        public string CardName { get => TrapCard.Name; }

        private IEnumerable<BattleAction> OnCardAddedToHand (CardsEventArgs args)
        {
            foreach (Card card in args.Cards)
            {
                if(card is TrapCard && card != TrapCard)
                {
                    yield return new ExileCardAction(card);
                    yield return new TriggerTrapAction(TrapCard);
                }
            }
            yield break;
        }

        private IEnumerable<BattleAction> OnCardDrawn (CardEventArgs args)
        {
            Card card = args.Card;
            if (card is TrapCard && card != TrapCard)
            {
                yield return new ExileCardAction(card);
                yield return new TriggerTrapAction(TrapCard);
            }
            yield break;
        }

        private IEnumerable<BattleAction> OnCardMoved (CardMovingEventArgs args)
        {
            Card card = args.Card;
            if (card is TrapCard &&  args.DestinationZone == CardZone.Hand && card != TrapCard)
            {
                yield return new ExileCardAction(card);
                yield return new TriggerTrapAction(TrapCard);
            }
            yield break;
        }


    }
}
