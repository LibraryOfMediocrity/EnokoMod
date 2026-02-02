using Cysharp.Threading.Tasks.Triggers;
using EnokoMod.Cards.Templates;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Presentation.UI.Panels;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EnokoMod.Cards
{
    public sealed class EnokoDebugDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.HideMesuem = true;
            config.IsPooled = false;
            config.FindInBattle = false;
            config.Rarity = Rarity.Common;
            config.Type = CardType.Skill;
            config.Damage = 0;
            config.Colors = new List<ManaColor>() { ManaColor.White, ManaColor.Black };
            config.Cost = new ManaGroup() { Any = 2 };
            config.TargetType = TargetType.Nobody;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoDebugDef))]
    public sealed class EnokoDebug : Card
    {

        public override Interaction Precondition()
        {
            //base.React(new HealAction(Battle.Player, Battle.Player, 10));
            //base.React(new ExileCardAction(this));
            return null;
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            // works but card is not shown (investigate cardUI widget)
            // try creating a new battleaction that extends movecardaction
            Card card = base.Battle.DrawZone.First();
            List<Card> hand = base.Battle._handZone;
            List<Card> draw = base.Battle._drawZone;
            draw.Remove(card);
            hand.Insert(1, card);
            card.Zone = CardZone.Hand;
            yield break;
        }
        
    }
}
