using EnokoMod.Cards.Templates;
using EnokoMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnokoMod.Cards
{
    public sealed class EnokoNetGunDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Type = CardType.Attack;
            config.Rarity = Rarity.Common;
            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Any = 1, Black = 1 };
            config.Damage = 14;
            config.UpgradedDamage = 18;
            config.Value1 = 2;
            config.UpgradedValue1 = 4;
            config.TargetType = TargetType.SingleEnemy;
            config.RelativeEffects = new List<string>() { "EnokoConstrainSe" };
            config.UpgradedRelativeEffects = new List<string>() { "EnokoConstrainSe" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoNetGunDef))]
    public sealed class EnokoNetGun : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return AttackAction(selector);
            yield return DebuffAction<EnokoConstrainSe>((Unit)base.Battle.AllAliveEnemies, level: base.Value1);
            yield break;
        }
    }
}
