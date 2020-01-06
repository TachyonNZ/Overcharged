// WorkGiver_DoBillFixed.cs created by Iron Wolf for Overcharged on 01/05/2020 9:03 PM
// last updated 01/05/2020  9:03 PM

using RimWorld;
using Verse;
using Verse.AI;

namespace Overcharged
{
    public class WorkGiver_DoBillFixed : WorkGiver_DoBill
    {
        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            if (t is IBillGiver giver)
            {
                if (!giver.CurrentlyUsableForBills()) return false; 
            }
            return base.HasJobOnThing(pawn, t, forced);
        }

    }
}