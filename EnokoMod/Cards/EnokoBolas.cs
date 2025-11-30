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
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;

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

    ﻿using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.Exhibits;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnokoMod.StatusEffects
    {
        public sealed class EnokoBolasSeDef : EnokoStatusEffectTemplate
        {
            public override StatusEffectConfig MakeConfig()
            {
                StatusEffectConfig config = GetDefaultStatusEffectConfig();
                config.RelativeEffects = new List<string>() { nameof(EnokoConstrainSe), nameof(Weak) };
                config.HasCount = true;
                config.CountStackType = StackType.Add;
                return config;
            }

        }

        [EntityLogic(typeof(EnokoBolasSeDef))]
        public sealed class EnokoBolasSe : StatusEffect
        {
            protected override void OnAdded(Unit unit)
            {
                base.ReactOwnerEvent<UnitEventArgs>(Battle.Player.TurnStarted, new EventSequencedReactor<UnitEventArgs>(this.OnTurnStarted));
            }

            private IEnumerable<BattleAction> OnTurnStarted(UnitEventArgs args)
            {
                this.NotifyActivating();
                foreach (BattleAction action in DebuffAction<EnokoConstrainSe>(Battle.AllAliveEnemies, level: this.Level))
                {
                    yield return action;
                }
                foreach (BattleAction action in DebuffAction<Weak>(Battle.AllAliveEnemies, duration: this.Count))
                {
                    yield return action;
                }
                yield return new RemoveStatusEffectAction(this);
                yield break;
            }
        }
    }
}