using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnokoMod.StatusEffects
{
    public sealed class BurialDef: EnokoStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.Keywords = Keyword.Exile;
            return config;
        }
    }

    [EntityLogic(typeof(BurialDef))]
    public sealed class Burial : StatusEffect
    {

    }
}
