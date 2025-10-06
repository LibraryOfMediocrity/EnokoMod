using EnokoMod.Cards.Templates;
using EnokoMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace EnokoMod.Cards
{
    public sealed class EnokoEdgeDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Skill;
            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Any = 1, Black = 2 };
            config.UpgradedCost = new ManaGroup() { Any = 2, Black = 1 };
            config.Value1 = 6;
            config.Value2 = 1;
            config.Mana = new ManaGroup() { Philosophy = 1 };
            config.TargetType = TargetType.SingleEnemy;
            config.RelativeKeyword = Keyword.Philosophy;
            config.UpgradedRelativeKeyword = Keyword.Philosophy;
            config.RelativeEffects = new List<string>() { nameof(EnokoConstrainSe) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(EnokoConstrainSe) };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoEdgeDef))]
    public sealed class EnokoEdge : Card
    {

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return DebuffAction<EnokoConstrainSe>(selector.SelectedEnemy, Value1);
            List<StatusEffect> effects = selector.SelectedEnemy.StatusEffects.Where((StatusEffect effect) => effect.Type == StatusEffectType.Negative).ToList();
            int count = effects.Count;
            yield return new DrawManyCardAction(count);
            ManaGroup gainMana = new ManaGroup() { Philosophy = count };
            yield return new GainManaAction(gainMana);
            yield break;
        }
    }
}
