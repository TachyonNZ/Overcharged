// LinqUtilities.cs created by Iron Wolf for Overcharged on 01/04/2020 8:34 AM
// last updated 01/04/2020  8:34 AM

using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Verse;

namespace Overcharged.Utilities
{
    public static class LinqUtilities
    {

        /// <summary>
        /// if the given enumeration is null this returns an empty enumeration, otherwise the input is returned unchanged 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns></returns>
        [NotNull]
        public static IEnumerable<T> MakeSafe<T>([CanBeNull, NoEnumeration] this IEnumerable<T> enumerable)
        {
            return enumerable ?? Enumerable.Empty<T>();
        }


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