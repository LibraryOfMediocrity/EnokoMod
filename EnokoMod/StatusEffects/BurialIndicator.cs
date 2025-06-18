using EnokoMod.Cards;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnokoMod.StatusEffects
{
    public sealed class BurialIndicatorDef : EnokoStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.Type = StatusEffectType.Special;
            config.HasLevel = false;
            config.IsStackable = false;
            config.RelativeEffects = new List<string>() { nameof(Burial) };
            return config;
        }
    }

    [EntityLogic(typeof(BurialIndicatorDef))]
    public sealed class BurialIndicator : StatusEffect
    {
        //test if sourcecard is recorded. if it is, everything is easy.

        public override string Name
        {
            get
            {
                try
                {
                    return TypeFactory<Card>.LocalizeProperty(BurialCards[Limit].Name, "Name", false, true);
                }
                catch
                {
                    return base.Name;
                }
            }
        }

        private readonly List<Type> BurialCards = new List<Type>()
        {
            typeof(EnokoPitfall)
        }; 
    }
}
