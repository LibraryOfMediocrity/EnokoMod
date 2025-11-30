using EnokoMod.Cards.Templates;
using EnokoMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnokoMod.Cards
{
    public sealed class EnokoRevengeDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Common;
            config.Type = CardType.Defense;
            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Any = 1, Black = 1 };
            config.UpgradedCost = new ManaGroup() { Any = 2 };
            config.Block = 13;
            config.UpgradedBlock = 16;
            config.Value1 = 3;
            config.Value2 = 2;
            config.TargetType = TargetType.Self;
            config.RelativeEffects = new List<string>() { nameof(EnokoConstrainSe) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(EnokoConstrainSe) };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoRevengeDef))]
    public sealed class EnokoRevenge : Card
    {

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return DefenseAction();
            yield return BuffAction<EnokoRevengeSe>(level: Value2, count: Value1);
            yield break;
        }
    }

    public sealed class EnokoRevengeSeDef : EnokoStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.RelativeEffects = new List<string>() { nameof(EnokoConstrainSe) };
            config.HasCount = true;
            config.CountStackType = StackType.Max;
            return config;
        }
    }

    [EntityLogic(typeof(EnokoRevengeSeDef))]
    public sealed class EnokoRevengeSe : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<StatisticalDamageEventArgs>(base.Owner.StatisticalTotalDamageReceived, new EventSequencedReactor<StatisticalDamageEventArgs>(this.OnStatisticalDamageReceived));
            base.ReactOwnerEvent<UnitEventArgs>(base.Owner.TurnStarted, new EventSequencedReactor<UnitEventArgs>(this.OnOwnerTurnStarted));
        }

        private IEnumerable<BattleAction> OnStatisticalDamageReceived(StatisticalDamageEventArgs args)
        {
            if (args.DamageSource != base.Owner && args.DamageSource.IsAlive)
            {
                foreach (KeyValuePair<Unit, IReadOnlyList<DamageEventArgs>> keyValuePair in args.ArgsTable)
                {
                    keyValuePair.Deconstruct(out Unit unit, out IReadOnlyList<DamageEventArgs> readOnlyList);
                    Unit unit2 = unit;
                    IReadOnlyList<DamageEventArgs> readOnlyList2 = readOnlyList;
                    if (unit2 == base.Owner)
                    {
                        if (readOnlyList2.Any((DamageEventArgs damage) => damage.DamageInfo.DamageType == DamageType.Attack))
                        {
                            base.NotifyActivating();
                            yield return DebuffAction<EnokoConstrainSe>(args.DamageSource, this.Count);
                            int num = base.Level - 1;
                            base.Level = num;
                            if (base.Level == 0)
                            {
                                yield return new RemoveStatusEffectAction(this, true, 0.1f);
                            }
                        }
                    }
                }
            }
            yield break;
        }

        private IEnumerable<BattleAction> OnOwnerTurnStarted(UnitEventArgs args)
        {
            yield return new RemoveStatusEffectAction(this, true, 0.1f);
            yield break;
        }
    }
}
