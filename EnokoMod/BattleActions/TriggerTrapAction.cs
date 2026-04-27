using EnokoMod.Cards.Templates;
using EnokoMod.TrapToolBox;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using System.Collections.Generic;
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
                Units = card.Zone != CardZone.None ? card.DefaultTarget : null // in case card gets removed from battle
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
            // nothing happens if the card doesn't exist
            if (Args.Card != null && Args.Card.Zone != CardZone.None)
            {
                // unit null check
                if (Args.Units == null)
                {
                    Debug.LogWarning("Null Units");
                    yield break;
                }
                // card must be in a valid zone to be triggered
                if (Args.Card.Zone == CardZone.Hand || Args.Card.Zone == CardZone.PlayArea || Args.Card.Zone == CardZone.FollowArea)
                {
                    // pre event reactors
                    yield return CreateEventPhase<TriggerTrapEventArgs>("PreTrigger", Args, EnokoGameEvents.PreTriggerEvent);

                    List<DamageAction> damageActions = new List<DamageAction>();
                    yield return CreatePhase("Main", delegate
                    {
                        // react with statisticaltotaldamageaction in case of damage actions
                        if(Args.Card.Zone == CardZone.Hand) Args.Card.NotifyActivating();
                        IEnumerable<BattleAction> actions = Args.Card.TrapTriggered(units: Args.Units);
                        base.React(new Reactor(StatisticalTotalDamageAction.WrapReactorWithStats(actions, damageActions)));
                    }, true);

                    if (damageActions.Count > 0)
                    {
                        // necessary for this to work properly with certain things that react to damage
                        yield return CreatePhase("Statistics", delegate
                        {
                            base.Battle.React(new StatisticalTotalDamageAction(damageActions), Args.Card, ActionCause.Card);
                        });
                    }
                    // just in case
                    yield return CreatePhase("Special Resolve", delegate
                    {
                        base.Battle.React(new SpecialResolveAction(Args.Card), Args.Card, ActionCause.Card);
                    });
                    // post event reactors
                    yield return CreateEventPhase<TriggerTrapEventArgs>("PostTrigger", Args, EnokoGameEvents.PostTriggerEvent);
                }
            }
            yield break;
        }

    }
}
