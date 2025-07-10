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
using EnokoMod.BattleActions;
using LBoL.Core.StatusEffects;
using EnokoMod.StatusEffects;

namespace EnokoMod.Cards
{
    public sealed class EnokoBunkerTrapDef : TrapTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetTrapDefaultConfig();
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Defense;
            config.Colors = new List<ManaColor>() { ManaColor.White, ManaColor.Black };
            config.Cost = new ManaGroup() { Any = 2 };
            config.Value1 = 1;
            config.UpgradedValue1 = 2;
            config.Block = 7;
            config.TargetType = TargetType.Self;
            config.RelativeKeyword = Keyword.Block;
            config.UpgradedRelativeKeyword = Keyword.Block;
            config.Index = CardIndexGenerator.GetUniqueIndex(config, true);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoBunkerTrapDef))]
    public sealed class EnokoBunkerTrap : TrapCard
    {
        public int TriggerBlock 
        {  
            get
            {
                return (base.Battle != null? base.Battle.ExileZone.Count * Value1 : 0) + 7;
            }
        }


        public override IEnumerable<BattleAction> OnTurnEndingInHand()
        {
            yield return new TriggerTrapAction(this);
            yield break;
        }

        public override IEnumerable<BattleAction> TrapTriggered(Unit[] units)
        {
            if(TriggerBlock > 0) yield return new CastBlockShieldAction(Battle.Player, TriggerBlock, 0, cast: false);
            yield break;
        }

    }
}
