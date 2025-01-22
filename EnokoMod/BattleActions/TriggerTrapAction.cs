﻿using EnokoMod.Cards.Templates;
using EnokoMod.TrapToolBox;
using LBoL.Core;
using LBoL.Core.Battle;
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

        public TriggerTrapAction(TrapCard card, TrapSelector selection)
        {
            base.Args = new TriggerTrapEventArgs
            {
                Card = card,
                Units = TrapTools.SelectUnit(selection, base.Battle)
            };
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

                yield return CreateEventPhase<TriggerTrapEventArgs>("PreTrigger", Args, EnokoGameEvents.PreTriggerEvent);

                yield return CreatePhase("Main", delegate
                {
                    base.React(new Reactor(Args.Card.TrapTriggered(unit: Args.Units)));
                }, true);

                yield return CreateEventPhase<TriggerTrapEventArgs>("PostTrigger", Args, EnokoGameEvents.PostTriggerEvent);
            }
            yield break;
        }

    }
}
