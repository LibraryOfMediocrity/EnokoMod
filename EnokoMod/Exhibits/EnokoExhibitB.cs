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
using System.Linq;
using System.Text;

namespace EnokoMod.Exhibits
{
    public sealed class EnokoExhibitBDef : EnokoExhibitTemplate
    {
        public override ExhibitConfig MakeConfig()
        {
            ExhibitConfig exhibitConfig = this.GetDefaultExhibitConfig();
            exhibitConfig.Mana = new ManaGroup() { Black = 1 };
            exhibitConfig.Value1 = 3;
            exhibitConfig.BaseManaColor = ManaColor.Black;
            exhibitConfig.Keywords = Keyword.Block;
            return exhibitConfig;
        }
    }

    [EntityLogic(typeof(EnokoExhibitBDef))]
    public sealed class EnokoExhibitB : ShiningExhibit
    {
        protected override void OnEnterBattle()
        {
            base.ReactBattleEvent<UnitEventArgs>(base.Battle.Player.TurnEnding, new EventSequencedReactor<UnitEventArgs>(this.OnTurnEnding));
        }

        private IEnumerable<BattleAction> OnTurnEnding(UnitEventArgs args)
        {
            int block = 0;
            foreach (EnemyUnit unit in base.Battle.AllAliveEnemies)
            {
                List<StatusEffect> effects = unit.StatusEffects.Where(delegate(StatusEffect effect)
                {
                    return effect.Type == StatusEffectType.Negative;
                }).ToList();
                if (effects.Count > 0) block += base.Value1;
            }
            this.NotifyActivating();
            yield return new CastBlockShieldAction(base.Battle.Player, new BlockInfo(block));
            yield break;
        }
    }
}
