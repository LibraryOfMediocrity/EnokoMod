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
using LBoL.Core.Battle.Interactions;
using UnityEngine;
using System.Linq;

namespace EnokoMod.Cards
{
    public sealed class EnokoRecklessDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Skill;
            config.Colors = new List<ManaColor>() { ManaColor.White, ManaColor.Black };
            config.Cost = new ManaGroup() { Any = 1, Hybrid = 2, HybridColor = 1 };
            config.UpgradedCost = new ManaGroup() { Hybrid = 2, HybridColor = 1 };
            config.Value1 = 4;
            config.TargetType = TargetType.SingleEnemy;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoRecklessDef))]
    public sealed class EnokoReckless : Card
    {
        private string Header1
        {
            get
            {
                return this.LocalizeProperty("Header1");
            }
        }

        public override Interaction Precondition()
        {
            if (base.Battle.DrawZone.Count > 0 || base.Battle.DiscardZone.Count > 0)
            {
                SelectCardInteraction interaction = new SelectCardInteraction(1, 1, base.Battle.DrawZoneToShow.Concat(base.Battle.DiscardZone))
                {
                    Source = this,
                    Description = Header1
                };
                return interaction;
            }
            return null;
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return SacrificeAction(Value1);
            if (precondition != null)
            {
                yield return new PlayCardAction(((SelectCardInteraction)precondition).SelectedCards.First(), selector);
            }
            yield break;
        }
    }
}
