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
using EnokoMod.StatusEffects;
using System.Linq;
using EnokoMod.BattleActions;
using LBoL.Core.Units;
using LBoL.EntityLib.StatusEffects.Cirno;
using LBoL.Core.StatusEffects;

namespace EnokoMod.Cards
{
    public sealed class EnokoPitfallDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Defense;
            config.Colors = new List<ManaColor>() { ManaColor.White, ManaColor.Black };
            config.Cost = new ManaGroup() { Any = 1, Hybrid = 1, HybridColor = 1 };
            config.Block = 13;
            config.UpgradedBlock = 17;
            config.Value1 = 2;
            config.UpgradedValue1 = 3;
            config.TargetType = TargetType.Self;
            config.RelativeKeyword = Keyword.Block;
            config.UpgradedRelativeKeyword = Keyword.Block;
            config.RelativeEffects = new List<string>() { nameof(EnokoConstrainSe), nameof(TrapCardDisc), nameof(Burial) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(EnokoConstrainSe), nameof(TrapCardDisc), nameof(Burial) };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoPitfallDef))]
    public sealed class EnokoPitfall : BurialCard
    {

        protected override void OnEnterBattle(BattleController battle)
        {
            base.ReactBattleEvent<CardUsingEventArgs>(base.Battle.CardUsed, new EventSequencedReactor<CardUsingEventArgs>(this.OnCardUsed));
        }


        private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
        {
            if (IsBuried && args.Card.CardType != CardType.Attack)
            {
                foreach (BattleAction action in DebuffAction<EnokoConstrainSe>(base.Battle.AllAliveEnemies, Value1))
                {
                    yield return action;
                }
            }
            yield break;
        }

        public override bool Triggered
        {
            get
            {
                return base.Battle != null && base.Battle.AllAliveEnemies.Any((EnemyUnit enemy) => enemy.HasStatusEffect<EnokoConstrainSe>());
            }
        }

        public override int CardIndex { get => 0; }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return DefenseAction();
            if (!base.Battle.BattleShouldEnd && PlayInTriggered)
            {
                List<Card> traps = base.Battle.HandZone.Where((Card card) => card is TrapCard).ToList();
                if (!base.Battle.BattleShouldEnd && traps.FirstOrDefault() is TrapCard trap1) yield return new TriggerTrapAction(trap1);
                if (!base.Battle.BattleShouldEnd && traps.LastOrDefault() is TrapCard trap2) yield return new TriggerTrapAction(trap2);
            }
            yield break;
        }
    }
}
