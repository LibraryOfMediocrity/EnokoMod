using EnokoMod.Cards.Templates;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoLEntitySideloader.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using LBoL.Core.Units;
using UnityEngine;
using EnokoMod.TrapToolBox;
using EnokoMod.BattleActions;
using LBoL.Core.Battle.BattleActions;

namespace EnokoMod.Cards
{
    public sealed class EnokoWeaponDef : TrapTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetTrapDefaultConfig();
            config.IsPooled = true;
            config.Damage = 10;
            config.Block = 20;
            config.UpgradedBlock = 30;
            config.Value1 = 3;
            config.UpgradedValue1 = 4;
            config.Rarity = Rarity.Rare;
            config.Keywords = Keyword.Retain | Keyword.Forbidden | Keyword.Accuracy;
            config.UpgradedKeywords = Keyword.Retain | Keyword.Forbidden | Keyword.Accuracy;
            config.RelativeKeyword = Keyword.Block;
            config.UpgradedRelativeKeyword = Keyword.Block;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoWeaponDef))]
    public sealed class EnokoWeapon : TrapCard
    {

        public override IEnumerable<BattleAction> TrapTriggered(Unit[] selector)
        {
            for (int i = 0; i < Value1; i++)
            {
                yield return AttackAllAliveEnemyAction();
            }
            yield return DefenseAction();
            yield break;
        }

    }
}
