using EnokoMod.BattleActions;
using EnokoMod.Cards.Templates;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;

namespace EnokoMod.Cards
{
    public sealed class EnokoSlumberingTrapDef : TrapTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetTrapDefaultConfig();
            config.IsPooled = true;
            config.Rarity = Rarity.Uncommon;
            config.Damage = 10;
            config.UpgradedDamage = 15;
            config.Value1 = 8;
            config.UpgradedValue1 = 6;
            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Any = 2 };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoSlumberingTrapDef))]
    public sealed class EnokoSlumberingTrap : TrapCard
    {
        private int Counter { get; set; }

        private int Power { get; set; }

        public int Remaining 
        {
            get
            {
                return Math.Max(Value1 - Counter, 0);
            }
        }

        protected override void OnEnterBattle(BattleController battle)
        {
            Counter = 0;
            Power = 0;
        }

        public override DamageInfo Damage => base.Damage.MultiplyBy((int)Math.Pow(2, Power));

        public override IEnumerable<BattleAction> OnTurnEndingInHand()
        {
            yield return new TriggerTrapAction(this);
            yield break;
        }

        public override bool Triggered => Counter >= Value1;

        public override IEnumerable<BattleAction> TrapTriggered(Unit[] units)
        {
            yield return AttackAction(units);
            if (Counter < Value1)
            {
                Counter++;
            }
            else
            {
                Power++;
            }
            yield break;
        }
    }
}
