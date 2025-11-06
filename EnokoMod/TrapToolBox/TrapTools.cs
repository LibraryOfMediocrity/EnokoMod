using LBoL.Base.Extensions;
using LBoL.Core.Battle;
using LBoL.Core.Units;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace EnokoMod.TrapToolBox
{
    public static class TrapTools
    {
        /// <summary>
        /// Returns an array containing the selected enemies. Accepts a single unit or a <c>TrapSelector</c> value.
        /// </summary>
        public static Unit[] SelectUnit(Unit unit) => new Unit[] { unit };

        /// <summary>
        /// Returns an array containing the selected enemies. Accepts a single unit or a <c>TrapSelector</c> value.
        /// </summary>
        public static Unit[] SelectUnit(TrapSelector selector, BattleController battle)
        {

            return selector switch
            {
                TrapSelector.RandomEnemy => new Unit[1] { battle.RandomAliveEnemy },
                TrapSelector.AllEnemies => battle.AllAliveEnemies.ToArray(),
                TrapSelector.LeastLife => new Unit[1] { battle.EnemyGroup.Alives.MinBy((EnemyUnit unit) => unit.Hp) },
                TrapSelector.MostLife => new Unit[1] { battle.EnemyGroup.Alives.MaxBy((EnemyUnit unit) => unit.Hp) },
                _ => Array.Empty<Unit>(),
            };
        }
    }

    public enum TrapSelector
    {
        RandomEnemy,
        LeastLife,
        MostLife,
        AllEnemies
    }
}
