using LBoL.ConfigData;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using UnityEngine;
using EnokoMod.Config;
using EnokoMod.ImageLoader;
using EnokoMod.Localization;

namespace EnokoMod.UltimateSkills
{
    public class EnokoUltTemplate : UltimateSkillTemplate
    {
        public override IdContainer GetId()
        {
            return EnokoDefaultConfig.GetDefaultID(this);
        }

        public override LocalizationOption LoadLocalization()
        {
            return EnokoLocalization.UltimateSkillsBatchLoc.AddEntity(this);
        }

        public override Sprite LoadSprite()
        {
            return EnokoImageLoader.LoadUltLoader(ult: this);
        }

        public override UltimateSkillConfig MakeConfig()
        {
            throw new System.NotImplementedException();
        }

        public UltimateSkillConfig GetDefaulUltConfig()
        {
            return EnokoDefaultConfig.GetDefaultUltConfig();
        }
    }
}
