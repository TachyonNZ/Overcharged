// LightningRodBase.cs created by Iron Wolf for (Overcharged) on 01/03/2020 1:39 PM
// last updated 01/03/2020  2:21 PM

using System.Linq;
using Verse;

namespace Overcharged
{
    /// <summary>
    ///     abstract base class for lightning rods
    /// </summary>
    /// <seealso cref="Verse.ThingWithComps" />
    public abstract class LightningRodBase : ThingWithComps
    {
        private float? _range;

        /// <summary>Gets the range.</summary>
        /// <value>The range.</value>
        public virtual float Range
        {
            get
            {
                if (_range == null)
                    _range = def.comps.OfType<ThingCompProperties_LightningRodRangeBoost>().Select(c => c.rangeBoost)
                        .Sum();

                return _range.Value;
            }
        }


        /// <summary>Gets a value indicating whether this instance can currently divert lightning.</summary>
        /// <value><c>true</c> if this instance can currently divert lightning; otherwise, <c>false</c>.</value>
        public abstract bool CanDivert { get; }


        /// <summary>Strikes this instance with the specified energy</summary>
        /// <param name="energy">The energy.</param>
        public void Strike(int energy)
        {
            OnStrike(energy);
            foreach (var receiverComp in AllComps.OfType<ILightningReceiverComp>()) receiverComp.Strike(energy);
        }

        protected virtual void OnStrike(int energy)
        {
        }
    }

    /// <summary>
    ///     interface for thing comps that can receive lightning from a lightning rod
    /// </summary>
    public interface ILightningReceiverComp
    {
        /// <summary>Strikes this instance with the specified energy.</summary>
        /// <param name="energy">The energy.</param>
        void Strike(int energy);
    }
}