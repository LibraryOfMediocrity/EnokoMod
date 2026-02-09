using EnokoMod.Cards.Templates;
using EnokoMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace EnokoMod.Cards
{
    public sealed class EnokoSightDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Defense;
            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Any = 1, Black = 1 };
            config.UpgradedCost = new ManaGroup() { Any = 2 };
            config.Block = 12;
            config.UpgradedBlock = 15;
            config.Value1 = 2;
            config.RelativeEffects = new List<string>() { nameof(LockedOn), nameof(EnokoConstrainSe) };
            config.UpgradedRelativeEffects = config.RelativeEffects;
            config.RelativeKeyword = Keyword.Block;
            config.UpgradedRelativeKeyword = Keyword.Block;
            config.TargetType = TargetType.All;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoSightDef))]
    public sealed class EnokoSight : Card
    {

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return DefenseAction();
            foreach (Unit unit in base.Battle.AllAliveEnemies)
            {
                if (unit.TryGetStatusEffect(out EnokoConstrainSe effect))
                {
                    yield return DebuffAction<LockedOn>(unit, level: Value1 + effect.Level);
                    yield return new RemoveStatusEffectAction(effect);
                }
                else
                {
                    yield return DebuffAction<LockedOn>(unit, level: Value1);
                }
            }
            yield break;
        }
    }
}
