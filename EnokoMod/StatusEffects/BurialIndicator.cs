using EnokoMod.Cards.Templates;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace EnokoMod.StatusEffects
{
    public sealed class BurialIndicatorDef : EnokoStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.Type = StatusEffectType.Special;
            config.IsStackable = false;
            config.RelativeEffects = new List<string>() { nameof(Burial) };
            return config;
        }
    }

    [EntityLogic(typeof(BurialIndicatorDef))]
    public sealed class BurialIndicator : StatusEffect
    {
       
        public override string Name
        {
            get
            {
                if(SourceCard is BurialCard)
                {
                    return SourceCardName;
                }
                else
                {
                    return base.Name; // should never return this
                }
            }
        }

        protected override void OnAdded(Unit unit)
        {
            //_showCount = false;
        }

        public string BurialEffect
        {
            get
            {
                return SourceCard is BurialCard burial ? burial.BurialDescription : "ERROR";
            }
        }
    }
}
