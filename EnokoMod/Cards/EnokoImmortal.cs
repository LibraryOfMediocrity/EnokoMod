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
    public sealed class EnokoImmortalDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Ability;
            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Black = 2 };
            config.UpgradedCost = new ManaGroup() { Any = 1, Black = 1 };
            config.Value1 = 3;
            config.UpgradedValue1 = 5;
            config.RelativeKeyword = Keyword.Shield;
            config.UpgradedRelativeKeyword = Keyword.Shield;
            config.TargetType = TargetType.Nobody;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoImmortalDef))]
    public sealed class EnokoImmortal : Card
    {

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<EnokoImmortalSe>(Value1);
            yield break;
        }
    }

    public sealed class EnokoImmortalSeDef : EnokoStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.Keywords = Keyword.Shield;
            return config;
        }
    }

    [EntityLogic(typeof(EnokoImmortalSeDef))]
    public sealed class EnokoImmortalSe : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            foreach(Unit enemy in Battle.AllAliveEnemies)
            {
                base.ReactOwnerEvent<StatusEffectEventArgs>(enemy.StatusEffectRemoved, OnStatusEffectRemoved);
            }
            base.HandleOwnerEvent<UnitEventArgs>(Battle.EnemySpawned, delegate (UnitEventArgs args)
            {
                base.ReactOwnerEvent<StatusEffectEventArgs>(args.Unit.StatusEffectRemoved, OnStatusEffectRemoved);
            });
        }

        private IEnumerable<BattleAction> OnStatusEffectRemoved(StatusEffectEventArgs args)
        {
            if (base.Battle.BattleShouldEnd) yield break;
            if (args.Effect.Type == StatusEffectType.Negative) yield return new CastBlockShieldAction(Owner, 0, Level, cast: false);
            yield break;
        }
    }
}
