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
using System.Collections.Generic;
using System.Linq;

namespace EnokoMod.Cards
{
    public sealed class EnokoKillDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Attack;
            config.Colors = new List<ManaColor>() { ManaColor.White };
            config.Cost = new ManaGroup() { Any = 2, White = 1 };
            config.TargetType = TargetType.SingleEnemy;
            config.Damage = 18;
            config.UpgradedDamage = 24;
            config.UpgradedKeywords = Keyword.Accuracy;
            config.RelativeEffects = new List<string>() { nameof(TrapCardDisc) };
            config.UpgradedRelativeEffects = config.RelativeEffects;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoKillDef))]
    public sealed class EnokoKill : Card
    {

        protected override void OnEnterBattle(BattleController battle)
        {
            base.HandleBattleEvent<DieEventArgs>(base.Battle.EnemyDied, delegate(DieEventArgs args) 
            {
                if (args.DieSource == this) Killed = true;
            });
        }

        private bool Killed;

        private string Header
        {
            get
            {
                return this.LocalizeProperty("Header");
            }
        }


        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            Killed = false;
            yield return AttackAction(selector);
            List<TrapCard> trapCards = Battle.HandZone.Where((Card c) => c is TrapCard).Cast<TrapCard>().ToList();
            if (trapCards.Count <= 0) yield break;
            if (Killed)
            {
                foreach(TrapCard card in trapCards)
                {
                    if (base.Battle.BattleShouldEnd) yield break;                
                    yield return new TriggerTrapAction(card);
                }
            }
            else
            {
                SelectHandInteraction interaction = new SelectHandInteraction(1, 1, trapCards)
                {
                    Source = this,
                    Description = Header
                };
                yield return new InteractionAction(interaction);
                if (interaction.SelectedCards[0] is TrapCard trap)
                {
                    yield return new TriggerTrapAction(trap, selector.SelectedEnemy);
                }
                
            }
            yield break;
        }
    }
}
