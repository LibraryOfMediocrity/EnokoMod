using EnokoMod.Cards.Templates;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Units;
using EnokoMod.StatusEffects;
using EnokoMod.BattleActions;
using EnokoMod.TrapToolBox;

namespace EnokoMod.Cards
{
    public sealed class EnokoEyeTrapDef : TrapTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetTrapDefaultConfig();
            config.Rarity = Rarity.Common;
            config.Type = CardType.Skill;
            config.TargetType = TargetType.Nobody;
            config.Cost = new ManaGroup() { Any = 1 };
            config.Scry = 5;
            config.UpgradedScry = 7;
            config.Keywords = config.Keywords | Keyword.Scry;
            config.UpgradedKeywords = config.UpgradedKeywords | Keyword.Scry;
            config.Index = CardIndexGenerator.GetUniqueIndex(config, true);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoEyeTrapDef))]
    public sealed class EnokoEyeTrap : TrapCard
    {
        protected override void OnEnterBattle(BattleController battle)
        {
            base.ReactBattleEvent<UnitEventArgs>(base.Battle.Player.TurnStarting, new EventSequencedReactor<UnitEventArgs>(this.OnTurnStarting));
        }

        private IEnumerable<BattleAction> OnTurnStarting(UnitEventArgs args)
        {
            yield return new TriggerTrapAction(this);
            yield break;
        }

        public override Unit[] DefaultTarget => TrapTools.SelectUnit(base.Battle.Player);

        public override IEnumerable<BattleAction> TrapTriggered(Unit[] selector)
        {
            yield return new ScryAction(this.Scry);
            yield break;
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new TriggerTrapAction(this);
            yield break;
        }
    }
}
