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
using System.Linq;
using LBoL.Core.StatusEffects;

namespace EnokoMod.Cards
{
    public sealed class EnokoHaymakerDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Common;
            config.Type = CardType.Attack;
            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Any = 1, Black = 1 };
            config.UpgradedCost = new ManaGroup() { Any = 2 };
            config.Damage = 12;
            config.UpgradedDamage = 16;
            config.TargetType = TargetType.SingleEnemy;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoHaymakerDef))]
    public sealed class EnokoHaymaker : Card
    {

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            int hittimes = (selector.SelectedEnemy.StatusEffects.Where((StatusEffect effect) => effect.Type == StatusEffectType.Negative).Any() == true ? 2 : 1);
            for (int i = 0; i < hittimes; i++)
            {
                yield return AttackAction(selector);   
            }
            yield break;
        }
    }
}
