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

namespace EnokoMod.Cards
{
    public sealed class EnokoRuptureDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Attack;
            config.Colors = new List<ManaColor>() { ManaColor.White };
            config.Cost = new ManaGroup() { Any = 1, White = 2 };
            config.Damage = 18;
            config.UpgradedDamage = 24;
            config.TargetType = TargetType.AllEnemies;
            config.UpgradedKeywords = Keyword.Accuracy;
            config.RelativeKeyword = Keyword.Exile;
            config.UpgradedRelativeKeyword = Keyword.Exile;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoRuptureDef))]
    public sealed class EnokoRupture : Card
    {
        public override IEnumerable<BattleAction> OnExile(CardZone srcZone)
        {
            yield return AttackAllAliveEnemyAction();
            yield return new MoveCardAction(this, CardZone.Hand);
            yield break;
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return AttackAllAliveEnemyAction();
            yield break;
        }
    }
}
