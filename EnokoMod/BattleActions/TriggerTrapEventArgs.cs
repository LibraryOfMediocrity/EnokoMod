using EnokoMod.Cards.Templates;
using LBoL.Core;
using LBoL.Core.Cards;
using LBoL.Core.Units;

namespace EnokoMod.BattleActions
{
    public class TriggerTrapEventArgs : GameEventArgs
    {
        public TrapCard Card { get; internal set; }

        public Unit[] Units { get; internal set; }

        public override string GetBaseDebugString()
        {
            return "Card: " + this.Card.Name + "\nUnits: " + this.Units.ToString();
        }
    }
}
