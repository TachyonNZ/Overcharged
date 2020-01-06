﻿// WorktablePatches.cs created by Iron Wolf for Overcharged on 01/05/2020 7:11 PM
// last updated 01/05/2020  7:11 PM

using Harmony;

namespace Overcharged.HPatches
{
    static class WorktablePatches
    {
        [HarmonyPatch(typeof(InfusionChamber)), HarmonyPatch(nameof(InfusionChamber.CurrentlyUsableForBills))]
        static class MakeIsUsableVirtual //this is needed because CurrentlyUsableForBills isn't virtual and the bills tab is hardcoded to use the work table building :V 
        {
            
            [HarmonyPrefix]
            static bool Prefix(InfusionChamber __instance, ref bool __result)
            {
                __result = __instance.CurrentlyUsableForBillsReal();
                return false; 
            }
        }
    }
}