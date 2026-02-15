using EnokoMod.Cards.Templates;
using EnokoMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.Cards.Neutral.TwoColor;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace EnokoMod.Cards
{
    public sealed class EnokoReinforceDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Rare;
            config.Type = CardType.Ability;
            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Any = 2, Black = 3 };
            config.UpgradedCost = new ManaGroup() { Any = 1, Black = 2 };
            config.Value1 = 4;
            config.RelativeEffects = new List<string>() { nameof(EnokoConstrainSe) };
            config.UpgradedRelativeEffects = config.RelativeEffects;
            config.TargetType = TargetType.Nobody;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoReinforceDef))]
    public sealed class EnokoReinforce : Card
    {

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<EnokoReinforceSe>();
            foreach (BattleAction action in DebuffAction<EnokoConstrainSe>(Battle.AllAliveEnemies, level: Value1))
            {
                yield return action;
            }
            yield break;
        }
    }

    public sealed class EnokoReinforceSeDef : EnokoStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.HasLevel = false;
            config.RelativeEffects = new List<string>() { nameof(EnokoConstrainSe) };
            return config;
        }
    }

    [EntityLogic(typeof(EnokoReinforceSeDef))]
    public sealed class EnokoReinforceSe : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            foreach (Unit enemy in Battle.AllAliveEnemies)
            {
                base.ReactOwnerEvent<StatusEffectApplyEventArgs>(enemy.StatusEffectAdding, OnStatusEffectAdding);
            }
            base.HandleOwnerEvent<UnitEventArgs>(Battle.EnemySpawned, delegate (UnitEventArgs args)
            {
                base.ReactOwnerEvent<StatusEffectApplyEventArgs>(args.Unit.StatusEffectAdding, OnStatusEffectAdding);
            });
        }

        private IEnumerable<BattleAction> OnStatusEffectAdding(StatusEffectApplyEventArgs args)
        {
            if (args.Effect is EnokoConstrainSe constrain)
            {
                this.NotifyActivating();
                constrain.Level *= 2;
            }
            yield break;
        }
    }
}
