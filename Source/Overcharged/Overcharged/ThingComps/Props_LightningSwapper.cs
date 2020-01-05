// Props_LightningSwapper.cs created by Iron Wolf for Overcharged on 01/05/2020 9:51 AM
// last updated 01/05/2020  9:51 AM

using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Overcharged.Utilities;
using Verse;

namespace Overcharged.ThingComps
{
    public class Props_LightningSwapper : ThingPropertiesBase<LightningSwapper>
    {
        public class Entry
        {
            public ThingDef uncharged;
            public ThingDef charged; 
        }

        public List<Entry> entries;

        [Unsaved] private Dictionary<ThingDef, ThingDef> _lookupDict;

        [CanBeNull]
        public ThingDef GetChargedProduct([NotNull] ThingDef unchargedThing)
        {
            if (unchargedThing == null) throw new ArgumentNullException(nameof(unchargedThing));
            return _lookupDict.TryGetValue(unchargedThing); 
        }


        public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
        {
            _lookupDict = new Dictionary<ThingDef, ThingDef>();
            foreach (string configError in base.ConfigErrors(parentDef))
            {
                yield return configError; 
            }

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

        }
    }

    public class LightningSwapper : ThingCompBase<Props_LightningSwapper> 
    {

    }

}