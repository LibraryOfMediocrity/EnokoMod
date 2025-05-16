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
        /// Returns an array containing the selected enemies. Accepts a single unit or a TrapSelector value.
        /// </summary>
        public static Unit[] SelectUnit(Unit unit) => new Unit[] { unit };

        /// <summary>
        /// Returns an array containing the selected enemies. Accepts a single unit or a TrapSelector value.
        /// </summary>
        public static Unit[] SelectUnit(TrapSelector selector, BattleController battle)
        {
            
            switch (selector)
            {
                case TrapSelector.RandomEnemy:
                    return new Unit[1] { battle.RandomAliveEnemy };
                case TrapSelector.AllEnemies:
                    return battle.AllAliveEnemies.ToArray();
                case TrapSelector.LeastLife:
                    return new Unit[1] { battle.EnemyGroup.Alives.MinBy((EnemyUnit unit) => unit.Hp) };
                case TrapSelector.MostLife:
                    return new Unit[1] { battle.EnemyGroup.Alives.MaxBy((EnemyUnit unit) => unit.Hp) };
                default:
                    return Array.Empty<Unit>();
            }
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
