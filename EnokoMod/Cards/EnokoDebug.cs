using EnokoMod.Cards.Templates;
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
            config.Colors = new List<ManaColor>() { ManaColor.White, ManaColor.Black };
            config.Cost = new ManaGroup() { Any = 0 };
            config.TargetType = TargetType.Nobody;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoDebugDef))]
    public sealed class EnokoDebug : Card
    {

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            UnityEngine.Debug.Log("Start of function");
            List<Card> cards = base.Battle.ExileZone.Where((Card exile) => exile is TrapCard || !exile.IsExile).ToList();
            if (cards != null && cards.Count > 0)
            {
                UnityEngine.Debug.Log("Inside if statement");
                SelectCardInteraction interaction = new SelectCardInteraction(0, 1/*Value1*/, cards);
                UnityEngine.Debug.Log("Calling interaction");
                yield return new InteractionAction(interaction); 
                UnityEngine.Debug.Log("Interaction Called");
                if (interaction.SelectedCards.Count > 0)
                {
                    UnityEngine.Debug.Log("Moving cards");
                    yield return new MoveCardAction(interaction.SelectedCards[0], CardZone.Hand);
                }
                UnityEngine.Debug.Log("End of if statement");
            }
            else
            {
                yield return new DrawManyCardAction(Value2);
            }
            yield break;
        }
    }
}
