using EnokoMod.BattleActions;
using EnokoMod.Cards.Templates;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace EnokoMod.Cards
{
    public sealed class EnokoShovelTrapDef : TrapTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetTrapDefaultConfig();
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Skill;
            config.Cost = new ManaGroup() { Any = 1 };
            config.Value1 = 1;
            config.Value2 = 1;
            config.UpgradedValue2 = 2;
            config.TargetType = TargetType.Nobody;
            config.Index = CardIndexGenerator.GetUniqueIndex(config, true);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoShovelTrapDef))]
    public sealed class EnokoShovelTrap : TrapCard
    {

        protected override void OnEnterBattle(BattleController battle)
        {
            base.ReactBattleEvent<CardUsingEventArgs>(base.Battle.CardUsed, new EventSequencedReactor<CardUsingEventArgs>(this.OnCardUsed));
            base.HandleBattleEvent<UnitEventArgs>(Battle.Player.TurnStarted, delegate { this.First = true; });
            First = true;
        }

        private bool First = true;

        private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
        {
            if (args.Card.CardType != CardType.Attack && First)
            {
                yield return new TriggerTrapAction(this);
                First = false;
            }
            yield break;
        }

        private string Header
        {
            get
            {
                return this.LocalizeProperty("Header");
            }
        }

        public override IEnumerable<BattleAction> TrapTriggered(Unit[] units)
        {
            List<Card> cards = base.Battle.ExileZone.Where((Card exile) => exile is TrapCard || !exile.IsExile).ToList();
            if (cards != null && cards.Count > 0)
            {
                SelectCardInteraction interaction = new SelectCardInteraction(0, 1/*Value1*/, cards)
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
            else
            {
                yield return new DrawManyCardAction(Value2);
            }
            yield break;
        }

    }
}
