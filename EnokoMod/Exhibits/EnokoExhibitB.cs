using LBoL.Base;
using LBoL.ConfigData;
using LBoL.EntityLib.Exhibits;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnokoMod.Exhibits
{
    public sealed class EnokoExhibitBDef : EnokoExhibitTemplate
    {
        public override ExhibitConfig MakeConfig()
        {
            ExhibitConfig exhibitConfig = this.GetDefaultExhibitConfig();
            exhibitConfig.Mana = new ManaGroup() { Black = 1 };
            exhibitConfig.BaseManaColor = ManaColor.Black;
            return exhibitConfig;
        }
    }

    [EntityLogic(typeof(EnokoExhibitBDef))]
    public sealed class EnokoExhibitB : ShiningExhibit
    {

    }
}
