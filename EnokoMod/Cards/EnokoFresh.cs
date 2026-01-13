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
    public sealed class EnokoFreshDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Rare;
            config.Type = CardType.Skill;
            config.Colors = new List<ManaColor>() { ManaColor.White, ManaColor.Black };
            config.Cost = new ManaGroup() { Any = 0 };
            config.Value1 = 5;
            config.UpgradedValue1 = 7;
            config.TargetType = TargetType.Nobody;
            config.Keywords = Keyword.Exile;
            config.UpgradedKeywords = Keyword.Exile;
            config.RelativeKeyword = Keyword.Retain;
            config.UpgradedRelativeKeyword = Keyword.Retain;
            config.RelativeEffects = new List<string>() { nameof(Burial) };
            config.UpgradedRelativeEffects = config.RelativeEffects;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoFreshDef))]
    public sealed class EnokoFresh : BurialCard
    {
        public override int CardIndex => 4;

        protected override void OnEnterBattle(BattleController battle)
        {
            base.ReactBattleEvent<UnitEventArgs>(Battle.Player.TurnEnding, OnTurnEnding);
        }

        private IEnumerable<BattleAction> OnTurnEnding(UnitEventArgs args)
        {
            if (IsBuried)
            {
                yield return new MoveCardAction(this, CardZone.Discard);
            }
            yield break;
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            List<Card> cards = Battle.HandZone.Where((Card c) => !c.IsRetain).ToList();
            yield return new DiscardManyAction(cards);
            yield return new DrawManyCardAction(Value1);
            yield break;
        }
    }
}
