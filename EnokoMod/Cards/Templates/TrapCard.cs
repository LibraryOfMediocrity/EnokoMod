using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EnokoMod.Cards.Templates
{
    public abstract class TrapCard : Card
    {
        public virtual Unit[] DefaultTarget()
        { 
            return SelectUnit(TrapSelector.RandomEnemy);
        }

        public abstract IEnumerable<BattleAction> TrapTriggered(Unit[] unit);

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            return TrapTriggered(selector.GetUnits(base.Battle));
        }

        public Unit[] GetUnits(Unit unit) => new Unit[] { unit };


        public enum TrapSelector
        {
            RandomEnemy,
            LeastLife,
            MostLife,
            AllEnemies
        }
        //what if return type was IEnumerable
        public Unit[] SelectUnit(TrapSelector selector)
        {
            switch (selector)
            {
                case TrapSelector.RandomEnemy:
                    return new Unit[1] { base.Battle.RandomAliveEnemy };
                case TrapSelector.AllEnemies:
                    return base.Battle.AllAliveEnemies.ToArray();
                case TrapSelector.LeastLife:
                    return new Unit[1] { base.Battle.EnemyGroup.Alives.MinBy((EnemyUnit unit) => unit.Hp) };
                case TrapSelector.MostLife:
                    return new Unit[1] { base.Battle.EnemyGroup.Alives.MaxBy((EnemyUnit unit) => unit.Hp) };
                default:
                    return Array.Empty<Unit>();
            }
        }
    }

}
