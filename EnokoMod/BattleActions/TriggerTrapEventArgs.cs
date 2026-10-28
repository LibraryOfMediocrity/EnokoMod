using EnokoMod.Cards.Templates;
using LBoL.Core;
using LBoL.Core.Units;
using System;
using System.Linq;

namespace EnokoMod.BattleActions
{
    public class TriggerTrapEventArgs : GameEventArgs
    {
        public TrapCard Card { get; internal set; }

        public Unit[] Units { get; internal set; }

        public override string GetBaseDebugString()
        {
            // error catchers for cases where Trap card is removed from combat (i think it works)
            try
            {
                string[] targets = Units.Select(unit => unit.Name).ToArray();
                return "Card: " + this.Card.Name + " -> Units: " + string.Join(", ", targets);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log("Trap event debug string error:\n" + e.Message);
                return "Card: " + this.Card.Name + " -> Units: dunno, null error probably";
            }
        }
    }
}
