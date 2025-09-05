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
using LBoL.Core.StatusEffects;
using LBoL.EntityLib.StatusEffects.Others;
using EnokoMod.StatusEffects;

namespace EnokoMod.Cards
{
    public sealed class EnokoDiscombobulateDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Attack;
            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Any = 2, Black = 1 };
            config.UpgradedCost = new ManaGroup() { Any = 1, Black = 1 };
            config.Damage = 10;
            config.UpgradedDamage = 15;
            config.Value1 = 1;
            config.Value2 = 3;
            config.TargetType = TargetType.SingleEnemy;
            config.RelativeEffects = new List<string>() 
            {
                nameof(Weak),
                nameof(Vulnerable),
                nameof(TempFirepowerNegative),
                nameof(Poison),
                nameof(LockedOn),
                nameof(EnokoConstrainSe) 
            };
            config.UpgradedRelativeEffects = new List<string>()
            {
                nameof(Weak),
                nameof(Vulnerable),
                nameof(TempFirepowerNegative),
                nameof(Poison),
                nameof(LockedOn),
                nameof(EnokoConstrainSe)
            };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoDiscombobulateDef))]
    public sealed class EnokoDiscombobulate : Card
    {

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            List<Type> effects = new List<Type>() 
            { 
                typeof(Weak), 
                typeof(Vulnerable), 
                typeof(TempFirepowerNegative),
                typeof(Poison),
                typeof(LockedOn),
                typeof(EnokoConstrainSe)
            };
            int i = 0;
            yield return AttackAction(selector);
            foreach (Type type in effects)
            {
                yield return DebuffAction(type, selector.SelectedEnemy, level: (i > 2 ? Value2 : Value1), duration: (i > 2 ? Value2 : Value1));
                i++;
                if (base.Battle.BattleShouldEnd) yield break;
            }
            yield break;
        }
    }
}
