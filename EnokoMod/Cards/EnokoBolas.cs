﻿using EnokoMod.Cards.Templates;
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
using LBoL.Core.StatusEffects;

namespace EnokoMod.Cards
{
    public sealed class EnokoBolasDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Common;
            config.Type = CardType.Skill;
            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Black = 1 };
            config.Value1 = 5;
            config.Value2 = 1;
            config.RelativeEffects = new List<string>() { nameof(EnokoConstrainSe), nameof(Weak) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(EnokoConstrainSe), nameof(Weak) };
            config.TargetType = TargetType.AllEnemies;
            config.Keywords = Keyword.Exile;
            config.UpgradedKeywords = Keyword.Exile;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoBolasDef))]
    public sealed class EnokoBolas : Card
    {

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            foreach (BattleAction action in DebuffAction<EnokoConstrainSe>(Battle.AllAliveEnemies, level: Value1))
            {
                yield return action;
            }
            foreach (BattleAction action in DebuffAction<Weak>(Battle.AllAliveEnemies, duration: Value2))
            {
                yield return action;
            }
            if (this.IsUpgraded) yield return BuffAction<EnokoBolasSe>(level: Value1, count: Value2);
            yield break;
        }
    }
}
