using EnokoMod.Cards.Templates;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace EnokoMod.Cards
{
    public sealed class EnokoInfiniteDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Attack;
            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Black = 1 };
            config.UpgradedCost = new ManaGroup() { Any = 1 };
            config.Value1 = 1;
            config.Damage = 7;
            config.UpgradedDamage = 10;
            config.TargetType = TargetType.SingleEnemy;
            config.Keywords = Keyword.Exile;
            config.UpgradedKeywords = Keyword.Exile;
            config.RelativeEffects = new List<string>() { nameof(Graze) };
            config.UpgradedRelativeEffects = config.RelativeEffects;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoInfiniteDef))]
    public sealed class EnokoInfinite : Card
    {

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<Graze>(level: Value1);
            yield return AttackAction(selector);
            if(consumingMana != ManaGroup.Empty)
            {
                Card card = Library.CreateCard<EnokoInfinite>(IsUpgraded);
                yield return new AddCardsToHandAction(card);
            }
            yield break;
        }
    }
}
