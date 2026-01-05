using EnokoMod.Cards.Templates;
using EnokoMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace EnokoMod.Cards
{
    public sealed class EnokoScrappyDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Skill;
            config.Colors = new List<ManaColor>() { ManaColor.White };
            config.Cost = new ManaGroup() { Any = 1, White = 1 };
            config.Value1 = 3;
            config.Value2 = 7;
            config.UpgradedValue2 = 10;
            config.TargetType = TargetType.Nobody;
            config.RelativeKeyword = Keyword.Upgrade | Keyword.Exile;
            config.UpgradedRelativeKeyword = Keyword.Upgrade | Keyword.Exile;
            config.RelativeEffects = new List<string>() { nameof(Burial) };
            config.UpgradedRelativeEffects = new List<string>() { nameof(Burial) };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoScrappyDef))]
    public sealed class EnokoScrappy : BurialCard
    {
        public override int CardIndex => 1;

        protected override void OnEnterBattle(BattleController battle)
        {
            base.ReactBattleEvent<CardEventArgs>(Battle.CardExiled, OnCardExiled);
            
        }

        private IEnumerable<BattleAction> OnCardExiled(CardEventArgs args)
        {
            if (IsBuried && args.Card != this)
            {
                Indicator.NotifyActivating();
                yield return new DamageAction(Battle.Player, Battle.AllAliveEnemies, DamageInfo.Reaction(damage: Value2), Config.GunName);
            }
            yield break;
        }

        public override Interaction Precondition()
        {
            List<EnokoScrappy> list = Library.CreateCards<EnokoScrappy>(2, this.IsUpgraded).ToList<EnokoScrappy>();
            EnokoScrappy EnokoScrappy = list[0];
            EnokoScrappy EnokoScrappy2 = list[1];
            EnokoScrappy.ChoiceCardIndicator = 1;
            EnokoScrappy2.ChoiceCardIndicator = 2;
            EnokoScrappy.SetBattle(base.Battle);
            EnokoScrappy2.SetBattle(base.Battle);
            return new MiniSelectCardInteraction(list);
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            MiniSelectCardInteraction miniSelectCardInteraction = (MiniSelectCardInteraction)precondition;
            Card card = (miniSelectCardInteraction?.SelectedCard);
            if (card != null && card.ChoiceCardIndicator == 1)
            {
                if (IsUpgraded)
                {
                    yield return base.UpgradeAllHandsAction();
                }
                else
                {
                    yield return base.UpgradeRandomHandAction(Value1);
                }
            }
            else
            {
                yield return new ExileCardAction(this);
            }
            yield break;
        }
    }
}
