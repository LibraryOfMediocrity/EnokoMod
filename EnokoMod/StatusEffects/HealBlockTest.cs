using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnokoMod.StatusEffects
{
    public sealed class HealBlockTestDef : EnokoStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.Type = StatusEffectType.Negative;
            return config;
        }
    }

    [EntityLogic(typeof(HealBlockTestDef))]
    public sealed class HealBlockTest : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.HandleOwnerEvent<HealEventArgs>(base.Owner.HealingReceiving, delegate (HealEventArgs args)
            {
                if (!base.Battle.BattleShouldEnd)
                {
                    int num = (int)Math.Min(args.Amount, Level);
                    args.Amount -= num;
                    Level -= num;
                    if (base.Level == 0)
                    {
                        this.React(new RemoveStatusEffectAction(this));
                    }
                }
            });
        }
    }
}
