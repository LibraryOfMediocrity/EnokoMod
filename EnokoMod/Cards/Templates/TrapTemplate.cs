using LBoL.Base;
using LBoL.ConfigData;
using System.Collections.Generic;
using System.Linq;

namespace EnokoMod.Cards.Templates
{
    public abstract class TrapTemplate : EnokoCardTemplate
    {
        public CardConfig GetTrapDefaultConfig()
        {
            CardConfig config = base.GetCardDefaultConfig();
            config.TargetType = TargetType.SingleEnemy;
            config.Colors = new List<ManaColor>() { ManaColor.White };
            config.Type = CardType.Attack;
            config.IsPooled = false;
            config.Keywords = Keyword.Retain | Keyword.Exile;
            config.UpgradedKeywords = Keyword.Retain | Keyword.Exile;
            config.RelativeEffects = new List<string>() { "TrapCardDisc" };
            config.UpgradedRelativeEffects = new List<string>() { "TrapCardDisc" };
            return config;
        }
    }
}
