// ThingCompProperties_LightningRodRangeBoost.cs created by Iron Wolf for (Overcharged) on 01/03/2020 1:52 PM
// last updated 01/03/2020  1:53 PM

using Verse;

namespace Overcharged
{
    /// <summary>Properties for a thing comp that boosts a lightning rods base range</summary>
    /// <seealso cref="Verse.CompProperties" />
    public class ThingCompProperties_LightningRodRangeBoost : CompProperties
    {
        public float rangeBoost;

        public ThingCompProperties_LightningRodRangeBoost()
        {
            compClass = typeof(ThingComp_LightningRodRangeBoost);
        }
    }


    public class ThingComp_LightningRodRangeBoost
    {
    }
}