using EnokoMod.Cards.Templates;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoL.EntityLib.StatusEffects.Others;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace EnokoMod.Cards
{
    public sealed class EnokoMaladyDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Common;
            config.Type = CardType.Attack;
            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Any = 1, Black = 1 };
            config.Damage = 7;
            config.Value1 = 1;
            config.Value2 = 3;
            config.UpgradedValue2 = 5;
            config.RelativeEffects = new List<string>() { nameof(Poison) };
            config.UpgradedRelativeEffects = config.RelativeEffects;
            config.TargetType = TargetType.RandomEnemy;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoMaladyDef))]
    public sealed class EnokoMalady : Card
    {

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            for(int i = 0; i < Value2; i++)
            {
                if (base.Battle.BattleShouldEnd)
                    yield break;
                Unit unit = selector.GetEnemy(Battle);
                if(unit.IsAlive)
                    yield return AttackAction(unit);
                if(unit.IsAlive)
                    yield return DebuffAction<Poison>(unit, level: Value1);
            }
            yield break;
        }
    }
}
