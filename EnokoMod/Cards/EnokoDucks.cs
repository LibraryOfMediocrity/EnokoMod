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
using System.ComponentModel.Design;

namespace EnokoMod.Cards
{
    public sealed class EnokoDucksDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Rare;
            config.Type = CardType.Ability;
            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Any = 1, Red = 1 };
            config.UpgradedCost = new ManaGroup() { Any = 1 };
            config.TargetType = TargetType.Nobody;
            config.RelativeEffects = new List<string>() { nameof(EnokoConstrainSe), nameof(LockedOn) };
            config.UpgradedRelativeEffects = config.RelativeEffects;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoDucksDef))]
    public sealed class EnokoDucks : Card
    {

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<EnokoDucksSe>();
            yield break;
        }
    }

    public sealed class EnokoDucksSeDef : EnokoStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.HasLevel = false;
            config.Order = 12;
            config.RelativeEffects = new List<string>() { nameof(EnokoConstrainSe), nameof(LockedOn) };
            return config;
        }
    }

    [EntityLogic(typeof(EnokoDucksSeDef))]
    public sealed class EnokoDucksSe : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            foreach (Unit enemy in Battle.AllAliveEnemies)
            {
                base.ReactOwnerEvent<StatusEffectApplyEventArgs>(enemy.StatusEffectAdding, OnStatusEffectAdding);
                base.HandleOwnerEvent<StatusEffectEventArgs>(enemy.StatusEffectRemoving, delegate (StatusEffectEventArgs args)
                {
                    if (args.Effect is LockedOn && args.ActionSource != args.Effect)
                    {
                        this.NotifyActivating();
                        args.CancelBy(this);
                    }
                });
            }
            base.HandleOwnerEvent<UnitEventArgs>(Battle.EnemySpawned, delegate (UnitEventArgs args)
            {
                base.ReactOwnerEvent<StatusEffectApplyEventArgs>(args.Unit.StatusEffectAdding, OnStatusEffectAdding);
            });
        }

        private IEnumerable<BattleAction> OnStatusEffectAdding(StatusEffectApplyEventArgs args)
        {
            if(args.Effect is EnokoConstrainSe se)
            {
                this.NotifyActivating();
                int level = se.Level;
                args.CancelBy(this);
                yield return new ApplyStatusEffectAction<LockedOn>(args.Unit, level: level);
            }
            yield break;
        }
    }
}
