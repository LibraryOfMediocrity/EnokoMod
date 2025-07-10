using EnokoMod.Cards.Templates;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using LBoL.Core.Units;
using UnityEngine;
using EnokoMod.TrapToolBox;
using EnokoMod.BattleActions;
using LBoL.Core.Battle.BattleActions;
using UnityEngine.Rendering;
using LBoL.EntityLib.StatusEffects.Reimu;

namespace EnokoMod.Cards
{
    public sealed class EnokoSenseWeakTrapDef : TrapTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetTrapDefaultConfig();
            config.IsPooled = true;
            config.Cost = new ManaGroup() { Any = 1 };
            config.Damage = 5;
            config.UpgradedDamage = 7;
            config.Value1 = 2;
            config.UpgradedValue1 = 3;
            config.Rarity = Rarity.Uncommon;
            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoSenseWeakTrapDef))]
    public sealed class EnokoSenseWeakTrap : TrapCard
    {
        protected override void OnEnterBattle(BattleController battle)
        {
            foreach (Unit unit in battle.AllAliveEnemies)
            {
                base.ReactBattleEvent<StatusEffectEventArgs>(unit.StatusEffectRemoved, new EventSequencedReactor<StatusEffectEventArgs>(this.OnStatusEffectRemoved));
            }
            base.HandleBattleEvent<UnitEventArgs>(Battle.EnemySpawned, new GameEventHandler<UnitEventArgs>(delegate (UnitEventArgs args)
            {
                base.ReactBattleEvent<StatusEffectEventArgs>(args.Unit.StatusEffectRemoved, new EventSequencedReactor<StatusEffectEventArgs>(this.OnStatusEffectRemoved));
            }));
        }

        private IEnumerable<BattleAction> OnStatusEffectRemoved(StatusEffectEventArgs args)
        {
            if (base.Battle.BattleShouldEnd || !args.Unit.IsAlive) yield break;
            if(args.Effect.Type == StatusEffectType.Negative) yield return new TriggerTrapAction(this, args.Unit);
            yield break;
        }

        public override IEnumerable<BattleAction> TrapTriggered(Unit[] selector)
        {
            yield return AttackAction(selector, this.GunName, base.Damage);
            DeltaDamage += Value1;
            yield break;
        }

    }
}
