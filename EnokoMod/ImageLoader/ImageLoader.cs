using System;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using UnityEngine;

namespace EnokoMod.ImageLoader
{
    public sealed class EnokoImageLoader
    {
        public static string file_extension = ".png";
        public static PlayerImages LoadPlayerImages(string name)
        {
            PlayerImages sprites = new PlayerImages();
            sprites.AutoLoad(name, (s) => ResourceLoader.LoadSprite(s, BepinexPlugin.directorySource, ppu: 100, 1, FilterMode.Bilinear, generateMipMaps: true), (s) => ResourceLoader.LoadSpriteAsync(s, BepinexPlugin.directorySource));
            return sprites;
        }

        public static CardImages LoadCardImages(CardTemplate cardTemplate, bool UseDefault = false)
        {
            
            if (!UseDefault)
            {
                var imgs = new CardImages(BepinexPlugin.embeddedSource);
                imgs.AutoLoad(cardTemplate, extension: file_extension);
                return imgs;
            }
            else
            {
                var imgs = new CardImages(BepinexPlugin.embeddedSource);
                imgs.AutoLoad("EnokoDefault", extension: file_extension, "");
                return imgs;
            }
        }

        public static ExhibitSprites LoadExhibitSprite(ExhibitTemplate exhibit)
        {
            var exhibitSprites = new ExhibitSprites();
            Sprite wrap(string s) => ResourceLoader.LoadSprite(exhibit.GetId() + s + file_extension, BepinexPlugin.embeddedSource);
            exhibitSprites.main = wrap("");
            return exhibitSprites;
        }

        public static Sprite LoadUltLoader(UltimateSkillTemplate ult)
        {
            return LoadSprite(ult.GetId());
        }

        public static Sprite LoadStatusEffectLoader(StatusEffectTemplate status)
        {
            return LoadSprite(status.GetId());
        }

        public static Sprite LoadSprite(IdContainer ID)
        {
            return ResourceLoader.LoadSprite(ID + file_extension, BepinexPlugin.embeddedSource);
        }
    }
}
