using LBoL.ConfigData;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using UnityEngine;
using EnokoMod.ImageLoader;
using EnokoMod.Localization;
using EnokoMod.Config;

namespace EnokoMod.StatusEffects
{
    public class EnokoStatusEffectTemplate : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return EnokoDefaultConfig.GetDefaultID(this);
        }

        public override LocalizationOption LoadLocalization()
        {
            return EnokoLocalization.StatusEffectsBatchLoc.AddEntity(this);
        }

        public override Sprite LoadSprite()
        {
            return EnokoImageLoader.LoadStatusEffectLoader(status: this);
        }

        public override StatusEffectConfig MakeConfig()
        {
            return GetDefaultStatusEffectConfig();
        }

        public static StatusEffectConfig GetDefaultStatusEffectConfig()
        {
            return EnokoDefaultConfig.GetDefaultStatusEffectConfig();
        }
    }
}
