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
    public sealed class EnokoRecycleDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Rare;
            config.Type = CardType.Ability;
            config.Colors = new List<ManaColor>() { ManaColor.White, ManaColor.Black };
            config.Cost = new ManaGroup() { White = 1, Black = 1 };
            config.Value1 = 4;
            config.UpgradedValue1 = 6;
            config.RelativeKeyword = Keyword.Exile;
            config.UpgradedRelativeKeyword = Keyword.Exile;
            config.RelativeEffects = new List<string>() { nameof(EnokoConstrainSe) };
            config.UpgradedRelativeEffects = config.RelativeEffects;
            config.TargetType = TargetType.Nobody;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoRecycleDef))]
    public sealed class EnokoRecycle : Card
    {

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<EnokoRecycleSe>(Value1);
            yield break;
        }
    }

    public sealed class EnokoRecycleSeDef : EnokoStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.RelativeEffects = new List<string>() { nameof(EnokoConstrainSe) };
            config.Keywords = Keyword.Exile;
            return config;
        }
    }

    [EntityLogic(typeof(EnokoRecycleSeDef))]
    public sealed class EnokoRecycleSe : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<CardEventArgs>(Battle.CardExiled, OnCardExiled);
        }

        private IEnumerable<BattleAction> OnCardExiled(CardEventArgs args)
        {
            if(args.Card.CardType == CardType.Attack)
            {
                foreach (EnemyUnit enemy in Battle.AllAliveEnemies)
                {
                    yield return new ApplyStatusEffectAction<EnokoConstrainSe>(enemy, level: Level);
                }
            }
            yield break;
        }
    }
}
