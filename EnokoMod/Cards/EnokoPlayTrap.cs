using EnokoMod.Cards.Templates;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using LBoL.Core.Battle.BattleActions;

namespace EnokoMod.Cards
{
    public sealed class EnokoPlayTrapDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Common;
            config.Type = CardType.Attack;
            config.Colors = new List<ManaColor>() { ManaColor.White };
            config.Cost = new ManaGroup() { Any = 1, White = 1 };
            config.Damage = 8;
            config.TargetType = TargetType.SingleEnemy;
            config.RelativeCards = new List<string>() { "EnokoBearTrap" };
            config.UpgradedRelativeCards = new List<string>() { "EnokoBearTrap+" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoPlayTrapDef))]
    public sealed class EnokoPlayTrap : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return AttackAction(selector);
            Card card = Library.CreateCard<EnokoBearTrap>(this.IsUpgraded);
            yield return new PlayCardAction(card, selector);
            yield break;
        }
    }
}
