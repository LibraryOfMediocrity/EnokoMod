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

namespace EnokoMod.Cards
{
    public sealed class EnokoCaltropsDef : TrapTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetTrapDefaultConfig();
            config.Cost = new ManaGroup() { Any = 1 };
            config.Damage = 8;
            config.UpgradedDamage = 10;
            config.Rarity = Rarity.Common;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoCaltropsDef))]
    public sealed class EnokoCaltrops : TrapCard
    {
        protected override void OnEnterBattle(BattleController battle)
        {
            base.ReactBattleEvent<DamageEventArgs>(base.Battle.Player.DamageReceived, new EventSequencedReactor<DamageEventArgs>(this.OnDamageRecieving));
        }

        private IEnumerable<BattleAction> OnDamageRecieving(DamageEventArgs args)
        {
            if (args.Source != base.Battle.Player && args.Source.IsAlive && args.DamageInfo.DamageType == DamageType.Attack && args.DamageInfo.Amount > 0f)
            {
                base.NotifyActivating();
                var BattleActions = this.TrapTriggered(GetUnits(args.Source));
                foreach (var action in BattleActions)
                {
                    yield return action;
                }
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
