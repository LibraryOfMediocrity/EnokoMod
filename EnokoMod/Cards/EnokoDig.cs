using EnokoMod.Cards.Templates;
using EnokoMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace EnokoMod.Cards
{
    public sealed class EnokoDigDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Attack;
            config.Damage = 5;
            config.Value1 = 2;
            config.Value2 = 1;
            config.UpgradedValue2 = 2;
            config.Colors = new List<ManaColor>() { ManaColor.White };
            config.Cost = new ManaGroup() { Any = 1 };
            config.TargetType = TargetType.SingleEnemy;
            config.RelativeEffects = new List<string>() { nameof(Firepower), nameof(Burial) };
            config.UpgradedRelativeEffects = config.RelativeEffects;
            config.RelativeCards = new List<string>() { nameof(EnokoTreasure) };
            config.UpgradedRelativeCards = new List<string>() { nameof(EnokoTreasure) + "+" };
            config.Keywords = Keyword.Exile|Keyword.Debut;
            config.UpgradedKeywords = config.Keywords;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoDigDef))]
    public sealed class EnokoDig : BurialCard
    {
        public override int CardIndex => 2;

        protected override void OnEnterBattle(BattleController battle)
        {
            //base.ReactBattleEvent<CardMovingEventArgs>(Battle.CardMoved, OnCardMoved);
            //base.ReactBattleEvent<CardMovingToDrawZoneEventArgs>(Battle.CardMovedToDrawZone, OnCardMovedDraw);
            base.ReactBattleEvent<UnitEventArgs>(Battle.Player.TurnStarted, OnTurnStarted);
        }

        private IEnumerable<BattleAction> OnTurnStarted(UnitEventArgs args)
        {
            if (IsBuried)
            {
                yield return new MoveCardAction(this, CardZone.Hand);
            }
            yield break;
        }

        /*
        private IEnumerable<BattleAction> OnCardMoved(CardMovingEventArgs args)
        {
            if (args.SourceZone == CardZone.Exile)
            {
                if (this.Zone == CardZone.Hand) NotifyActivating();
                else yield return PerformAction.ViewCard(this);
                yield return BuffAction<Firepower>(Value2);
            }
            yield break;
        }

        private IEnumerable<BattleAction> OnCardMovedDraw(CardMovingToDrawZoneEventArgs args)
        {
            if (args.SourceZone == CardZone.Exile)
            {
                if (this.Zone == CardZone.Hand) NotifyActivating();
                else yield return PerformAction.ViewCard(this);
                yield return BuffAction<Firepower>(Value2);
            }
            yield break;
        }
        */

        protected override string GetBaseDescription()
        {
            if(DebutActive) return base.GetBaseDescription();
            return base.GetExtraDescription1;
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return AttackAction(selector);

            if (TriggeredAnyhow)
            {
                Card card = Library.CreateCard<EnokoTreasure>(this.IsUpgraded);
                yield return new PlayCardAction(card);
            }
            yield break;
        }
    }

    public sealed class EnokoTreasureDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.HideMesuem = true;
            config.IsPooled = false;
            config.FindInBattle = false;
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Ability;
            config.Colors = new List<ManaColor>() { ManaColor.White };
            config.Cost = new ManaGroup() { Any = 0 };
            config.Value1 = 1;
            config.UpgradedValue1 = 2;
            config.TargetType = TargetType.Nobody;
            config.RelativeEffects = new List<string>() { nameof(Firepower) };
            config.UpgradedRelativeEffects = config.RelativeEffects;
            config.RelativeKeyword = Keyword.Exile;
            config.UpgradedRelativeKeyword = Keyword.Exile;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoTreasureDef))]
    public sealed class EnokoTreasure : Card
    {

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<EnokoTreasureSe>(Value1);
            yield break;
        }
    }

    public sealed class EnokoTreasureSeDef : EnokoStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.RelativeEffects = new List<string>() { nameof(Firepower) };
            config.Keywords = Keyword.Exile;
            return config;
        }
    }

    [EntityLogic(typeof(EnokoTreasureSeDef))]
    public sealed class EnokoTreasureSe : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<CardMovingEventArgs>(Battle.CardMoved, OnCardMoved);
            base.ReactOwnerEvent<CardMovingToDrawZoneEventArgs>(Battle.CardMovedToDrawZone, OnCardMovedDraw);
        }

        private IEnumerable<BattleAction> OnCardMoved(CardMovingEventArgs args)
        {
            if (args.SourceZone == CardZone.Exile)
            {
                NotifyActivating();
                yield return BuffAction<Firepower>(Level);
            }
            yield break;
        }

        private IEnumerable<BattleAction> OnCardMovedDraw(CardMovingToDrawZoneEventArgs args)
        {
            if (args.SourceZone == CardZone.Exile)
            {
                NotifyActivating();
                yield return BuffAction<Firepower>(Level);
            }
            yield break;
        }
    }
}
