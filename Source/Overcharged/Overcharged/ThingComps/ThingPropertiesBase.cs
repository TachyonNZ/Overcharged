// ThingPropertiesBase.cs created by Iron Wolf for Overcharged on 01/05/2020 9:51 AM
// last updated 01/05/2020  9:51 AM

using System;
using Verse;

namespace Overcharged.ThingComps
{
    public class ThingPropertiesBase<T> : CompProperties where T: ThingComp 
    {
        

        public ThingPropertiesBase()
        {
            compClass = typeof(T); 
        }
    }

    public class ThingCompBase<T> : ThingComp where T: CompProperties
    {
        protected T Props
        {
            get
            {
                try
                {
                    return (T) props; 
                }
                catch (InvalidCastException e)
                {
                    throw new
                        InvalidCastException($"trying to cast {props?.GetType().Name ?? "NULL"} to {typeof(T).Name} encountered exception {e.GetType().Name}",
                                             e);
                }
            }
        }
    }
}