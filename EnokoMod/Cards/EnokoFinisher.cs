using EnokoMod.Cards.Templates;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnokoMod.Cards
{
    public sealed class EnokoFinisherDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Rare;
            config.Type = CardType.Attack;
            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Any = 1, Black = 1 };
            config.UpgradedCost = new ManaGroup() { Any = 2 };
            config.Damage = 12;
            config.UpgradedDamage = 15;
            config.TargetType = TargetType.SingleEnemy;
            config.Keywords = Keyword.Accuracy;
            config.UpgradedKeywords = Keyword.Accuracy;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoFinisherDef))]
    public sealed class EnokoFinisher : Card
    {

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            int DebuffCount = selector.SelectedEnemy.StatusEffects.Where((StatusEffect e) => e.Type == StatusEffectType.Negative).Count();
            for (int i = 0; i <= DebuffCount; i++)
            {
                yield return AttackAction(selector);
            }
            yield break;
        }
    }
}
