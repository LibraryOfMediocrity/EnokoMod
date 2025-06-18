using EnokoMod.StatusEffects;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnokoMod.Cards.Templates
{
    public abstract class BurialCard : Card
    {
        public bool IsBuried
        {
            get { return this.Zone == CardZone.Exile; }
        }

        public abstract int CardIndex { get; }

        public override IEnumerable<BattleAction> OnExile(CardZone srcZone)
        {
            yield return new ApplyStatusEffectAction<BurialIndicator>(Battle.Player, limit: CardIndex);
            yield break;
        }

        public override IEnumerable<BattleAction> OnMove(CardZone srcZone, CardZone dstZone)
        {
            if (srcZone != CardZone.Exile && dstZone == CardZone.Exile)
            {
                yield return new ApplyStatusEffectAction<BurialIndicator>(Battle.Player, limit: CardIndex);
            }
            if (srcZone == CardZone.Exile && dstZone != CardZone.Exile)
            {
                StatusEffect effect = null;
                foreach (StatusEffect status in Battle.Player.StatusEffects)
                {
                    if (status is BurialIndicator && status.Limit == CardIndex)
                    {
                        effect = status;
                        break;
                    }
                }
                yield return new RemoveStatusEffectAction(effect);
            }
            yield break;
        }
    }
}
