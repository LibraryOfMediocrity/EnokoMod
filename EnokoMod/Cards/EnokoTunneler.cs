using Cysharp.Threading.Tasks;
using EnokoMod.Cards.Templates;
using EnokoMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace EnokoMod.Cards
{
    public sealed class EnokoTunnelerDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Attack;
            config.Colors = new List<ManaColor>() { ManaColor.Green };
            config.Cost = new ManaGroup() { Any = 1, Green = 1 };
            config.Damage = 15;
            config.UpgradedDamage = 20;
            config.TargetType = TargetType.SingleEnemy;
            config.Keywords = Keyword.Exile;
            config.UpgradedKeywords = Keyword.Exile|Keyword.Accuracy;
            config.RelativeEffects = new List<string>() { nameof(Burial) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(Burial) };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoTunnelerDef))]
    public sealed class EnokoTunneler : BurialCard
    {
        public override int CardIndex => 7;

        protected override void OnEnterBattle(BattleController battle)
        {
            base.ReactBattleEvent<UnitEventArgs>(Battle.Player.TurnStarted, OnTurnStarted);
        }

        private IEnumerable<BattleAction> OnTurnStarted(UnitEventArgs args)
        {
            if (IsBuried) yield return new PlayCardAction(this);
            yield break;
        }
    }
}
