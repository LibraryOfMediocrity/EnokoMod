using HarmonyLib;

namespace EnokoMod
{
    public static class PInfo
    {
        // each loaded plugin needs to have a unique GUID. usually author+generalCategory+Name is good enough
        public const string GUID = "goat.lbol.Character.Enoko";
        public const string Name = "Enoko Mod";
        public const string version = "0.0.1";
        public static readonly Harmony harmony = new Harmony(GUID);

    }
}
