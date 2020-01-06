// InfusionChamber.cs created by Iron Wolf for Overcharged on 01/05/2020 6:43 PM
// last updated 01/05/2020  6:43 PM

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using RimWorld;
using Verse;

namespace Overcharged
{
    public class InfusionChamber : Building_WorkTable, ILightningReceiverThing, IThingHolder
    {
        private readonly List<Thing> _scratchList = new List<Thing>();
        private CompAffectedByFacilities _comp;

        private RecipeDef _stored;
        private ThingOwner _container;

        public InfusionChamber()
        {
            _container = new ThingOwner<Thing>(this, false);
        }

        /// <summary>Strikes this instance with the specified energy.</summary>
        /// <param name="energy">The energy.</param>
        public virtual void Strike(int energy)
        {
            Log.Message($"{_container.Count} for {Label}");

            IEnumerable<ThingComp> comps = AllComps ?? Enumerable.Empty<ThingComp>();
            foreach (ILightningReceiver lightningReceiver in comps.OfType<ILightningReceiver>()) lightningReceiver.Strike(energy);

            if (_stored != null && _container.Count > 0) MakeChargedThing();
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

        public bool CurrentlyUsableForBillsReal()
        {
            return _container.Count == 0;
        }

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

        public void LoadThings([NotNull] IEnumerable<Thing> things, [NotNull] RecipeDef recipe)
        {
            if (things == null) throw new ArgumentNullException(nameof(things));
            if (recipe == null) throw new ArgumentNullException(nameof(recipe));

            Log.Message($"loading {recipe.defName} in {Label}");

            foreach (Thing thing in things) _container.TryAdd(thing);

            _stored = recipe;
        }

        public override string GetInspectString()
        {
            if (_stored == null) return base.GetInspectString();
            StringBuilder builder = new StringBuilder(); 
            builder.AppendLine(base.GetInspectString());
            builder.Append("InfuserStorage".Translate(_stored.label));
            return builder.ToString();
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

        private void MakeChargedThing()
        {
            _scratchList.Clear();
            _scratchList.AddRange(_container);
            _container.TryDropAll(Position, Map, ThingPlaceMode.Direct); //drop it first so we can safely destroy the contents 
            var ext = _stored.GetModExtension<InfuserRecipeExtension>();
            if (ext == null)
            {
                Log.Error($"recipe def {_stored.defName} does not have an {nameof(InfuserRecipeExtension)}");
                return;
            }

            ThingDef stuff;
            if (ext.chargedThing.MadeFromStuff) //transfer the stuff from the uncharged item to the charged item 
            {
                stuff = null;
                foreach (Thing thing in _scratchList)
                {
                    if (!thing.def.MadeFromStuff) continue;
                    stuff = thing.Stuff;
                    break;
                }
            }
            else
            {
                stuff = null;
            }

            foreach (Thing thing in _scratchList) thing.Destroy();
            _scratchList.Clear();
            IntermittentLightningSprayer.ThrowMagicPuffUp(Position.ToVector3(), Map);
            Thing newThing = ThingMaker.MakeThing(ext.chargedThing, stuff);
            newThing.stackCount = ext.count;
            GenPlace.TryPlaceThing(newThing, Position, Map, ThingPlaceMode.Near);
        }


        private void SwapAndDestroyThing(Thing thing, ThingDef swappedThingDef)
        {
            ThingDef stuff = thing.def.MadeFromStuff ? null : thing.Stuff;
            Thing spawnThing = ThingMaker.MakeThing(swappedThingDef, stuff);

            var qualityComp = thing.TryGetComp<CompQuality>();
            if (qualityComp != null)
            {
                var sQComp = spawnThing.TryGetComp<CompQuality>();
                sQComp?.SetQuality(qualityComp.Quality, ArtGenerationContext.Colony);
            }

            spawnThing.stackCount = thing.stackCount;

            GenPlace.TryPlaceThing(spawnThing, Position, Map, ThingPlaceMode.Near);


            thing.Destroy();
        }
    }
}