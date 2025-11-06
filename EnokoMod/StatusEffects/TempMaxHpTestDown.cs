using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System.Collections;
using System.Collections.Generic;

namespace EnokoMod.StatusEffects
{
    public sealed class TempMaxHpTestDownDef : EnokoStatusEffectTemplate
    {
        public override StatusEffectConfig MakeConfig()
        {
            StatusEffectConfig config = GetDefaultStatusEffectConfig();
            config.Type = StatusEffectType.Special;
            return config;
        }
    }

    [EntityLogic(typeof(TempMaxHpTestDownDef))]
    public sealed class TempMaxHpTestDown : StatusEffect, IOpposing<TempMaxHpTest>
    {
        protected override void OnAdded(Unit unit)
        {
            base.HandleOwnerEvent<GameEventArgs>(base.Battle.BattleEnding, delegate
            {
                int num1 = Owner.Hp;
                int num2 = Owner.MaxHp + Level;
                base.Battle.Player.SetMaxHp(num1, num2);
                IGameRunVisualTrigger visualTrigger = base.GameRun.VisualTrigger;
                visualTrigger?.OnSetHpAndMaxHp(num1, num2, true);
            });
        }

        public OpposeResult Oppose(TempMaxHpTest other)
        {
            if (base.Level < other.Level)
            {
                other.Level -= base.Level;
                return OpposeResult.KeepOther;
            }
            if (base.Level == other.Level)
            {
                return OpposeResult.Neutralize;
            }
            base.Level -= other.Level;
            return OpposeResult.KeepSelf;
        }
    }
}
