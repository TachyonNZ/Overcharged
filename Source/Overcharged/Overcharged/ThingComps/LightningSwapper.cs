// LightningSwapper.cs created by Iron Wolf for Overcharged on //2020 
// last updated 01/05/2020  10:58 AM

using System;
using Overcharged.Utilities;
using RimWorld;
using Verse;

namespace Overcharged.ThingComps
{
    public class LightningSwapper : ThingCompBase<Props_LightningSwapper>, ILightningReceiver
    {
        /// <summary>Strikes this instance with the specified energy.</summary>
        /// <param name="energy">The energy.</param>
        public void Strike(int energy)
        {
            //ignore energy for now 
            foreach (Thing thing in StorageBuilding.GetAllStoredThings())
            {
                SwapAndDestroyThing(thing);
            }
        }

        private Building_Storage StorageBuilding
        {
            get
            {
                try
                {
                    return (Building_Storage) parent;
                }
                catch (InvalidCastException e)
                {
                    throw new
                        InvalidCastException($"could not convert type {parent.GetType().Name} to {nameof(Building_Storage)}", e);
                }
            }
        }


        private void SwapAndDestroyThing(Thing thing)
        {
            ThingDef swapThingDef = Props.GetChargedProduct(thing.def);
            if (swapThingDef != null)
            {
                ThingDef stuff = thing.def.MadeFromStuff ? null : thing.Stuff;
                Thing spawnThing = ThingMaker.MakeThing(swapThingDef, stuff);

                var qualityComp = thing.TryGetComp<CompQuality>();
                if (qualityComp != null)
                {
                    var sQComp = spawnThing.TryGetComp<CompQuality>();
                    sQComp?.SetQuality(qualityComp.Quality, ArtGenerationContext.Colony);
                }

                spawnThing.stackCount = thing.stackCount;

                GenPlace.TryPlaceThing(spawnThing, parent.Position, parent.Map, ThingPlaceMode.Near);
            }

            if (!Props.IsImmuneFromDestruction(thing.def))
                thing.Destroy();
        }
    }
}