using EnokoMod.BattleActions;
using EnokoMod.Cards.Templates;
using EnokoMod.StatusEffects;
using EnokoMod.TrapToolBox;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace EnokoMod.Cards
{
    public sealed class EnokoHiddenTrapDef : TrapTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetTrapDefaultConfig();
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Skill;
            config.Colors = new List<ManaColor>() { ManaColor.White, ManaColor.Black };
            config.Cost = new ManaGroup() { Any = 2 };
            config.Value1 = 1;
            config.UpgradedValue1 = 2;
            config.TargetType = TargetType.SingleEnemy;
            config.RelativeEffects = new List<string>() { nameof(TrapCardDisc), nameof(EnokoConstrainSe) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(TrapCardDisc), nameof(EnokoConstrainSe) };
            config.Index = CardIndexGenerator.GetUniqueIndex(config, true);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoHiddenTrapDef))]
    public sealed class EnokoHiddenTrap : TrapCard
    {
        public int TriggerConstrain
        {
            get
            {
                return base.Battle != null ? base.Battle.HandZone.Count * Value1 : 0;
            }
        }

        //public override Unit[] DefaultTarget => TrapTools.SelectUnit(TrapSelector.MostLife, base.Battle);

        public override IEnumerable<BattleAction> OnTurnStartedInHand()
        {
            yield return new TriggerTrapAction(this, TrapSelector.MostLife);
            yield break;
        }

        public override IEnumerable<BattleAction> TrapTriggered(Unit[] units)
        {
            foreach (BattleAction action in DebuffAction<EnokoConstrainSe>(units, level: TriggerConstrain))
            {
                yield return action;
            }
            yield break;
        }

    }
}
