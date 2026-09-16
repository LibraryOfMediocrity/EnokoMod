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
    public sealed class EnokoHellPowerDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Rare;
            config.Type = CardType.Attack;
            config.Colors = new List<ManaColor>() { ManaColor.Green };
            config.Cost = new ManaGroup() { Any = 1, Green = 1 };
            config.UpgradedCost = new ManaGroup() { Any = 2 };
            config.Damage = 10;
            config.UpgradedDamage = 14;
            config.Mana = new ManaGroup() { Green = 1 };
            config.TargetType = TargetType.SingleEnemy;
            config.RelativeEffects = new List<string>() { nameof(Burial) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(Burial) };
            config.Keywords = Keyword.Echo | Keyword.Plentiful;
            config.UpgradedKeywords = config.Keywords;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoHellPowerDef))]
    public sealed class EnokoHellPower : BurialCard
    {
        public override int CardIndex => 8;

        protected override void OnEnterBattle(BattleController battle)
        {
            base.ReactBattleEvent<UnitEventArgs>(Battle.Player.TurnStarted, OnTurnStarted);
        }

        private IEnumerable<BattleAction> OnTurnStarted(UnitEventArgs args)
        {
            if (IsBuried) yield return BuffAction<TempFirepower>(level: Battle.ExileZone.Count);
            yield break;
        }

        public override ManaGroup? PlentifulMana => this.Mana;

        protected override string GetBaseDescription()
        {
            if (base.PlentifulHappenThisTurn) return base.GetExtraDescription1;
            return base.GetBaseDescription();
        }

    }
}
