using EnokoMod.BattleActions;
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
    public sealed class EnokoMasterDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Rare;
            config.Type = CardType.Ability;
            config.Colors = new List<ManaColor>() { ManaColor.White };
            config.Cost = new ManaGroup() { Any = 2, White = 3 };
            config.UpgradedCost = new ManaGroup() { Any = 1, White = 2 };
            config.Value1 = 5;
            config.Value2 = 4;
            config.TargetType = TargetType.Nobody;
            config.RelativeEffects = new List<string>() { nameof(TrapCardDisc) };
            config.UpgradedRelativeEffects = config.RelativeEffects;
            config.RelativeKeyword = Keyword.Block;
            config.UpgradedRelativeKeyword = Keyword.Block;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoMasterDef))]
    public sealed class EnokoMaster : Card
    {

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<EnokoMasterSe>(level: Value2, count: Value1);
            yield break;
        }
    }

    public sealed class EnokoMasterSeDef : EnokoStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.Keywords = Keyword.Block;
            config.RelativeEffects = new List<string> { nameof(TrapCardDisc) };
            config.CountStackType = StackType.Add;
            config.HasCount = true;
            return config;
        }
    }

    [EntityLogic(typeof(EnokoMasterSeDef))]
    public sealed class EnokoMasterSe : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<TriggerTrapEventArgs>(EnokoGameEvents.PostTriggerEvent, OnTrapTriggered);
        }

        private IEnumerable<BattleAction> OnTrapTriggered(TriggerTrapEventArgs args)
        {
            this.NotifyActivating();
            yield return new CastBlockShieldAction(Owner, Count, 0, cast: false);
            if(args.Card.CardType == CardType.Attack)
            {
                args.Card.DeltaDamage += this.Level;
                args.Card.NotifyChanged();
            }
            yield break;
        }
    }
}
