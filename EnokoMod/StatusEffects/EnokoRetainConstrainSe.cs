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
    public sealed class EnokoRetainConstrainSeDef : EnokoStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.Type = StatusEffectType.Special;
            config.RelativeEffects = new List<string>() { nameof(EnokoConstrainSe) };
            return config;
        }
    }

    [EntityLogic(typeof(EnokoRetainConstrainSeDef))]
    public sealed class EnokoRetainConstrainSe : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<StatusEffectEventArgs>(unit.StatusEffectRemoving, new EventSequencedReactor<StatusEffectEventArgs>(this.OnStatusEffectRemoving));
        }

        private IEnumerable<BattleAction> OnStatusEffectRemoving(StatusEffectEventArgs args)
        {
            if(args.Effect is EnokoConstrainSe)
            {
                this.NotifyActivating();
                args.ForceCancelBecause(CancelCause.Reaction);
                base.Level--;
                if (base.Level <= 0)
                {
                    yield return new RemoveStatusEffectAction(this);
                }
            }
            yield break;
        }
    }
}
