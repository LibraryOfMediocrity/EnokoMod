using EnokoMod.Cards.Templates;
using EnokoMod.StatusEffects;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace EnokoMod.Cards
{
    public sealed class EnokoEternalDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Rare;
            config.Type = CardType.Defense;
            config.Colors = new List<ManaColor>() { ManaColor.Black };
            config.Cost = new ManaGroup() { Black = 1 };
            config.Shield = 10;
            config.Value1 = 2;
            config.UpgradedValue1 = 3;
            config.Value2 = 2;
            config.UpgradedValue2 = 3;
            config.Mana = new ManaGroup() { Any = 1 };
            config.IsXCost = true;
            config.Keywords = Keyword.Exile;
            config.UpgradedKeywords = Keyword.Exile;
            config.RelativeKeyword = Keyword.Shield;
            config.UpgradedRelativeKeyword = Keyword.Shield;
            config.RelativeEffects = new List<string>() { nameof(EnokoConstrainSe) };
            config.UpgradedRelativeEffects = config.RelativeEffects; 
            config.TargetType = TargetType.Self;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoEternalDef))]
    public sealed class EnokoEternal : Card
    {

        public ShieldInfo Barrier
        {
            get {
                ManaGroup? manaGroup = base.PendingManaUsage;
                if (manaGroup != null)
                {
                    ManaGroup valueOrDefault = manaGroup.GetValueOrDefault();
                    return new ShieldInfo(this.RawShield + (base.SynergyAmount(valueOrDefault, ManaColor.Any, 1) - 1) * base.Value1, BlockShieldType.Normal);
                }
                return new ShieldInfo(this.RawShield, BlockShieldType.Normal);
            }
        }

        public override int AdditionalValue2
        {
            get
            {
                ManaGroup? manaGroup = base.PendingManaUsage;
                if (manaGroup != null)
                {
                    ManaGroup valueOrDefault = manaGroup.GetValueOrDefault();
                    return base.SynergyAmount(valueOrDefault, ManaColor.Any, 1) - 1;
                }
                return 0;
            }
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new CastBlockShieldAction(Battle.Player, Barrier);
            foreach(BattleAction action in DebuffAction<EnokoRetainConstrainSe>(Battle.AllAliveEnemies, Value2))
            {
                yield return action;
            }
            yield break;
        }
    }
}
