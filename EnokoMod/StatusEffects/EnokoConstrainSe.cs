using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnokoMod.StatusEffects
{
    public sealed class EnokoConstrainSeDef : EnokoStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.Type = StatusEffectType.Negative;
            return config;
        }
    }

    [EntityLogic(typeof(EnokoConstrainSeDef))]
    public sealed class EnokoConstrainSe : StatusEffect
    {
        public string OwnerName
        {
            get
            {
                return this.Owner.Name;
            }
        }

        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<UnitEventArgs>(base.Owner.TurnStarted, new EventSequencedReactor<UnitEventArgs>(this.OnTurnStarted));
            base.ReactOwnerEvent<DamageEventArgs>(base.Owner.DamageReceived, new EventSequencedReactor<DamageEventArgs>(this.OnDamageRecieved));
        }

        private IEnumerable<BattleAction> OnTurnStarted(UnitEventArgs args)
        {
            yield return new RemoveStatusEffectAction(this);
            yield break;
        }

        private IEnumerable<BattleAction> OnDamageRecieved(DamageEventArgs args)
        {  
            if (args.DamageInfo.DamageType == DamageType.Attack)
            {
                yield return TriggerConstrain();
            }
            yield break;
        }

        public BattleAction TriggerConstrain()
        {
            this.NotifyActivating();
            return DamageAction.Reaction(this.Owner, this.Level);
        }

        protected override void OnRemoved(Unit unit)
        {
            if (unit.IsAlive)
            {
                base.React(DamageAction.LoseLife(unit, this.Level));
            }
        }
    }
}
