// SimpleLightningRod.cs created by Iron Wolf for Overcharged on 01/04/2020 7:58 AM
// last updated 01/04/2020  7:58 AM

namespace Overcharged
{
    /// <summary>
    /// LightningRodBase class for a simple lightning rod that just diverts lightning 
    /// </summary>
    /// <seealso cref="Overcharged.LightningRodBase" />
    public class SimpleLightningRod : LightningRodBase
    {
        /// <summary>Gets a value indicating whether this instance can currently divert lightning.</summary>
        /// <value><c>true</c> if this instance can currently divert lightning; otherwise, <c>false</c>.</value>
        public override bool CanDivert => true;
    }
}