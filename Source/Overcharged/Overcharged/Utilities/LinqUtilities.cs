// LinqUtilities.cs created by Iron Wolf for Overcharged on 01/04/2020 8:34 AM
// last updated 01/04/2020  8:34 AM

using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Verse;

namespace Overcharged.Utilities
{
    public static class LinqUtilities
    {
        [CanBeNull]
        public static LightningRodBase GetStrikeRod([NotNull] this IEnumerable<LightningRodBase> rods, IntVec3 startLocation)
        {
            if (rods == null) throw new ArgumentNullException(nameof(rods));
            foreach (LightningRodBase lightningRod in rods)
            {
                if(!lightningRod.CanDivert) continue;
                var dist = lightningRod.Position.DistanceToSquared(startLocation);
                var range = lightningRod.Range;
                if (dist < range * range) //just return the first one that's in range 
                {
                    return lightningRod;
                }
            }

            return null; 
        }
    }
}