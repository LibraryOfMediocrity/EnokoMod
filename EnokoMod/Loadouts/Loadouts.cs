using System.Collections.Generic;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.EntityLib.Cards.Neutral.NoColor;
using EnokoMod.Cards;
//using EnokoMod.Exhibits;
//using EnokoMod.EnokoUlt;
namespace EnokoMod.Loadouts
{
    public class EnokoLoadouts
    {
        /*
        public static string UltimateSkillA = nameof(EnokoUltA);
        public static string UltimateSkillB = nameof(EnokoUltB);

        public static string ExhibitA = nameof(EnokoExhibitA);
        public static string ExhibitB = nameof(EnokoExhibitB);
        */
        public static List<string> DeckA = new List<string>{
            "Shoot", "Shoot", "Boundary", "Boundary", 
            "EnokoAttackW", "EnokoAttackW", "EnokoAttackW", 
            "EnokoBlockB", "EnokoBlockB", "EnokoFort"
            //put Enoko cards here when done
        };

        public static List<string> DeckB = new List<string>{
            "Shoot", "Shoot", "Boundary", "Boundary",
            "EnokoAttackB", "EnokoAttackB", "EnokoBlockW", 
            "EnokoBlockW", "EnokoBlockW", "EnokoNetGun"
            // put Enoko cards here when done
            
        };

        public static PlayerUnitConfig playerUnitConfig = new PlayerUnitConfig(
            Id: BepinexPlugin.modId,
            ShowOrder: 8,
            Order: 0,
            UnlockLevel: new int?(0),
            ModleName: "",
            HasHomeName: false,
            NarrativeColor: "#C9CBF4",
            IsSelectable: true,
            MaxHp: 78,
            InitialMana: new ManaGroup() { White = 2, Black = 2 },
            InitialMoney: 50,
            InitialPower: 0,
            UltimateSkillA: "EnokoUltW",
            UltimateSkillB: "EnokoUltB",
            ExhibitA: "EnokoExhibitW",
            ExhibitB: "EnokoExhibitB",
            BasicRingOrder: null,
            LeftColor: ManaColor.White,
            RightColor: ManaColor.Black,

            DeckA: EnokoLoadouts.DeckA,
            DeckB: EnokoLoadouts.DeckB,
            DifficultyA: 3,
            DifficultyB: 2
        );
    }
}

