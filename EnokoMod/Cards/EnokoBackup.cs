using EnokoMod.Cards.Templates;
using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;

namespace EnokoMod.Cards
{
    public sealed class EnokoBackupDef : EnokoCardTemplate
    {
        public override CardConfig MakeConfig()
        {
            CardConfig config = GetCardDefaultConfig();
            config.Rarity = Rarity.Uncommon;
            config.Type = CardType.Attack;
            config.Colors = new List<ManaColor>() { ManaColor.White, ManaColor.Green };
            config.Cost = new ManaGroup() { Hybrid = 2, HybridColor = 3 };
            config.Damage = 7;
            config.Value1 = 3;
            config.TargetType = TargetType.Nobody;
            config.Keywords = Keyword.Exile;
            config.UpgradedKeywords = Keyword.Exile | Keyword.Echo;
            config.Index = CardIndexGenerator.GetUniqueIndex(config);
            return config;
        }
    }

    [EntityLogic(typeof(EnokoBackupDef))]
    public sealed class EnokoBackup : BurialCard
    {
        public override int CardIndex => 5;

        protected override void OnEnterBattle(BattleController battle)
        {
            Activated = false;
            base.ReactBattleEvent<DamageEventArgs>(Battle.Player.DamageTaking, OnDamageTaking);
            base.ReactBattleEvent<DamageEventArgs>(Battle.Player.DamageReceived, OnDamageReceived);
        }

        private bool Activated;

        private IEnumerable<BattleAction> OnDamageReceived(DamageEventArgs args)
        {
            if(IsBuried && Activated)
            {
                for (int i = 0; i < Value1; i++)
                {
                    yield return AttackAction(args.Source);
                }
                yield return new MoveCardToDrawZoneAction(this, DrawZoneTarget.Random);
                Activated = false;
            }
            yield break;
        }

        private IEnumerable<BattleAction> OnDamageTaking(DamageEventArgs args)
        {
            if (IsBuried && args.Source != Battle.Player && args.Source.IsAlive && args.DamageInfo.DamageType == DamageType.Attack && !args.DamageInfo.ZeroDamage)
            {
                // test interactions with graze in case of weird order
                int num = args.DamageInfo.Damage.RoundToInt();
                if (num > 0)
                {
                    Indicator.NotifyActivating();
                    args.DamageInfo = args.DamageInfo.ReduceActualDamageBy(num);
                    args.AddModifier(this);
                }
                Activated = true;
            }
            yield break;
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield break;
        }
    }
}
