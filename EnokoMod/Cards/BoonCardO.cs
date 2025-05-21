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
using LBoL.EntityLib.Cards;
using LBoL.Core.StatusEffects;
using LBoL.EntityLib.StatusEffects.Enemy;

namespace EnokoMod.Cards
{
    public sealed class BoonCardODef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.HideMesuem = true;
            config.IsPooled = false;
            config.FindInBattle = false;
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Skill;
            config.Colors = new List<ManaColor>() { ManaColor.White };
            config.Cost = new ManaGroup() { Any = 0 };
            config.Value1 = 7;
            config.UpgradedValue1 = 9;
            config.TargetType = TargetType.Nobody;
            config.RelativeEffects = new List<string>() { "DroneBlock" };
            config.UpgradedRelativeEffects = new List<string>() { "DroneBlock" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(BoonCardODef))]
    public sealed class BoonCardO : OptionCard
    {
        public override IEnumerable<BattleAction> TakeEffectActions()
        {
            yield return base.BuffAction<DroneBlock>(base.Value1, 0, 0, 0, 0.2f);
            yield break;
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return base.BuffAction<DroneBlock>(base.Value1, 0, 0, 0, 0.2f);
            yield break;
        }
    }
}
