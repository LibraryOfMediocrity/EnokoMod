using LBoL.Base;
using LBoL.ConfigData;
using System.Collections.Generic;

namespace EnokoMod.Cards.Templates
{
    public abstract class TrapTemplate : EnokoCardTemplate
    {

        public CardConfig GetTrapDefaultConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.TargetType = TargetType.SingleEnemy;
            config.Colors = new List<ManaColor>() { ManaColor.White };
            config.Type = CardType.Attack;
            config.IsPooled = false;
            config.Keywords = Keyword.Retain | Keyword.Exile;
            config.UpgradedKeywords = Keyword.Retain | Keyword.Exile;
            return config;
        }
    }
}
