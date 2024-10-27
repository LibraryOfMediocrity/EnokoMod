using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoLEntitySideloader;
using Mono.Cecil.Cil;

namespace EnokoMod.Cards.Templates
{
    public static class CardIndexGenerator
    {
        private static readonly List<ManaColor> offColors = BepinexPlugin.offColors;
        private static int? initial_offset = null;
        private static HashSet<int> uniqueIds = new HashSet<int>() { };

        public const int milx1 = (int)1E7;

        public static HashSet<int> UniqueIds
        {
            get
            {
                if (uniqueIds == null)
                    uniqueIds = new HashSet<int>();
                return uniqueIds;

            }
        }

        internal static void PromiseClearIndexSet() => EntityManager.AddPostLoadAction(() => uniqueIds = null);
        public static int Initial_offset
        {
            get
            {
                if (initial_offset == null)
                {
                    int millions = 0;
                    if (UniqueTracker.Instance.configIndexes.TryGetValue(typeof(CardConfig), out var indexSet))
                    {
                        millions = indexSet.Where(i => i >= milx1).DefaultIfEmpty().Max() / milx1;
                    }
                    millions += 1;
                    initial_offset = millions * milx1;
                }
                return initial_offset.Value;
            }
        }

        public static int GetUniqueIndex(CardConfig config)
        {
            /*
            Generate a unique card ID based on the original offset, the card's rarity, colors and the type:

            Example:
            initial offset = 50000000
            rarity = Common
            color = Blue (Main Color)
            cost = 3
            type = Attack 

            ID = 50123101

            0: Blue is a main color (0=Main Color, 1=Off-Color)
            1: Common (Basic=0, Common=1, Uncommon=2, Rare=2)
            2: Blue (White=1, Blue=2, Black=3, Red=4, Green=5, Colorless=6, Multicolor=9)
            3: Card's cost (Unplayable, X-cost and Cost > 9 will return 9)
            1: Attack (Attack = 1, Defense = 2, Skill = 3, Ability = 4, Teammate = 5)
            01: Amount of cards with the same value whose IDs were set before.   
            */

            int id = Initial_offset;

            //Off-color check
            id += config.Colors.Any(x => offColors.Any(y => y == x)) ? 1000000 : 0;

            //Rarity
            id += config.Keywords.HasFlag(LBoL.Base.Keyword.Basic) ? 0 : (int)(config.Rarity + 1) * 100000;

            //Color
            int color = config.Colors.Count > 1 ? 9 : (int)config.Colors[0];
            id += color * 10000;

            //Cost
            int cost = (config.IsXCost || config.Keywords.HasFlag(LBoL.Base.Keyword.Forbidden) || config.Cost.Total > 9) ? 9 : config.Cost.Total;
            id += cost * 1000;

            //Type
            id += (int)config.Type * 100;

            //Cards with similar parameters
            if (UniqueTracker.Instance.configIndexes.TryGetValue(typeof(CardConfig), out var indexSet))
                while (indexSet.Contains(id)) id++;

            return id;
        }
    }
}
