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
            string[] targets = Units.Select(unit => unit.Name).ToArray();
            return "Card: " + this.Card.Name + " -> Units: " + string.Join(", ", targets);
        }
    }
}
