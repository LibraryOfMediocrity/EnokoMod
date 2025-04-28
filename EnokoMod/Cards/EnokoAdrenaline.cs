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
    public sealed class EnokoAdrenalineDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Common;
            config.Type = CardType.Skill;
            config.Colors = new List<ManaColor>() { ManaColor.White };
            config.Cost = new ManaGroup() { White = 1 };
            config.TargetType = TargetType.Self;
            config.Value1 = 1;
            config.UpgradedValue1 = 2;
            config.Value2 = 1;
            config.Mana = new ManaGroup() { Colorless = 1 };
            config.RelativeEffects = new List<string>() { "Graze" };
            config.UpgradedRelativeEffects = new List<string>() { "Graze" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoAdrenalineDef))]
    public sealed class EnokoAdrenaline : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<Graze>(base.Value1);
            yield return new DrawCardAction();
            int count = base.Battle.HandZone.Count - 5;
            if (count < 0) count = 0;
            ManaGroup manaGroup = new ManaGroup() { Colorless = count };
            yield return new GainManaAction(manaGroup);
            yield break;
        }
    }
}
