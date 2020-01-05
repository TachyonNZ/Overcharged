// Props_LightningSwapper.cs created by Iron Wolf for Overcharged on 01/05/2020 9:51 AM
// last updated 01/05/2020  9:51 AM

using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Overcharged.Utilities;
using RimWorld;
using Verse;

namespace Overcharged.ThingComps
{
    public class Props_LightningSwapper : ThingPropertiesBase<LightningSwapper>
    {
        public List<Entry> entries;

        [Unsaved] private Dictionary<ThingDef, ThingDef> _lookupDict;
        [Unsaved] private HashSet<ThingDef> _immuneSet; 

        public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
        {
            _lookupDict = new Dictionary<ThingDef, ThingDef>();
            foreach (string configError in base.ConfigErrors(parentDef)) yield return configError;

            if (!typeof(Building_Storage).IsAssignableFrom(parentDef.thingClass))
                yield return "parent is not a storage building!";

            foreach (Entry entry in entries.MakeSafe())
            {
                if (entry.charged == null || entry.uncharged == null)
                {
                    yield return "entry contains either a null charged or uncharged field";
                    continue;
                }

                if (_lookupDict.ContainsKey(entry.uncharged))
                {
                    yield return $"duplicate entry for {entry.uncharged.defName}";
                    continue;
                }

                _lookupDict[entry.uncharged] = entry.charged;
            }

            //now check to make sure the comps and such are compatible 
            foreach (KeyValuePair<ThingDef, ThingDef> keyValuePair in _lookupDict)
            {
                if (keyValuePair.Key.HasComp<CompQuality>() && !keyValuePair.Value.HasComp<CompQuality>())
                    yield return
                        $"uncharged {keyValuePair.Key.defName} has quality comp but product {keyValuePair.Value.defName} does not!";
                if (keyValuePair.Key.HasComp<CompArt>() && !keyValuePair.Value.HasComp<CompArt>())
                    yield return
                        $"uncharged {keyValuePair.Key.defName} has art comp but product {keyValuePair.Value.defName} does not!";
                if (keyValuePair.Key.MadeFromStuff && !keyValuePair.Key.MadeFromStuff)
                    yield return
                        $"uncharged {keyValuePair.Key.defName} is made from stuff but product {keyValuePair.Value.defName} is not!";
                else if (!keyValuePair.Key.MadeFromStuff && keyValuePair.Key.MadeFromStuff)
                    yield return
                        $"uncharged {keyValuePair.Key.defName} is not made from stuff but {keyValuePair.Value.defName} is!";
            }

            _immuneSet = new HashSet<ThingDef>();
            foreach (KeyValuePair<ThingDef, ThingDef> keyValuePair in _lookupDict)
            {
                _immuneSet.Add(keyValuePair.Value); 
            }
        }

        public bool IsImmuneFromDestruction(ThingDef def)
        {
            return _immuneSet.Contains(def); 
        }

        [CanBeNull]
        public ThingDef GetChargedProduct([NotNull] ThingDef unchargedThing)
        {
            if (unchargedThing == null) throw new ArgumentNullException(nameof(unchargedThing));
            return _lookupDict.TryGetValue(unchargedThing);
        }

        public class Entry
        {
            public ThingDef uncharged;
            public ThingDef charged;
        }
    }
}