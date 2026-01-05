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
    public sealed class EnokoThornsSeDef : EnokoStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.Type = StatusEffectType.Negative;
            return config;
        }

        [EntityLogic(typeof(EnokoThornsSeDef))]
        public sealed class EnokoThornsSe : StatusEffect
        {


            protected override void OnAdded(Unit unit)
            {
                base.HandleOwnerEvent<UnitEventArgs>(base.Owner.TurnEnding, new GameEventHandler<UnitEventArgs>(delegate
                {
                    int num = base.Level / 2;
                    base.Level = num;
                    if (base.Level == 0)
                    {
                        this.React(new RemoveStatusEffectAction(this, true, 0.1f));
                    }
                }));
                base.HandleOwnerEvent<DamageEventArgs>(unit.DamageReceiving, new GameEventHandler<DamageEventArgs>(delegate(DamageEventArgs args) 
                {
                    DamageInfo damageInfo = args.DamageInfo;
                    if (damageInfo.DamageType == DamageType.Attack)
                    {
                        args.DamageInfo = damageInfo.IncreaseBy(base.Level);
                        args.AddModifier(this);
                    }
                }));
                base.HandleOwnerEvent<DamageEventArgs>(base.Owner.DamageReceived, new GameEventHandler<DamageEventArgs>(delegate
                {
                    int num = base.Level - 1;
                    base.Level = num;
                    if (base.Level == 0)
                    {
                        this.React(new RemoveStatusEffectAction(this, true, 0.1f));
                    }
                }));
            }
        }
    }
}
