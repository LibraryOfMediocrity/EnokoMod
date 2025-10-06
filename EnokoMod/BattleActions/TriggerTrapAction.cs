using EnokoMod.Cards.Templates;
using EnokoMod.TrapToolBox;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace EnokoMod.BattleActions
{
    public sealed class TriggerTrapAction : EventBattleAction<TriggerTrapEventArgs>
    {

        public TriggerTrapAction(TrapCard card, Unit[] units)
        {
            base.Args = new TriggerTrapEventArgs
            {
                Card = card,
                Units = units
            };
        }

        public TriggerTrapAction(TrapCard card)
        {
            base.Args = new TriggerTrapEventArgs
            {
                Card = card,
                Units = card.DefaultTarget
            };
        }

        public TriggerTrapAction(TrapCard card, Unit unit) : this(card, TrapTools.SelectUnit(unit))
        {
        }

        public TriggerTrapAction(TrapCard card, TrapSelector selection) : this(card, TrapTools.SelectUnit(selection, card.Battle))
        {
        }

        public override IEnumerable<Phase> GetPhases()
        {
            if (Args.Card != null)
            {
                if (Args.Units == null)
                {
                    Debug.LogWarning("Null Units");
                    yield break;
                }

                if (Args.Card.Zone == CardZone.Hand || Args.Card.Zone == CardZone.PlayArea || Args.Card.Zone == CardZone.FollowArea)
                {

                    yield return CreateEventPhase<TriggerTrapEventArgs>("PreTrigger", Args, EnokoGameEvents.PreTriggerEvent);

                    List<DamageAction> damageActions = new List<DamageAction>();
                    yield return CreatePhase("Main", delegate
                    {
                        //add statisticaltotaldamageaction if there were damageactions
                        if(Args.Card.Zone == CardZone.Hand) Args.Card.NotifyActivating();
                        IEnumerable<BattleAction> actions = Args.Card.TrapTriggered(units: Args.Units);
                        base.React(new Reactor(StatisticalTotalDamageAction.WrapReactorWithStats(actions, damageActions)));
                    }, true);

                    if (damageActions.Count > 0)
                    {
                        yield return CreatePhase("Statistics", delegate
                        {
                            base.Battle.React(new StatisticalTotalDamageAction(damageActions), Args.Card, ActionCause.Card);
                        });
                    }

                    yield return CreateEventPhase<TriggerTrapEventArgs>("PostTrigger", Args, EnokoGameEvents.PostTriggerEvent);
                }
            }
            yield break;
        }

    }
}
