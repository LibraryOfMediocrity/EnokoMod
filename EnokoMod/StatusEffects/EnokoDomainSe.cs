using LBoL.ConfigData;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnokoMod.StatusEffects
{
    public sealed class EnokoDomainSeDef : EnokoStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.HasLevel = false;
            return base.MakeConfig();
        }
    }

    [EntityLogic(typeof(EnokoDomainSeDef))]
    public sealed class EnokoDomainSe : StatusEffect
    {
        public int MaxHand
        {
            get
            {
                return 12;
            }
        }

        protected override void OnAdded(Unit unit)
        {
            base.Battle.MaxHand = this.MaxHand;
        }
    }
}
