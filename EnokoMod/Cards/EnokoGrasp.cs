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

namespace EnokoMod.Cards
{
    public sealed class EnokoGraspDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Skill;
            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Any = 1, Black = 1 };
            config.UpgradedCost = new ManaGroup() { Any = 1 };
            config.Value1 = 4;
            config.Keywords = Keyword.Debut;
            config.UpgradedKeywords = Keyword.Debut;
            config.TargetType = TargetType.AllEnemies;
            config.RelativeEffects = new List<string>() { nameof(EnokoConstrainSe) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(EnokoConstrainSe) };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoGraspDef))]
    public sealed class EnokoGrasp : Card
    {
        private new string ExtraDescription1
        {
            get
            {
                return this.LocalizeProperty("ExtraDescription1", true, true);
            }
        }

        protected override string GetBaseDescription()
        {
            if (!base.DebutActive)
            {
                return this.ExtraDescription1;
            }
            return base.GetBaseDescription();
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            foreach (BattleAction action in DebuffAction<EnokoConstrainSe>(Battle.AllAliveEnemies, level: Value1)) 
            {
                yield return action;
            }
            foreach (BattleAction action in DebuffAction<EnokoRetainConstrainSe>(Battle.AllAliveEnemies, level: (this.PlayInTriggered ? 2 : 1)))
            {
                yield return action;
            }
            yield break;
        }
    }
}
