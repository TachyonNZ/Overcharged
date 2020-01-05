// LightningPatch.cs created by Iron Wolf for Overcharged on 01/04/2020 8:24 AM
// last updated 01/04/2020  8:24 AM

using System;
using Harmony;
using Overcharged.Utilities;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Overcharged.HPatches
{
    static class LightningPatch
    {
        private const float EPSILON = 0.1f; 


        [HarmonyPatch(typeof(WeatherEvent_LightningStrike))]
        [HarmonyPatch(nameof(WeatherEvent_LightningStrike.FireEvent))]
        static class LightningStrike_FireEventPatch
        {
            [HarmonyPrefix]
            static bool NewStrike(ref IntVec3 ___strikeLoc, Map ___map, ref Mesh ___boltMesh)
            {
                if (___map == null) return true;
                var manager = ___map.GetComponent<LightningRodManager>();
                if (manager?.AnyActiveRods != true) return true;
                SoundDefOf.Thunder_OffMap.PlayOneShotOnCamera(___map); //from WeatherEvent_LightningFlash 

                if (!___strikeLoc.IsValid)
                {
                    ___strikeLoc = CellFinderLoose.RandomCellWith(sq => sq.Standable(___map) && !___map.roofGrid.Roofed(sq), ___map);
                }
                ___boltMesh = LightningBoltMeshPool.RandomBoltMesh;

                var rod = manager.LightningRods.GetStrikeRod(___strikeLoc);
                float radius = 1.9f;
                if (rod != null)
                {
                    radius = radius * (1 - Mathf.Clamp(rod.DamageMitigationFactor,0,1));
                    ___strikeLoc = rod.Position; 
                }
                if(radius > EPSILON)
                    GenExplosion.DoExplosion(___strikeLoc, ___map, radius, DamageDefOf.Flame, null);
                Vector3 loc = ___strikeLoc.ToVector3Shifted();

                rod?.Strike(1); 

                for (int i = 0; i < 4; i++)
                {
                    MoteMaker.ThrowSmoke(loc, ___map, 1.5f);
                    MoteMaker.ThrowMicroSparks(loc, ___map);
                    MoteMaker.ThrowLightningGlow(loc, ___map, 1.5f);
                }

                SoundInfo info = SoundInfo.InMap(new TargetInfo(___strikeLoc, ___map));
                SoundDefOf.Thunder_OnMap.PlayOneShot(info);
                return false; 
            }


        }

    }
}