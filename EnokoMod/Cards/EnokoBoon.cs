using EnokoMod.Cards.Templates;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using LBoL.Core.Battle.BattleActions;
using System.Linq;
using LBoL.EntityLib.Cards.Neutral.Green;
using LBoL.EntityLib.Cards.Enemy;
using LBoL.Core.Battle.Interactions;
using LBoL.EntityLib.Cards;

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
}
