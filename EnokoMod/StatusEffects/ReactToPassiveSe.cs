using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace EnokoMod.StatusEffects
{
    public sealed class ReactToPassiveSeDef : EnokoStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.HasLevel = false;
            return config;
        }
    }

    [EntityLogic(typeof(ReactToPassiveSeDef))]
    public sealed class ReactToPassiveSe : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            ReactOwnerEvent(base.Battle.SpecialResolved, OnSpecialResolved);
        }

        private IEnumerable<BattleAction> OnSpecialResolved(CardEventArgs args)
        {
            if (args.Card.CardType == CardType.Friend)
            {
                this.NotifyActivating();
                yield return new CastBlockShieldAction(Battle.Player, 5, 0, cast: false);
            }
            yield break;

        }
    }
}
