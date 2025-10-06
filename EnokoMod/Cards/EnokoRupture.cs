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
            config.Mana = new ManaGroup() { White = 1 };
            config.UpgradedMana = new ManaGroup() { Philosophy = 1 };
            config.Keywords = Keyword.Accuracy;
            config.UpgradedKeywords = Keyword.Accuracy;
            config.RelativeKeyword = Keyword.Exile | Keyword.Plentiful;
            config.UpgradedRelativeKeyword = Keyword.Exile | Keyword.Plentiful | Keyword.Philosophy; 
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

        public override ManaGroup? PlentifulMana => this.Mana;

        protected override string GetBaseDescription()
        {
            if (base.PlentifulHappenThisTurn) return base.GetExtraDescription1;
            return base.GetBaseDescription();
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return AttackAllAliveEnemyAction();
            yield break;
        }
    }
}
