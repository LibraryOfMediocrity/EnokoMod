using System;
using LBoL.ConfigData;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using EnokoMod.Config;
using EnokoMod.ImageLoader;
using EnokoMod.Localization;

namespace EnokoMod.Exhibits
{
    public class EnokoExhibitTemplate : ExhibitTemplate
    {
        public override IdContainer GetId()
        {
            return EnokoDefaultConfig.GetDefaultID(this);
        }

        public override LocalizationOption LoadLocalization()
        {
            return EnokoLocalization.ExhibitsBatchLoc.AddEntity(this);
        }

        public override ExhibitSprites LoadSprite()
        {
            return EnokoImageLoader.LoadExhibitSprite(exhibit: this);
        }

        public override ExhibitConfig MakeConfig()
        {
            return GetDefaultExhibitConfig();
        }

        public ExhibitConfig GetDefaultExhibitConfig()
        {
            return EnokoDefaultConfig.GetDefaultExhibitConfig();
        }

    }
}
