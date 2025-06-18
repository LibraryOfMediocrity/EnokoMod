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
using LBoL.Core.StatusEffects;

namespace EnokoMod.Cards
{
    public sealed class EnokoBlindDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Common;
            config.Type = CardType.Skill;
            config.Colors = new List<ManaColor>() { ManaColor.White, ManaColor.Black };
            config.Cost = new ManaGroup() { Any = 1, Hybrid = 1, HybridColor = 1 };
            config.Value1 = 2;
            config.Value2 = 2;
            config.UpgradedValue2 = 4;
            config.TargetType = TargetType.AllEnemies;
            config.RelativeEffects = new List<string>() { nameof(TempFirepowerNegative) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(TempFirepowerNegative) };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoBlindDef))]
    public sealed class EnokoBlind : Card
    {

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            foreach (BattleAction action in DebuffAction<TempFirepowerNegative>(Battle.AllAliveEnemies, Value2))
            {
                yield return action;
            }
            yield return new DrawManyCardAction(Value1);
            yield break;
        }
    }
}
