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
            exhibitConfig.Value2 = 3;
            exhibitConfig.HasCounter = true;
            exhibitConfig.InitialCounter = 3;
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
            this.Counter = Value2;
        }

        protected override void OnLeaveBattle()
        {
            this.Counter = Value2;
        }

        private IEnumerable<BattleAction> OnCardExiled(CardEventArgs args)
        {
            if (this.Counter > 0 && args.Card.CardType == CardType.Attack)
            {
                this.NotifyActivating();
                yield return new DrawCardAction();
                Counter--;
            }
            yield break;
        }
    }
}
