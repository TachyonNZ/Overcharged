// RecipeWoker_Preserve.cs created by Iron Wolf for Overcharged on 01/05/2020 8:54 PM
// last updated 01/05/2020  8:54 PM

using Verse;

namespace Overcharged
{
    public class RecipeWorker_Preserve : RecipeWorker
    {
        public override void ConsumeIngredient(Thing ingredient, RecipeDef recipe, Map map)
        {
            ingredient.DeSpawn(); 
        }
    }
}