// InfuserRecipeExtension.cs created by Iron Wolf for Overcharged on 01/05/2020 7:55 PM
// last updated 01/05/2020  7:55 PM

using System.Collections.Generic;
using Verse;

namespace Overcharged
{
    public class InfuserRecipeExtension : DefModExtension
    {

        public ThingDef chargedThing;
        public int count = 1;
        public override IEnumerable<string> ConfigErrors()
        {
            foreach (string configError in base.ConfigErrors())
            {
                yield return configError;
            }

            if (chargedThing == null)
            {
                yield return $"{nameof(chargedThing)} field is null!"; 
            }
        }
    }
}