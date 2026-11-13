using EnokoMod.BattleActions;
using EnokoMod.Cards.Templates;
using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace EnokoMod.Cards
{
    public sealed class EnokoLinkDef : TrapTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Skill;
            config.Colors = new List<ManaColor>() { ManaColor.Blue };
            config.Cost = new ManaGroup() { Any = 1, Blue = 1 };
            config.UpgradedCost = new ManaGroup() { Any = 0 };
            config.TargetType = TargetType.Nobody;
            config.Value1 = 1; 
            config.Value2 = 5;
            config.Keywords = Keyword.Retain | Keyword.Exile;
            config.UpgradedKeywords = Keyword.Retain | Keyword.Exile;
            config.RelativeEffects = new List<string>() { "TrapCardDisc" };
            config.UpgradedRelativeEffects = new List<string>() { "TrapCardDisc" };
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoLinkDef))]
    public sealed class EnokoLink : TrapCard
    {
        private string Header1 { get => this.LocalizeProperty("Header1"); }

        public override IEnumerable<BattleAction> OnDraw()
        {
            yield return new TriggerTrapAction(this);
            yield break;
        }

        public override IEnumerable<BattleAction> OnMove(CardZone srcZone, CardZone dstZone)
        {
            if(dstZone == CardZone.Hand) yield return new TriggerTrapAction(this);
            yield break;
        }

        protected override void OnEnterBattle(BattleController battle)
        {
            if (Zone == CardZone.Hand) base.React(new TriggerTrapAction(this));
        }

        public override IEnumerable<BattleAction> TrapTriggered(Unit[] units)
        {
            Card[] cards = Battle.DrawZone.SampleManyOrAll(Value2, BattleRng);
            if(cards.Length != 0)
            {
                MiniSelectCardInteraction interaction = new MiniSelectCardInteraction(cards)
                {
                    Source = this,
                    Description = Header1
                };
                yield return new InteractionAction(interaction);
                Card card = interaction.SelectedCard;
                yield return new MoveCardAction(card, CardZone.Hand);
            }
        }
    }
}
