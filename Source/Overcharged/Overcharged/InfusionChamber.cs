// InfusionChamber.cs created by Iron Wolf for Overcharged on 01/05/2020 6:43 PM
// last updated 01/05/2020  6:43 PM

using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace Overcharged
{
    public class InfusionChamber : Building_WorkTable, ILightningReceiverThing,  IThingHolder
    {
        private CompAffectedByFacilities _comp;

        
        private ThingOwner _container;

        public InfusionChamber()
        {
            _container = new ThingOwner<Thing>(this);
        }

        public bool CurrentlyUsableForBillsReal()
        {
            return _container.Count == 0; 
        }


        /// <summary>Strikes this instance with the specified energy.</summary>
        /// <param name="energy">The energy.</param>
        public virtual void Strike(int energy)
        {
            IEnumerable<ThingComp> comps = AllComps ?? Enumerable.Empty<ThingComp>();
            foreach (ILightningReceiver lightningReceiver in comps.OfType<ILightningReceiver>()) lightningReceiver.Strike(energy);
        }

        public void GetChildHolders(List<IThingHolder> outChildren)
        {
            ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, GetDirectlyHeldThings());
        }

        public ThingOwner GetDirectlyHeldThings()
        {
            return _container;
        }

        private CompAffectedByFacilities Comp => _comp ?? (_comp = GetComp<CompAffectedByFacilities>());

        public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
        {
            base.DeSpawn(mode);
            IEnumerable<Thing> facilities = Comp?.LinkedFacilitiesListForReading ?? Enumerable.Empty<Thing>();
            foreach (LightningRodBase lightningRodBase in facilities.OfType<LightningRodBase>())
                if (lightningRodBase.IsLinkedTo(this))
                    lightningRodBase.Notify_ReceiverRemoved(this);
        }

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Deep.Look(ref _container, "container", this);
        }

        public override void Tick()
        {
            base.Tick();

            if (this.IsHashIntervalTick(45))
            {
                IEnumerable<Thing> facilities = Comp?.LinkedFacilitiesListForReading ?? Enumerable.Empty<Thing>();
                foreach (LightningRodBase lightningRodBase in facilities.OfType<LightningRodBase>())
                {
                    if (lightningRodBase.IsLinkedTo(this)) continue;

                    lightningRodBase.Notify_ReceiverLinked(this);
                }
            }
        }
    }
}