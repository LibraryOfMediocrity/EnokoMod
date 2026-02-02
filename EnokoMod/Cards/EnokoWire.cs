using EnokoMod.BattleActions;
using EnokoMod.Cards.Templates;
using EnokoMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace EnokoMod.Cards
{
    public sealed class EnokoWireDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Skill;
            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Any = 1, Black = 1 };
            config.UpgradedCost = new ManaGroup() { Any = 1 };
            config.Value1 = 3;
            config.UpgradedValue1 = 4;
            config.RelativeEffects = new List<string>() { nameof(EnokoConstrainSe) };
            config.UpgradedRelativeEffects = config.RelativeEffects;
            config.RelativeCards = new List<string>() { nameof(EnokoBarbTrap) };
            config.UpgradedRelativeCards = config.RelativeCards;
            config.TargetType = TargetType.AllEnemies;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoWireDef))]
    public sealed class EnokoWire : Card
    {

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            foreach(BattleAction action in DebuffAction<EnokoConstrainSe>(selector.GetUnits(Battle), level: Value1))
            {
                yield return action;
            }
            yield return new AddCardsToHandAction(Library.CreateCard<EnokoBarbTrap>());
            yield break;
        }
    }

    public sealed class EnokoBarbTrapDef : TrapTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetTrapDefaultConfig();
            config.Damage = 5;
            config.UpgradedDamage = 7;
            config.Rarity = Rarity.Uncommon;
            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Any = 1 };
            config.Keywords = Keyword.Exile | Keyword.Ethereal;
            config.UpgradedKeywords = config.Keywords;
            config.Index = CardIndexGenerator.GetUniqueIndex(config, true);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoBarbTrapDef))]
    public sealed class EnokoBarbTrap : TrapCard
    {

        protected override void OnEnterBattle(BattleController battle)
        {
            base.ReactBattleEvent<CardUsingEventArgs>(Battle.CardUsed, OnCardUsed);
        }

        private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
        {
            if (Battle.BattleShouldEnd) yield break;
            if(args.Card.CardType == CardType.Attack)
            {
                yield return new TriggerTrapAction(this, args.Selector.GetUnits(Battle));
            }
            yield break;
        }

        public override IEnumerable<BattleAction> TrapTriggered(Unit[] units)
        {
            yield return AttackAction(units);
            yield break;
        }
    }
}
