// LoadInfuserBill.cs created by Iron Wolf for Overcharged on 01/05/2020 7:44 PM
// last updated 01/05/2020  7:44 PM

using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace Overcharged
{
    public class LoadInfuserBill : Bill_Production
    {
        public LoadInfuserBill()
        {
        }

        public LoadInfuserBill(RecipeDef recipe) : base(recipe)
        {
        }

        public override void Notify_IterationCompleted(Pawn billDoer, List<Thing> ingredients)
        {
            try
            {
                base.Notify_IterationCompleted(billDoer, ingredients);
                var infuser = (InfusionChamber) billStack.billGiver;
                infuser.LoadThings(ingredients, recipe); 

            }
            catch (InvalidCastException e)
            {
                throw new InvalidCastException($"unable to cast {billStack.billGiver.GetType()} to {nameof(InfusionChamber)}",e);
            }
        }
    }
}