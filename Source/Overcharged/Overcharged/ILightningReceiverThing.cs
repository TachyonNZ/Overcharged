// ILightningRecieverBuilding.cs created by Iron Wolf for Overcharged on 01/05/2020 6:38 PM
// last updated 01/05/2020  6:38 PM

using Verse;

namespace Overcharged
{
    /// <summary>
    /// interface for things that are lightning receivers 
    /// </summary>
    /// <seealso cref="Verse.ILoadReferenceable" />
    /// <seealso cref="Overcharged.ILightningReceiver" />
    public interface ILightningReceiverThing : ILoadReferenceable, ILightningReceiver
    {
        
    }
}