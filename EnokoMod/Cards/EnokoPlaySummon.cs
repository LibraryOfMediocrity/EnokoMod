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
using System.Collections.Generic;
using System.Linq;

namespace EnokoMod.Cards
{
    public sealed class EnokoPlaySummonDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Defense;
            config.Colors = new List<ManaColor>() { ManaColor.White };
            config.Cost = new ManaGroup() { White = 2 };
            //config.UpgradedCost = new ManaGroup() { Any = 2 };
            config.Block = 10;
            config.UpgradedBlock = 13;
            config.Shield = 5;
            config.UpgradedShield = 7;
            config.TargetType = TargetType.Self;
            config.Keywords = Keyword.Debut;
            config.UpgradedKeywords = Keyword.Debut;
            config.RelativeKeyword = Keyword.Block|Keyword.Shield;
            config.UpgradedRelativeKeyword = Keyword.Block|Keyword.Shield;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoPlaySummonDef))]
    public sealed class EnokoPlaySummon : Card
    {
        protected override string GetBaseDescription()
        {
            if (DebutActive) return base.GetBaseDescription();
            return base.GetExtraDescription1;
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return DefenseAction();
            if (TriggeredAnyhow)
            {
                TrapCard card = Library.CreateCard<EnokoPlayTrap>();
                yield return new AddCardsToHandAction(card);
                yield return new TriggerTrapAction(card, TrapSelector.AllEnemies);
            }
            yield break;
        }
    }

    public sealed class EnokoPlayTrapDef : TrapTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetTrapDefaultConfig();
            config.Damage = 10;
            config.UpgradedDamage = 15;
            config.Block = 10;
            config.UpgradedBlock = 15;
            config.Cost = new ManaGroup() { Any = 1 };
            config.RelativeKeyword = Keyword.Block;
            config.UpgradedRelativeKeyword = Keyword.Block;
            config.Index = CardIndexGenerator.GetUniqueIndex(config, true);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoPlayTrapDef))]
    public sealed class EnokoPlayTrap : TrapCard
    {

        protected override void OnEnterBattle(BattleController battle)
        {
            base.ReactBattleEvent<CardUsingEventArgs>(base.Battle.CardPlayed, OnCardPlayed);
        }

        private IEnumerable<BattleAction> OnCardPlayed(CardUsingEventArgs args)
        {
            yield return new TriggerTrapAction(this, TrapSelector.AllEnemies);
            yield break;
        }

        public override IEnumerable<BattleAction> TrapTriggered(Unit[] units)
        {
            yield return AttackAction(units);
            yield return DefenseAction();
            yield break;
        }

    }


}
