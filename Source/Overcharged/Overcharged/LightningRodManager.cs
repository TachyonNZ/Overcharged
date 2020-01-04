// LightningRodManager.cs created by Iron Wolf for Overcharged on 01/04/2020 7:51 AM
// last updated 01/04/2020  7:51 AM

using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Overcharged
{
    public class LightningRodManager : MapComponent
    {
        private List<LightningRodBase> _lightningRods = new List<LightningRodBase>();

        public LightningRodManager(Map map) : base(map)
        {
        }

        public IEnumerable<LightningRodBase> LightningRods => _lightningRods ?? (_lightningRods = new List<LightningRodBase>());

        public void DeRegister(LightningRodBase lightningRod)
        {
            _lightningRods.Remove(lightningRod);
        }

        public bool AnyActiveRods
        {
            get
            {
                foreach (LightningRodBase lightningRodBase in LightningRods)
                {
                    if (lightningRodBase.CanDivert) return true; 
                }

                return false; 
            }
        } 


        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref _lightningRods, nameof(_lightningRods), LookMode.Reference);
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
                _lightningRods = _lightningRods ?? new List<LightningRodBase>();
        }

        public void Register(LightningRodBase lightningRod)
        {
            if (!_lightningRods.Contains(lightningRod))
                _lightningRods.Add(lightningRod);
        }
    }
}