using EnokoMod.StatusEffects;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnokoMod.Cards.Templates
{
    public abstract class BurialCard : Card
    {

        public string BurialDescription
        {
            get
            {
                return this.LocalizeProperty("ShortDescription", decorated: true);
            }
        }

        protected override void OnLeaveBattle()
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
            if (effect != null)
            {
                effect.Level--;
                if (effect.Level == 0) base.React(new RemoveStatusEffectAction(effect));
            }
        }

        public bool IsBuried
        {
            get { return this.Zone == CardZone.Exile; }
        }

        public abstract int CardIndex { get; }

        public override IEnumerable<BattleAction> OnExile(CardZone srcZone)
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
            if(effect == null)
            {
                yield return BuffAction<BurialIndicator>(level: 1, limit: CardIndex);
            }
            else
            {
                effect.Level++;
            }
            yield break;
        }

        public override bool OnMoveVisual => false;

        public override IEnumerable<BattleAction> OnMove(CardZone srcZone, CardZone dstZone)
        {
            if (srcZone != CardZone.Exile && dstZone == CardZone.Exile)
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
                if (effect == null)
                {
                    yield return BuffAction<BurialIndicator>(level: 1, limit: CardIndex);
                }
                else
                {
                    effect.Level++;
                }
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
                if (effect != null)
                {
                    effect.Level--;
                    if (effect.Level == 0) yield return new RemoveStatusEffectAction(effect);
                }
            }
            yield break;
        }
        
    }
}
