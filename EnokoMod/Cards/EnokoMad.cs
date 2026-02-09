using EnokoMod.Cards.Templates;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace EnokoMod.Cards
{
    public sealed class EnokoMadDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Rare;
            config.Type = CardType.Attack;
            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Any = 1, Black = 1 };
            config.Damage = 12;
            config.UpgradedDamage = 14;
            config.Value1 = 1;
            config.UpgradedValue1 = 2;
            config.Value2 = 1;
            config.Keywords = Keyword.Grow;
            config.UpgradedKeywords = Keyword.Grow;
            config.TargetType = TargetType.SingleEnemy;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoMadDef))]
    public sealed class EnokoMad : Card
    {
        public override int AdditionalValue1 => GrowCount * Value2;

        // This card's code was shamelessly copied from that one suika card
        public DamageInfo HalfDamage
        {
            get
            {
                return this.Damage.MultiplyBy(0.5f);
            }
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            for(int i = 0; i < Value1; i++)
            {
                EnemyUnit target = selector.GetEnemy(base.Battle);
                yield return base.AttackAction(target);
                List<Unit> list = base.Battle.EnemyGroup.Alives.Where((EnemyUnit enemy) => enemy != target).Cast<Unit>().ToList();
                yield return base.AttackAction(list, "Instant", this.HalfDamage);
            }
            yield break;
        }
    }
}
