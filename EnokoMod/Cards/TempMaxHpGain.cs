using EnokoMod.Cards.Templates;
using EnokoMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;

namespace EnokoMod.Cards
{
    public sealed class TempMaxHpGainDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.HideMesuem = true;
            config.IsPooled = false;
            config.FindInBattle = false;
            config.Rarity = Rarity.Common;
            config.Type = CardType.Skill;
            config.Value1 = 10;
            config.UpgradedValue1 = 20;
            config.Colors = new List<ManaColor>() { ManaColor.Black, ManaColor.Green };
            config.Cost = new ManaGroup() { Any = 2 };
            config.TargetType = TargetType.Nobody;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(TempMaxHpGainDef))]
    public sealed class TempMaxHpGain : Card
    {
        public override bool Triggered => Battle.HandZone.Count % 2 == 0;

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            // reminder to retest burialcard
            int num = !Triggered ? 9999 : Value1;
            yield return TempMaxHpChange(Battle.Player, num, this.IsUpgraded);
            yield break;
        }


        /// <summary>
        /// Raise or lower Maximum Life until end of battle. <br></br>
        /// <c>isNegative</c> determines if Maximum Life is raised or lowered.
        /// </summary>
        private BattleAction TempMaxHpChange(Unit target, int amount, bool isNegative = false)
        {
            // could place hp change logic in status to allow them to be buff/debuff
            // would change life when added/stacked/removed + check if battle end removes status(prob not)
            // only edge case: externally changing level won't work as expected
            // place in battleaction if reacting is needed
            if (amount < 0) throw new ArgumentException("amount should not be negative", "amount");
            int newMaxHp = isNegative ? Math.Max(target.MaxHp - amount, 1) : Math.Min(target.MaxHp + amount, 9999);
            int newHp = Math.Min(target.Hp, newMaxHp);
            int change = Math.Abs(target.MaxHp - newMaxHp);
            target.SetMaxHp(newHp, newMaxHp);
            IGameRunVisualTrigger visualTrigger = base.GameRun.VisualTrigger;
            visualTrigger?.OnSetHpAndMaxHp(newHp, newMaxHp, true);
            if(isNegative) return new ApplyStatusEffectAction<TempMaxHpTestDown>(target, level: change, limit: change);
            else return new ApplyStatusEffectAction<TempMaxHpTest>(target, level: change, limit: change);
        }
    }
}
