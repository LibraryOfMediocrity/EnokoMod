using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.Exhibits;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnokoMod.StatusEffects
{
    public sealed class EnokoBolasSeDef : EnokoStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.RelativeEffects = new List<string>() { nameof(EnokoConstrainSe), nameof(Weak) };
            config.HasCount = true;
            config.CountStackType = StackType.Add;
            return config;
        }

    }

    [EntityLogic(typeof(EnokoBolasSeDef))]
    public sealed class EnokoBolasSe : StatusEffect
    {
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<UnitEventArgs>(Battle.Player.TurnStarted, new EventSequencedReactor<UnitEventArgs>(this.OnTurnStarted));
        }

        private IEnumerable<BattleAction> OnTurnStarted(UnitEventArgs args)
        {
            this.NotifyActivating();
            foreach (BattleAction action in DebuffAction<EnokoConstrainSe>(Battle.AllAliveEnemies, level: this.Level))
            {
                yield return action;
            }
            foreach (BattleAction action in DebuffAction<Weak>(Battle.AllAliveEnemies, duration: this.Count))
            {
                yield return action;
            }
            yield return new RemoveStatusEffectAction(this);
            yield break;
        }
    }
}
