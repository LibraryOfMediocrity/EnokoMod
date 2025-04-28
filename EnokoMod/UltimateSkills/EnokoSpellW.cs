using EnokoMod.BattleActions;
using EnokoMod.Cards.Templates;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoLEntitySideloader.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace EnokoMod.UltimateSkills
{
    public sealed class EnokoUltWDef : EnokoUltTemplate
    {
        public override UltimateSkillConfig MakeConfig()
        {
            UltimateSkillConfig config = GetDefaulUltConfig();
            config.Damage = 10;
            config.Value1 = 2;
            config.Value2 = 2;
            config.RelativeEffects = new List<string>() { "TrapCardDisc", "Trigger" };
            return config;
        }
    }

    [EntityLogic(typeof(EnokoUltWDef))]
    public sealed class EnokoUltW : UltimateSkill
    {
        public EnokoUltW()
        {
            TargetType = TargetType.SingleEnemy;
            GunName = "Simple2";
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector)
        {
            yield return PerformAction.Spell(base.Battle.Player, "EnokoUltW");
            EnemyUnit enemy = selector.GetEnemy(Battle);
            yield return new DamageAction(Owner, enemy, Damage, GunName, GunType.Single);
            yield return new DamageAction(Owner, enemy, Damage, GunName, GunType.Single);
            IEnumerable<Card> cards = base.Battle.HandZone.Where((Card card) => card is TrapCard);
            if (cards.Any()) {
                SelectHandInteraction interaction = new SelectHandInteraction(0, 1, cards);
                yield return new InteractionAction(interaction);
                if (interaction.SelectedCards[0] != null) {
                    TrapCard card = interaction.SelectedCards[0] as TrapCard;
                    for (int i = 0; i < Value2; i++)
                    {
                        yield return new TriggerTrapAction(card, selector.SelectedEnemy);
                    }
                }
            }
            yield break;
        }
    }
}
