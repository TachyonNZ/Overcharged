// BillPatches.cs created by Iron Wolf for Overcharged on 01/05/2020 7:46 PM
// last updated 01/05/2020  7:46 PM

using System.Linq;
using Harmony;
using RimWorld;
using Verse;

namespace Overcharged.HPatches
{
    static class BillPatches
    {
        [HarmonyPatch(typeof(BillUtility)), HarmonyPatch(nameof(BillUtility.MakeNewBill))]
        static class MakeBillPatch
        {
            static bool Prefix(RecipeDef recipe, ref Bill __result)
            {
                if (recipe.HasModExtension<InfuserRecipeExtension>())
                {
                    __result = new LoadInfuserBill(recipe);
                    return false; 
                }

                return true; 
            }
        }
    }
}