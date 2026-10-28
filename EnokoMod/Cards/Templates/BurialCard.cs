using EnokoMod.StatusEffects;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using System.Collections.Generic;
using System.Linq;

namespace EnokoMod.Cards.Templates
{
    public abstract class BurialCard : Card
    {
        // There is no animation for special moving a card from follow area to exile
        // so I have to temporarily turn off special exile anim when follow playing a burial card
        // or the card gets stuck visually
        public override bool OnExileVisual => _exileVisual;

        private bool _exileVisual = true;

        public override IEnumerable<BattleAction> AfterFollowPlayAction()
        {
            if(IsExile || IsTempExile) _exileVisual = false;
            return base.AfterFollowPlayAction();
        }

        public string BurialDescription
        {
            get
            {
                return this.LocalizeProperty("ShortDescription", decorated: true);
            }
        }

        public StatusEffect Indicator
        {
            get
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
                return effect;
            }
        }


        protected override void OnLeaveBattle()
        {
            if (base.Battle.BattleShouldEnd)
            {
                return;
            }
            StatusEffect effect = Indicator;
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

        public abstract int CardIndex { get; }  // currently: 8

        public override IEnumerable<BattleAction> OnExile(CardZone srcZone)
        {
            _exileVisual = true;
            if (base.Battle.BattleShouldEnd)
            {
                yield break;
            }
            StatusEffect effect = Indicator;
            int buryCount = Battle.ExileZone.Where((Card card) => card.GetType() == this.GetType()).Count();
            if (effect == null && buryCount == 1)
            {
                yield return BuffAction<BurialIndicator>(level: 1, limit: CardIndex);
            }
            else if(buryCount != effect.Level)
            {
                effect.Level++;
            }
            yield break;
        }

        public override bool OnMoveVisual => false;

        public override IEnumerable<BattleAction> OnMove(CardZone srcZone, CardZone dstZone)
        {
            if (base.Battle.BattleShouldEnd)
            {
                yield break;
            }
            int buryCount = Battle.ExileZone.Where((Card card) => card.GetType() == this.GetType()).Count();
            if (srcZone != CardZone.Exile && dstZone == CardZone.Exile)
            {
                StatusEffect effect = Indicator;
                if (effect == null && buryCount == 1)
                {
                    yield return BuffAction<BurialIndicator>(level: 1, limit: CardIndex);
                }
                else if(buryCount != effect.Level)
                {
                    effect.Level++;
                }
            }
            if (srcZone == CardZone.Exile && dstZone != CardZone.Exile)
            {
                StatusEffect effect = Indicator;
                if (effect != null && buryCount != effect.Level)
                {
                    effect.Level--;
                    if (effect.Level == 0) yield return new RemoveStatusEffectAction(effect);
                }
            }
            yield break;
        }
        
    }
}
