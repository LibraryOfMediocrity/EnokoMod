using EnokoMod.BattleActions;
using EnokoMod.Cards.Templates;
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
    public sealed class EnokoRefineDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Skill;
            config.Colors = new List<ManaColor>() { ManaColor.White };
            config.Cost = new ManaGroup() { White = 1 };
            config.UpgradedCost = new ManaGroup() { Any = 1 };
            config.RelativeKeyword = Keyword.Exile;
            config.UpgradedRelativeKeyword = Keyword.Exile;
            config.Value1 = 2;
            config.Value2 = 3;
            config.UpgradedValue2 = 4;
            config.TargetType = TargetType.Nobody;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoRefineDef))]
    public sealed class EnokoRefine : Card
    {

        protected override void OnEnterBattle(BattleController battle)
        {
            base.HandleBattleEvent<UnitEventArgs>(Battle.Player.TurnStarted, delegate { this.First = true; });
            First = true;
        }

        private string Header1
        {
            get
            {
                return this.LocalizeProperty("Header1");
            }
        }

        private string Header2
        {
            get
            {
                return this.LocalizeProperty("Header2");
            }
        }

        public int One { get => 1; }

        protected override string GetBaseDescription()
        {
            if (First || base.Battle == null)
            {
                return base.GetBaseDescription();
            }
            return base.GetExtraDescription1;
        }

        private bool First = true;

        public override bool Triggered => First;

        public override Interaction Precondition()
        {
            List<Card> list = base.Battle.HandZone.Where((Card hand) => hand is TrapCard).ToList<Card>();
            if (list.Count < 1)
            {
                return null;
            }
            return new SelectHandInteraction(0, 1, list)
            {
                Source = this,
                Description = Header1
            };
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (precondition != null && ((SelectHandInteraction)precondition).SelectedCards.Count > 0)
            {
                yield return new ExileCardAction(((SelectHandInteraction)precondition).SelectedCards[0]);
            }
            List<Card> list = base.Battle.HandZone.Where((Card hand) => hand is TrapCard).ToList<Card>();
            if (list.Count > 0)
            {
                SelectHandInteraction interaction = new SelectHandInteraction(0, 1, list)
                {
                    Source = this,
                    Description = Header2
                };
                yield return new InteractionAction(interaction);
                if (interaction.SelectedCards.Count > 0)
                {
                    for (int i = 0; i < Value1; i++)
                    {
                        yield return new TriggerTrapAction(interaction.SelectedCards[0] as TrapCard);
                    }
                }
            }
            if (First) yield return new DrawManyCardAction(Value2);
            this.First = false;
            yield break;
        }
    }
}
