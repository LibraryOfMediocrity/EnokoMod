using LBoL.ConfigData;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System;
using EnokoMod.Config;
using EnokoMod.ImageLoader;
using EnokoMod.Localization;


namespace EnokoMod.Cards.Templates
{
    public abstract class EnokoCardTemplate : CardTemplate
    {
        public override IdContainer GetId()
        {
            return EnokoDefaultConfig.GetDefaultID(this);
        }

        public virtual bool UseDefault
        {
            get { return true; }
        }

        public override CardImages LoadCardImages()
        {
            return EnokoImageLoader.LoadCardImages(this, UseDefault);
        }

        public override LocalizationOption LoadLocalization()
        {
            return EnokoLocalization.CardsBatchLoc.AddEntity(this);
        }


        public CardConfig GetCardDefaultConfig()
        {
            return EnokoDefaultConfig.GetCardDefaultConfig();
        }
    }


}

