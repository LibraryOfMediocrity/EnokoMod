using EnokoMod.Cards.Templates;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using LBoL.Base.Extensions;

namespace EnokoMod.Cards
{
    public sealed class EnokoCatalystDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Type = CardType.Skill;
            config.Rarity = Rarity.Rare;
            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.TargetType = TargetType.SingleEnemy;
            config.Cost = new ManaGroup() { Any = 2, Black = 1 };
            config.Keywords = Keyword.Exile;
            config.UpgradedKeywords = Keyword.Exile | Keyword.Retain;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoCatalystDef))]
    public sealed class EnokoCatalyst : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            var statusEffect = selector.SelectedEnemy.StatusEffects;
            foreach (StatusEffect status in statusEffect)
            {
                if (status != null && status.Type == StatusEffectType.Negative)
                {
                    Type type = status.GetType();
                    int level = 0, duration = 0;
                    if (status.HasLevel) level = status.Level;
                    if (status.HasDuration) duration = status.Duration;
                    //debuffaction returns enumerable for multible targets
                    foreach (BattleAction action in DebuffAction(type, base.Battle.AllAliveEnemies, level, duration, status.Limit))
                    {
                        yield return action;
                    }
                }
            }
            yield break;
        }
    }
}
