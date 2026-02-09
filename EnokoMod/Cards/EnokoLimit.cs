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
using System.Collections.Generic;

namespace EnokoMod.Cards
{
    public sealed class EnokoLimitDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Rare;
            config.Type = CardType.Ability;
            config.Colors = new List<ManaColor>() { ManaColor.White, ManaColor.Black };
            config.Cost = new ManaGroup() { Hybrid = 1, HybridColor = 1 };
            config.Mana = new ManaGroup() { Philosophy = 3 };
            config.UpgradedMana = new ManaGroup() { Philosophy = 4 };
            config.Value1 = 5;
            config.UpgradedValue1 = 8;
            config.RelativeKeyword = Keyword.Philosophy;
            config.UpgradedRelativeKeyword = Keyword.Philosophy;
            config.TargetType = TargetType.Nobody;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoLimitDef))]
    public sealed class EnokoLimit : Card
    {

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new GainManaAction(Mana);
            yield return BuffAction<EnokoLimitSe>(count: Value1);
            foreach (BattleAction battleAction in base.DebuffAction<ExtraTurn>(base.Battle.AllAliveEnemies, level: 1))
            {
                yield return battleAction;
            }
            yield break;
        }
    }

    public sealed class EnokoLimitSeDef : EnokoStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.HasLevel = false;
            config.HasCount = true;
            config.CountStackType = StackType.Add;
            return config;
        }
    }

    [EntityLogic(typeof(EnokoLimitSeDef))]
    public sealed class EnokoLimitSe : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.HandleOwnerEvent<CardUsingEventArgs>(Battle.CardUsing, delegate (CardUsingEventArgs args)
            {
                args.PlayTwice = true;
                Count--;
                if (Count == 0) base.React(new RemoveStatusEffectAction(this));
            });

            base.HandleOwnerEvent<UnitEventArgs>(Owner.TurnEnded, delegate (UnitEventArgs args)
            {
                base.React(new RemoveStatusEffectAction(this));
            });
        }
    }
}
