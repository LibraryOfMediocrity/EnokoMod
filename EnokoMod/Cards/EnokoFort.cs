using EnokoMod.Cards.Templates;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnokoMod.Cards
{
    public sealed class EnokoFortDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Common;
            config.Type = CardType.Defense;
            config.Colors = new List<ManaColor>() { ManaColor.White };
            config.Cost = new ManaGroup() { White = 2 };
            config.UpgradedCost = new ManaGroup() { Any = 1, White = 1 };
            config.TargetType = TargetType.Nobody;
            config.Block = 13;
            config.UpgradedBlock = 16;
            config.RelativeCards = new List<string>() { "EnokoBearTrap" };
            config.UpgradedRelativeCards = new List<string>() { "EnokoBearTrap" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoFortDef))]
    public sealed class EnokoFort : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return DefenseAction();
            yield return new AddCardsToHandAction(new Card[] { Library.CreateCard<EnokoBearTrap>()});
            yield break;
        }
    }
}
