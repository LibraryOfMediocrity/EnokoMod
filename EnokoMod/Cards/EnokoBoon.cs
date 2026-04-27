using EnokoMod.Cards.Templates;
using EnokoMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.Cards;
using LBoL.EntityLib.Cards.Enemy;
using LBoL.EntityLib.Cards.Neutral.Green;
using LBoL.EntityLib.StatusEffects.Enemy;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnokoMod.Cards
{
    public sealed class EnokoBoonDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Skill;
            config.Colors = new List<ManaColor>() { ManaColor.White };
            config.Cost = new ManaGroup() { Any = 2 };
            config.Value1 = 2;
            config.Keywords = Keyword.Forbidden | Keyword.Ethereal;
            config.UpgradedKeywords = Keyword.Forbidden | Keyword.Ethereal;
            config.RelativeKeyword = Keyword.Exile;
            config.UpgradedRelativeKeyword = Keyword.Exile;
            config.TargetType = TargetType.Nobody;
            config.RelativeCards = new List<string>() { "BoonCardW", "BoonCardE", "BoonCardO", "Xuanguang" };
            config.UpgradedRelativeCards = new List<string>() { "BoonCardW+", "BoonCardE+", "BoonCardO+", "Xuanguang" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoBoonDef))]
    public sealed class EnokoBoon : Card
    {
        private string Header
        {
            get
            {
                return this.LocalizeProperty("Header");
            }
        }

        protected override void OnEnterBattle(BattleController battle)
        {
            base.ReactBattleEvent<GameEventArgs>(base.Battle.BattleStarted, new EventSequencedReactor<GameEventArgs>(this.OnBattleStarted));
        }

        private IEnumerable<BattleAction> OnBattleStarted(GameEventArgs args)
        {
            yield return new ExileCardAction(this);
            List<Card> list = new List<Card>()
            {
                Library.CreateCard<BoonCardW>(this.IsUpgraded),
                Library.CreateCard<BoonCardE>(this.IsUpgraded),
                Library.CreateCard<BoonCardO>(this.IsUpgraded)
            };
            SelectCardInteraction interaction = new SelectCardInteraction(1, 1, list, SelectedCardHandling.DoNothing)
            {
                Source = this,
                Description = Header
            };
            yield return new InteractionAction(interaction, false);
            if (interaction.SelectedCards[0] is OptionCard optionCard)
            {
                optionCard.SetBattle(base.Battle);
                foreach (BattleAction battleAction in optionCard.TakeEffectActions())
                {
                    yield return battleAction;
                }
            }
            yield return new AddCardsToDrawZoneAction(Library.CreateCards<Xuanguang>(Value1), DrawZoneTarget.Random);
            yield break;
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield break;
        }
    }

    public sealed class BoonCardEDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.HideMesuem = true;
            config.IsPooled = false;
            config.FindInBattle = false;
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Ability;
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
            yield return base.BuffAction<BoonESe>(base.Value1, limit: (IsUpgraded ? 1 : 0));
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

    public sealed class BoonCardODef : EnokoCardTemplate
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
            config.Value1 = 7;
            config.UpgradedValue1 = 9;
            config.Value2 = 5;
            config.UpgradedValue2 = 7;
            config.TargetType = TargetType.Nobody;
            config.RelativeEffects = new List<string>() { "DroneBlock" };
            config.UpgradedRelativeEffects = new List<string>() { "DroneBlock" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(BoonCardODef))]
    public sealed class BoonCardO : OptionCard
    {
        public override IEnumerable<BattleAction> TakeEffectActions()
        {
            yield return base.BuffAction<DroneBlock>(base.Value1, 0, 0, 0, 0.2f);
            yield return HealAction(Value2);
            yield break;
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return base.BuffAction<DroneBlock>(base.Value1, 0, 0, 0, 0.2f);
            yield return HealAction(Value2);
            yield break;
        }
    }

    public sealed class BoonCardWDef : EnokoCardTemplate
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
            config.Value1 = 2;
            config.UpgradedValue1 = 4;
            config.TargetType = TargetType.Nobody;
            config.RelativeEffects = new List<string>() { "Firepower", "Spirit" };
            config.UpgradedRelativeEffects = new List<string>() { "Firepower", "Spirit" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(BoonCardWDef))]
    public sealed class BoonCardW : OptionCard
    {
        public override IEnumerable<BattleAction> TakeEffectActions()
        {
            yield return base.BuffAction<Firepower>(base.Value1, 0, 0, 0, 0.2f);
            yield return base.BuffAction<Spirit>(base.Value1, 0, 0, 0, 0.2f);
            yield break;
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return base.BuffAction<Firepower>(base.Value1, 0, 0, 0, 0.2f);
            yield return base.BuffAction<Spirit>(base.Value1, 0, 0, 0, 0.2f);
            yield break;
        }
    }
}
