using Cysharp.Threading.Tasks;
//using DG.Tweening;
using LBoL.ConfigData;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using UnityEngine;
using EnokoMod.Config;
using EnokoMod.Loadouts;
using EnokoMod.ImageLoader;
using EnokoMod.Localization;
//using EnokoMod.BattleActions;

namespace EnokoMod.Player
{
    public sealed class EnokoPlayerDef : PlayerUnitTemplate
    {
        public UniTask<Sprite>? LoadSpellPortraitAsync { get; private set; }

        public override IdContainer GetId()
        {
            return BepinexPlugin.modId;
        }

        public override LocalizationOption LoadLocalization()
        {
            return EnokoLocalization.PlayerUnitBatchLoc.AddEntity(this);
        }

        public override PlayerImages LoadPlayerImages()
        {
            return EnokoImageLoader.LoadPlayerImages(BepinexPlugin.playerName);
        }

        public override PlayerUnitConfig MakeConfig()
        {
            return EnokoLoadouts.playerUnitConfig;
        }

        [EntityLogic(typeof(EnokoPlayerDef))]
        public sealed class EnokoMod : PlayerUnit
        {
        }
    }
}
