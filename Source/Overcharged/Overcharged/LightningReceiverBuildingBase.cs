// LightningRecieverBuildingBase.cs created by Iron Wolf for Overcharged on 01/04/2020 7:03 PM
// last updated 01/04/2020  7:03 PM

using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace Overcharged
{
    public class LightningReceiverBuildingBase : Building_WorkTable, ILightningReceiver
    {
        private CompAffectedByFacilities _comp;

        /// <summary>Strikes this instance with the specified energy.</summary>
        /// <param name="energy">The energy.</param>
        public virtual void Strike(int energy)
        {
            var comps = AllComps ?? Enumerable.Empty<ThingComp>();
            foreach (ILightningReceiver lightningReceiver in comps.OfType<ILightningReceiver>())
            {
                lightningReceiver.Strike(energy); 
            }
        }

        private CompAffectedByFacilities Comp => _comp ?? (_comp = GetComp<CompAffectedByFacilities>());

        public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
        {
            base.DeSpawn(mode);
            IEnumerable<Thing> facilities = _comp?.LinkedFacilitiesListForReading ?? Enumerable.Empty<Thing>();
            foreach (LightningRodBase lightningRodBase in facilities.OfType<LightningRodBase>())
                if (lightningRodBase.IsLinkedTo(this))
                    lightningRodBase.Notify_ReceiverRemoved(this);
        }

        public override void Tick()
        {
            base.Tick();

            if (this.IsHashIntervalTick(45))
            {
                IEnumerable<Thing> facilities = _comp?.LinkedFacilitiesListForReading ?? Enumerable.Empty<Thing>();
                foreach (LightningRodBase lightningRodBase in facilities.OfType<LightningRodBase>())
                {
                    if (lightningRodBase.IsLinkedTo(this)) continue;

                    lightningRodBase.Notify_ReceiverLinked(this);
                }
            }
        }
    }
}