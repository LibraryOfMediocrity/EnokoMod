using HarmonyLib;
using LBoL.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnokoMod.BattleActions
{
    [HarmonyPatch]
    class EnokoGameEvents
    {
        static public GameEvent<TriggerTrapEventArgs> PreTriggerEvent { get; set; }
        static public GameEvent<TriggerTrapEventArgs> PostTriggerEvent { get; set; }

        [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.EnterBattle))]
        private static bool Prefix(GameRunController __instance)
        {
            //UnityEngine.Debug.Log("New Custom Events Loaded");
            PreTriggerEvent = new GameEvent<TriggerTrapEventArgs>();
            PostTriggerEvent = new GameEvent<TriggerTrapEventArgs>();
            return true;
        }
    }
}
