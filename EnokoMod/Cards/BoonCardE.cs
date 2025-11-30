using EnokoMod.Cards.Templates;
using EnokoMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.Cards;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace EnokoMod.Cards
{
    public sealed class BoonCardEDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.HideMesuem = true;
            config.IsPooled = false;
            config.FindInBattle = false;
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Skill;
            config.Colors = new List<ManaColor>() { ManaColor.White };
            config.Cost = new ManaGroup() { Any = 0 };
            config.Value1 = 1;
            config.Mana = new ManaGroup() { Philosophy = 1 };
            config.UpgradedMana = new ManaGroup() { Philosophy = 1, Colorless = 1 };
            config.RelativeKeyword = Keyword.Philosophy;
            config.UpgradedRelativeKeyword = Keyword.Philosophy;
            config.TargetType = TargetType.Nobody;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(BoonCardEDef))]
    public sealed class BoonCardE : OptionCard
    {
        public override IEnumerable<BattleAction> TakeEffectActions()
        {
            yield return base.BuffAction<BoonESe>(base.Value1, limit: (IsUpgraded? 1 : 0));
            yield break;
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return base.BuffAction<BoonESe>(base.Value1, limit: (IsUpgraded ? 1 : 0));
            yield break;
        }
    }

    public sealed class BoonESeDef : EnokoStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.LimitStackType = StackType.Overwrite;
            config.Keywords = Keyword.Philosophy;
            return config;
        }
    }

    [EntityLogic(typeof(BoonESeDef))]
    public sealed class BoonESe : StatusEffect
    {
        public ManaGroup Mana;

        public override bool Stack(StatusEffect other)
        {
            bool result = base.Stack(other);
            if (Limit == 1)
            {
                IEnumerable<ManaColor> colors = Mana.EnumerateComponents().Concat(new List<ManaColor>() { ManaColor.Philosophy, ManaColor.Colorless });
                Mana = ManaGroup.FromComponents(colors);
            }
            else
            {
                IEnumerable<ManaColor> colors = Mana.EnumerateComponents().Concat(new List<ManaColor>() { ManaColor.Philosophy });
                Mana = ManaGroup.FromComponents(colors);
            }
            return result;
        }

        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<UnitEventArgs>(base.Battle.Player.TurnStarted, new EventSequencedReactor<UnitEventArgs>(this.OnTurnStarted));
            if (Limit == 1)
            {
                Mana = new ManaGroup() { Philosophy = 1, Colorless = 1 };
            }
            else
            {
                Mana = new ManaGroup() { Philosophy = 1 };
            }
        }

        private IEnumerable<BattleAction> OnTurnStarted(UnitEventArgs args)
        {
            if (!base.Battle.BattleShouldEnd)
            {
                base.NotifyActivating();
                yield return new GainManaAction(this.Mana);
                yield return new DrawManyCardAction(base.Level);
            }
            yield break;
        }

    }
}
