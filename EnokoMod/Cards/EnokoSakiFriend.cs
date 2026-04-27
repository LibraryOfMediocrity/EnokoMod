using EnokoMod.Cards.Templates;
using EnokoMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace EnokoMod.Cards
{
    public sealed class EnokoSakiFriendDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Friend;
            config.Colors = new List<ManaColor>() { ManaColor.Green };
            config.Cost = new ManaGroup() { Any = 1, Green = 1 };
            config.Value1 = 2;
            config.UpgradedValue1 = 3;
            config.Damage = 10;
            config.Loyalty = 2;
            config.UpgradedLoyalty = 3;
            config.PassiveCost = 1;
            config.ActiveCost = -2;
            config.TargetType = TargetType.AllEnemies;
            config.RelativeKeyword = Keyword.Exile;
            config.UpgradedRelativeKeyword = Keyword.Exile;
            config.RelativeEffects = new List<string>() { nameof(Burial), nameof(TempFirepower) };
            config.UpgradedRelativeEffects = config.RelativeEffects;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoSakiFriendDef))]
    public sealed class EnokoSakiFriend : BurialCard
    {
        public override int CardIndex => 6;

        protected override void OnEnterBattle(BattleController battle)
        {
            base.ReactBattleEvent<UnitEventArgs>(Battle.Player.TurnStarting, OnTurnStarted);
        }

        private IEnumerable<BattleAction> OnTurnStarted(UnitEventArgs args)
        {
            if (IsBuried && !Battle.BattleShouldEnd) yield return new MoveCardAction(this, CardZone.Hand);
            yield break;
        }

        public override IEnumerable<BattleAction> OnTurnStartedInHand()
        {
            foreach (BattleAction action in this.GetPassiveActions())
            {
                yield return action;
            }
            yield break;
        }

        public override IEnumerable<BattleAction> GetPassiveActions()
        {
            //Trigger the effect only if the card has been summoned. 
            if (!base.Summoned || base.Battle.BattleShouldEnd)
            {
                yield break;
            }
            base.NotifyActivating();
            //Trigger the action multiple times if "Mental Energy Injection" is active.
            for (int i = 0; i < base.Battle.FriendPassiveTimes; i++)
            {
                if (base.Battle.BattleShouldEnd)
                {
                    yield break;
                }
                yield return BuffAction<TempFirepower>(base.Loyalty);
            }
            //Increase base loyalty.
            base.Loyalty += base.PassiveCost;
            yield return new SpecialResolveAction(this);
            yield break;
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            base.Loyalty += base.ActiveCost;
            base.CardGuns = new Guns(base.GunName, base.Value1, true);
            foreach (GunPair gunPair in base.CardGuns.GunPairs)
            {
                yield return base.AttackAction(selector, gunPair);
            }
            yield return new ExileCardAction(this);
            yield break;
        }
    }
}
