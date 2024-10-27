
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using UnityEngine.PlayerLoop;


namespace EnokoMod.Localization
{
    public sealed class EnokoLocalization
    {
        public static BatchLocalization CardsBatchLoc = new BatchLocalization(BepinexPlugin.directorySource, typeof(CardTemplate), "Cards");
        public static BatchLocalization ExhibitsBatchLoc = new BatchLocalization(BepinexPlugin.directorySource, typeof(ExhibitTemplate), "Exhibits");
        public static BatchLocalization PlayerUnitBatchLoc = new BatchLocalization(BepinexPlugin.directorySource, typeof(PlayerUnitTemplate), "PlayerUnit");
        public static BatchLocalization UnitModelBatchLoc = new BatchLocalization(BepinexPlugin.directorySource, typeof(UnitModelTemplate), "UnitModel");
        public static BatchLocalization UltimateSkillsBatchLoc = new BatchLocalization(BepinexPlugin.directorySource, typeof(UltimateSkillTemplate), "UltimateSkills");
        public static BatchLocalization StatusEffectsBatchLoc = new BatchLocalization(BepinexPlugin.directorySource, typeof(StatusEffectTemplate), "StatusEffects");

        public static void Init()
        {
            CardsBatchLoc.DiscoverAndLoadLocFiles("Cards");
            ExhibitsBatchLoc.DiscoverAndLoadLocFiles("Exhibits");
            PlayerUnitBatchLoc.DiscoverAndLoadLocFiles("PlayerUnit");
            UnitModelBatchLoc.DiscoverAndLoadLocFiles("UnitModel");
            UltimateSkillsBatchLoc.DiscoverAndLoadLocFiles("UltimateSkills");
            StatusEffectsBatchLoc.DiscoverAndLoadLocFiles("StatusEffects");
        }

    }
}
