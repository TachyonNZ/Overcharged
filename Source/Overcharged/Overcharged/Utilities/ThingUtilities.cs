// ThingUtilities.cs created by Iron Wolf for Overcharged on 01/05/2020 10:43 AM
// last updated 01/05/2020  10:43 AM

using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using RimWorld;
using Verse;

namespace Overcharged.Utilities
{
    public  static class ThingUtilities
    {
        public static bool HasComp<T>([NotNull] this ThingDef thingDef) where T : ThingComp
        {
            if (thingDef == null) throw new ArgumentNullException(nameof(thingDef));
            return thingDef.HasComp(typeof(T));
        }

        public static IEnumerable<Thing> GetAllStoredThings([NotNull] this Building_Storage storageBuilding)
        {
            if (storageBuilding == null) throw new ArgumentNullException(nameof(storageBuilding));
            var things = storageBuilding.slotGroup?.HeldThings;
            return things.MakeSafe(); 
        }
    }
}