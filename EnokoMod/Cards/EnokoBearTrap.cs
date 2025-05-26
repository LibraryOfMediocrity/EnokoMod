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

namespace EnokoMod.Cards
{
    public sealed class EnokoBearTrapDef : TrapTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetTrapDefaultConfig();
            config.Cost = new ManaGroup() { Any = 1 };
            config.Damage = 8;
            config.UpgradedDamage = 12;
            config.Rarity = Rarity.Common;
            config.Index = CardIndexGenerator.GetUniqueIndex(config, true);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoBearTrapDef))]
    public sealed class EnokoBearTrap : TrapCard
    {
        protected override void OnEnterBattle(BattleController battle)
        {
            base.ReactBattleEvent<DamageEventArgs>(base.Battle.Player.DamageReceived, new EventSequencedReactor<DamageEventArgs>(this.OnDamageRecieving));
        }

        private IEnumerable<BattleAction> OnDamageRecieving(DamageEventArgs args)
        {
            if (args.Source != base.Battle.Player && args.Source.IsAlive && args.DamageInfo.DamageType == DamageType.Attack && args.DamageInfo.Amount > 0f)
            {
                yield return new TriggerTrapAction(this, args.Source);
                /*
                var BattleActions = this.TrapTriggered(TrapTools.SelectUnit(args.Source));
                foreach (var action in BattleActions)
                {
                    yield return action;
                }
                */
                yield break;
            }
            yield break;
        }

        public override IEnumerable<BattleAction> TrapTriggered(Unit[] selector)
        {
            yield return AttackAction(selector, this.GunName, base.Damage);
            yield break;
        }

    }
}
