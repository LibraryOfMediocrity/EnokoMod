using EnokoMod.BattleActions;
using EnokoMod.Cards.Templates;
using EnokoMod.TrapToolBox;
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
            config.RelativeCards = new List<string>() { nameof(EnokoAwakenedTrap) };
            config.UpgradedRelativeCards = new List<string>() { nameof(EnokoAwakenedTrap) + "+" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoSlumberingTrapDef))]
    public sealed class EnokoSlumberingTrap : TrapCard
    {
        public int Counter { get; set; }

        protected override void OnEnterBattle(BattleController battle)
        {
            Counter = 0;
        }

        public override IEnumerable<BattleAction> OnTurnEndingInHand()
        {
            yield return new TriggerTrapAction(this);
            yield break;
        }

        public override IEnumerable<BattleAction> TrapTriggered(Unit[] units)
        {
            yield return AttackAction(units);
            Counter++;
            if (Counter == Value1)
            {
                if (Battle.HandIsNotFull)
                    yield return new AddCardsToHandAction(Library.CreateCard<EnokoSlumberingTrap>());
                else
                    yield return new AddCardsToDrawZoneAction(Library.CreateCards<EnokoSlumberingTrap>(1), DrawZoneTarget.Top);
                yield return new RemoveCardAction(this);
            }
            yield break;
        }
    }

    public sealed class EnokoAwakenedTrapDef : TrapTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetTrapDefaultConfig();
            config.Rarity = Rarity.Uncommon;
            config.Damage = 10;
            config.UpgradedDamage = 15;
            config.Value1 = 2;
            config.Colors = new List<ManaColor>() { ManaColor.Red };
            config.Cost = new ManaGroup() { Any = 2 };
            config.Index = CardIndexGenerator.GetUniqueIndex(config, true);
            return config;
        }
    }


    [EntityLogic(typeof(EnokoAwakenedTrapDef))]
    public sealed class EnokoAwakenedTrap : TrapCard
    {

        private int counter = 0;

        public override IEnumerable<BattleAction> OnTurnEndingInHand()
        {
            yield return new TriggerTrapAction(this, TrapSelector.MostLife);
            yield break;
        }

        public override DamageInfo Damage => base.Damage.MultiplyBy((int)Math.Pow(Value1, counter));

        public override IEnumerable<BattleAction> TrapTriggered(Unit[] units)
        {
            yield return AttackAction(units);
            counter++;
            this.NotifyChanged();
        }
    }
}
