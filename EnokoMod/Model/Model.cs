using Cysharp.Threading.Tasks;
using LBoL.ConfigData;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using LBoLEntitySideloader.Utils;
using UnityEngine;
using EnokoMod.Localization;
using LBoL.Presentation;
using EnokoMod.Player;
using System.Collections.Generic;

namespace EnokoMod.model

{
    public sealed class EnokoModel : UnitModelTemplate
    {
        //If a custom model is used, use a custom sprite for the Ultimate animation.
        public static string spellsprite_name = "EnokoSpell.png";

        public override IdContainer GetId()
        {
            return new EnokoPlayerDef().UniqueId;
        }

        public override LocalizationOption LoadLocalization()
        {
            return EnokoLocalization.UnitModelBatchLoc.AddEntity(this);
        }

        public override ModelOption LoadModelOptions()
        {
            
            //Load the custom character's sprite.
            return new ModelOption(ResourceLoader.LoadSpriteAsync("EnokoModel.png", BepinexPlugin.directorySource, ppu: 650));
            
        }

        public override UniTask<Sprite> LoadSpellSprite()
        {
            
            //Load the custom character's portrait.
            return ResourceLoader.LoadSpriteAsync("EnokoSpell.png", BepinexPlugin.directorySource);
            
        }

        public override UnitModelConfig MakeConfig()
        {
            //change Spellcolor
            UnitModelConfig config = DefaultConfig().Copy();
            config.Flip = true;
            config.Type = 0;
            config.Offset = new Vector2(0, -0.10f);
            config.HasSpellPortrait = true;
            config.SpellColor = new List<Color32> { 
                new Color32(179, 182, 250, byte.MaxValue),
                new Color32(201, 203, 244, byte.MaxValue),
                new Color32(200, 202, 247, 150),
                new Color32(206, 208, 252, byte.MaxValue) };
            return config;
            
        }
    }
}
