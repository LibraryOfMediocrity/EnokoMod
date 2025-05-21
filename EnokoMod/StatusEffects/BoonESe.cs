using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.StatusEffects;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using LBoL.Core.Units;
using System.Linq;

namespace EnokoMod.StatusEffects
{
    public sealed class BoonESeDef : EnokoStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.LimitStackType = LBoL.Base.StackType.Overwrite;
            config.Keywords = Keyword.Philosophy;
            return config;
        }
    }

    [EntityLogic(typeof(BoonESeDef))]
    public sealed class BoonESe : StatusEffect
    {
        public ManaGroup Mana;

        public override bool Stack(StatusEffect other)
        {
            bool result = base.Stack(other);
            if (Limit == 1)
            {
                IEnumerable<ManaColor> colors = Mana.EnumerateComponents().Concat(new List<ManaColor>() { ManaColor.Philosophy, ManaColor.Colorless });
                Mana = ManaGroup.FromComponents(colors);
            }
            else
            {
                IEnumerable<ManaColor> colors = Mana.EnumerateComponents().Concat(new List<ManaColor>() { ManaColor.Philosophy });
                Mana = ManaGroup.FromComponents(colors);
            }
            return result;
        }

        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<UnitEventArgs>(base.Battle.Player.TurnStarted, new EventSequencedReactor<UnitEventArgs>(this.OnTurnStarted));
            if (Limit == 1)
            {
                Mana = new ManaGroup() { Philosophy = 1, Colorless = 1 };
            }
            else
            {
                Mana = new ManaGroup() { Philosophy = 1 };
            }
        }

        private IEnumerable<BattleAction> OnTurnStarted(UnitEventArgs args)
        {
            if (!base.Battle.BattleShouldEnd)
            {
                base.NotifyActivating();
                yield return new GainManaAction(this.Mana);
                yield return new DrawManyCardAction(base.Level);
            }
            yield break;
        }

    }
}
