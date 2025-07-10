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
using EnokoMod.StatusEffects;
using LBoL.Core.Units;
using System.Linq;

namespace EnokoMod.Cards
{
    public sealed class EnokoConstrainTriggerDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Defense;
            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Any = 1, Black = 1 };
            config.Block = 14;
            config.UpgradedBlock = 18;
            config.Value1 = 4;
            config.UpgradedValue1 = 6;
            config.TargetType = TargetType.SingleEnemy;
            config.RelativeKeyword = Keyword.Block;
            config.UpgradedRelativeKeyword = Keyword.Block;
            config.RelativeEffects = new List<string>() { nameof(EnokoConstrainSe) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(EnokoConstrainSe) };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoConstrainTriggerDef))]
    public sealed class EnokoConstrainTrigger : Card
    {

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return DefenseAction();
            yield return DebuffAction<EnokoConstrainSe>(selector.SelectedEnemy, level: Value1);
            foreach (Unit enemy in Battle.AllAliveEnemies.Where((EnemyUnit unit) => unit.HasStatusEffect<EnokoConstrainSe>()))
            {
                enemy.TryGetStatusEffect<EnokoConstrainSe>(out EnokoConstrainSe statusEffect);
                yield return statusEffect.TriggerConstrain(); 
            }
            yield break;
        }
    }
}
