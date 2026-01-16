using EnokoMod.Cards.Templates;
using EnokoMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace EnokoMod.Cards
{
    public sealed class EnokoAllSeeingDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Rare;
            config.Type = CardType.Ability;
            config.Colors = new List<ManaColor>() { ManaColor.White, ManaColor.Black };
            config.Cost = new ManaGroup() { Any = 1, White = 2, Black = 2 };
            config.Value1 = 1;
            config.TargetType = TargetType.Nobody;
            config.Keywords = Keyword.Ethereal;
            config.RelativeEffects = new List<string>() { nameof(Weak), nameof(Vulnerable) };
            config.UpgradedRelativeEffects = config.RelativeEffects;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoAllSeeingDef))]
    public sealed class EnokoAllSeeing : Card
    {

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<EnokoAllSeeingSe>(Value1);
            yield break;
        }
    }

    public sealed class EnokoAllSeeingSeDef : EnokoStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.RelativeEffects = new List<string>() { nameof(Weak), nameof(Vulnerable) };
            config.Order = 11;
            return config;
        }
    }

    [EntityLogic(typeof(EnokoAllSeeingSeDef))]
    public sealed class EnokoAllSeeingSe : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<UnitEventArgs>(Owner.TurnStarted, OnTurnStarted);
        }

        private IEnumerable<BattleAction> OnTurnStarted(UnitEventArgs args)
        {
            foreach (Unit unit in Battle.AllAliveEnemies)
            {
                if (!unit.HasStatusEffect<Weak>())
                {
                    yield return DebuffAction<Weak>(unit, duration: Level);
                }
                if (!unit.HasStatusEffect<Vulnerable>())
                {
                    yield return DebuffAction<Vulnerable>(unit, duration: Level);
                }
            }
            yield break;
        }
    }
}
