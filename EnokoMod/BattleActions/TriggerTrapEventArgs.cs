using EnokoMod.Cards.Templates;
using LBoL.Core;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using System.Linq;

namespace EnokoMod.BattleActions
{
    public class TriggerTrapEventArgs : GameEventArgs
    {
        public TrapCard Card { get; internal set; }

        public Unit[] Units { get; internal set; }

        public override string GetBaseDebugString()
        {
            if (Units == null || Units.Length == 0) return "Triggered " + this.Card.Name + " with null units. Card may have been removed from battle."; 
            string[] targets = Units.Select(unit => unit.Name).ToArray();
            return "Card: " + this.Card.Name + " -> Units: " + string.Join(", ", targets);
        }
    }
}
