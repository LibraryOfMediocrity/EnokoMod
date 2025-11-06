using EnokoMod.BattleActions;
using EnokoMod.TrapToolBox;
using LBoL.Base;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using System.Collections.Generic;

namespace EnokoMod.Cards.Templates
{
    public abstract class TrapCard : Card
    {
        /// <summary>
        /// Gets the default target enemy using the <c>SelectUnit</c> method.
        /// </summary>
        public virtual Unit[] DefaultTarget => TrapTools.SelectUnit(TrapSelector.RandomEnemy, base.Battle);
        
        /// <summary>
        /// This effect is called when the trap is triggered. <br></br>
        /// <strong>Must be able to handle both single and multiple targets.</strong>
        /// </summary>
        public abstract IEnumerable<BattleAction> TrapTriggered(Unit[] units);

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            //return TrapTriggered(selector.GetUnits(base.Battle));
            yield return new TriggerTrapAction(this, selector.GetUnits(base.Battle));
            yield break;
        }

        
    }

}
