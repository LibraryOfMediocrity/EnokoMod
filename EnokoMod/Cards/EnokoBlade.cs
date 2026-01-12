using EnokoMod.BattleActions;
using EnokoMod.Cards.Templates;
using EnokoMod.TrapToolBox;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace EnokoMod.Cards
{
    public sealed class EnokoBladeDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Attack;
            config.Colors = new List<ManaColor>() { ManaColor.White };
            config.Cost = new ManaGroup() { Any = 2, White = 1 };
            config.TargetType = TargetType.SingleEnemy;
            config.Damage = 16;
            config.UpgradedDamage = 20;
            config.Value1 = 4;
            config.UpgradedValue1 = 6;
            config.Value2 = 2;
            config.Keywords = Keyword.Accuracy;
            config.UpgradedKeywords = Keyword.Accuracy;
            config.RelativeCards = new List<string>() { nameof(EnokoSnareTrap) };
            config.UpgradedRelativeCards = config.RelativeCards;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoBladeDef))]
    public sealed class EnokoBlade : Card
    {

        protected override void OnEnterBattle(BattleController battle)
        {
            base.HandleBattleEvent<TriggerTrapEventArgs>(EnokoGameEvents.PostTriggerEvent, delegate
            {
                TriggerTrapTimes++;
            });
        }

        private int TriggerTrapTimes = 0;

        public override int AdditionalDamage => TriggerTrapTimes * Value1;

        public override bool Triggered => Battle.TurnCardUsageHistory.Count >= 2 && Battle.HandZone.OfType<EnokoSnareTrap>().Count() < 1;

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return AttackAction(selector);
            if (TriggeredAnyhow)
            {
                yield return new AddCardsToHandAction(Library.CreateCard<EnokoSnareTrap>());
            }
            yield break;
        }
    }

    public sealed class EnokoSnareTrapDef : TrapTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetTrapDefaultConfig();
            config.Damage = 8;
            config.UpgradedDamage = 12;
            config.Rarity = Rarity.Uncommon;
            config.Cost = new ManaGroup() { Any = 1 };
            config.Index = CardIndexGenerator.GetUniqueIndex(config, true);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoSnareTrapDef))]
    public sealed class EnokoSnareTrap : TrapCard
    {

        public int Counter {  get; set; }

        private bool skipFirst;

        protected override void OnEnterBattle(BattleController battle)
        {
            Counter = 3;
            skipFirst = true;
            base.ReactBattleEvent<CardUsingEventArgs>(base.Battle.CardUsed, OnCardUsed);
        }

        private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
        {
            if (skipFirst || base.Battle.BattleShouldEnd) 
            { 
                skipFirst = false;
                yield break;
            }
            Counter--;
            NotifyChanged();
            if (Counter == 0)
            {
                Counter = 3;
                NotifyChanged();
                yield return new TriggerTrapAction(this, TrapSelector.LeastLife);
            }
            yield break;
        }

        public override bool Triggered => Counter == 1;

        public override IEnumerable<BattleAction> TrapTriggered(Unit[] units)
        {
            yield return AttackAction(units);
            yield break;
        }

    }
}
