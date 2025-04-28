using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.EntityLib.Exhibits;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnokoMod.Exhibits
{
    public sealed class EnokoExhibitWDef : EnokoExhibitTemplate
    {
        public override ExhibitConfig MakeConfig()
        {
            ExhibitConfig exhibitConfig = this.GetDefaultExhibitConfig();
            exhibitConfig.Mana = new ManaGroup() { White = 1 };
            exhibitConfig.Value1 = 1;
            exhibitConfig.Keywords = Keyword.Shield | Keyword.Exile;
            exhibitConfig.BaseManaColor = ManaColor.White;
            return exhibitConfig;
        }
    }

    [EntityLogic(typeof(EnokoExhibitWDef))]
    public sealed class EnokoExhibitW : ShiningExhibit
    {
        protected override void OnEnterBattle()
        {
            base.ReactBattleEvent<CardEventArgs>(base.Battle.CardExiled, new EventSequencedReactor<CardEventArgs>(this.OnCardExiled));
        }

        private IEnumerable<BattleAction> OnCardExiled(CardEventArgs args)
        {
            if (args.Card.CardType == CardType.Attack) yield return new CastBlockShieldAction(base.Battle.Player, new ShieldInfo(Value1));
            yield break;
        }
    }
}
