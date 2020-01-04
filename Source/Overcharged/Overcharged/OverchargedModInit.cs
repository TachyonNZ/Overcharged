// OverchargedModInit.cs created by Iron Wolf for Overcharged on 01/04/2020 8:27 AM
// last updated 01/04/2020  8:27 AM

using System.Reflection;
using Harmony;
using Verse;

namespace Overcharged
{
    [StaticConstructorOnStartup]
    public static class OverchargedModInit
    {
        static OverchargedModInit()
        {
            var harmony = HarmonyInstance.Create("com.github.overcharged");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}