using EnokoMod.BattleActions;
using EnokoMod.Cards.Templates;
using EnokoMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace EnokoMod.Cards
{
    public sealed class EnokoBarrageDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Skill;
            config.Colors = new List<ManaColor>() { ManaColor.White };
            config.Value1 = 3;
            config.Cost = new ManaGroup() { White = 1 };
            config.TargetType = TargetType.Nobody;
            config.RelativeCards = new List<string>() { nameof(EnokoTinyTrap) };
            config.UpgradedRelativeCards = new List<string>() { nameof(EnokoTinyTrap) + "+" };
            config.RelativeEffects = new List<string>() { nameof(TrapCardDisc), nameof(Burial) };
            config.UpgradedRelativeEffects = config.RelativeEffects;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoBarrageDef))]
    public sealed class EnokoBarrage : BurialCard
    {
        public override int CardIndex => 3;

        protected override void OnEnterBattle(BattleController battle)
        {
            base.ReactBattleEvent<UnitEventArgs>(base.Battle.Player.TurnEnding, OnTurnEnding);
        }

        private IEnumerable<BattleAction> OnTurnEnding(UnitEventArgs args)
        {
            if(IsBuried && !base.Battle.BattleShouldEnd)
            {
                List<TrapCard> cards = Battle.HandZone.Where((Card c) => c is TrapCard && c.CardType == CardType.Attack).Cast<TrapCard>().ToList();
                foreach (TrapCard card in cards)
                {
                    yield return new TriggerTrapAction(card);
                }
            }  
            yield break;
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new AddCardsToHandAction(Library.CreateCards<EnokoTinyTrap>(Value1, IsUpgraded));
            yield break;
        }
    }
}
