using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.StatusEffects.Enemy;
using LBoLEntitySideloader.Attributes;
using System.Collections;
using System.Collections.Generic;

namespace EnokoMod.StatusEffects
{
    public sealed class YuyukoCounterDef : EnokoStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.Type = StatusEffectType.Special;
            config.Order = 1;
            config.HasLevel = false;
            return config;
        }
    }

    [EntityLogic(typeof(YuyukoCounterDef))]
    public sealed class YuyukoCounter : StatusEffect
    {
        private StatusEffect effect = null;

        protected override void OnAdded(Unit unit)
        {
            base.HandleOwnerEvent<HealEventArgs>(base.Owner.HealingReceiving, delegate(HealEventArgs args) 
            {
                args.CanCancel = false; // this causes error because cancelling has no safety checks so just create custom law of mortality and apply in exhibit
            });
            base.HandleOwnerEvent<StatusEffectApplyEventArgs>(base.Owner.StatusEffectAdding, delegate (StatusEffectApplyEventArgs args)
            {
                if (args.Effect is YuyukoDeath)
                { 
                    args.Effect.Level *= 2;
                    effect ??= args.Effect;
                }
            });
            base.HandleOwnerEvent<UnitEventArgs>(base.Owner.TurnStarted, delegate (UnitEventArgs args)
            {
                if (effect != null) base.React(new ApplyStatusEffectAction<HealBlockTest>(Owner, level: effect.Level));
            });
        }
    }
}
