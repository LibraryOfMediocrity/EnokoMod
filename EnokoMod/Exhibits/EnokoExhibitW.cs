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
            exhibitConfig.BaseManaColor = ManaColor.White;
            return exhibitConfig;
        }
    }

    [EntityLogic(typeof(EnokoExhibitWDef))]
    public sealed class EnokoExhibitW : ShiningExhibit
    {
        
    }
}
