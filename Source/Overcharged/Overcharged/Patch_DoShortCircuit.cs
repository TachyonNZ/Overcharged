using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Harmony;
using UnityEngine;
using Verse;
using RimWorld;
using RT_Fuse;

namespace Overcharged
{
	[HarmonyPatch(typeof(ShortCircuitUtility))]
	[HarmonyPatch("DoShortCircuit")]
	static class Patch_DoShortCircuit
	{
		private static MethodInfo tryStartFireNearMethodInfo = AccessTools.Method(typeof(ShortCircuitUtility), "TryStartFireNear");

		private static bool TryStartFireNear(Building culprit)
		{
			return (bool)tryStartFireNearMethodInfo.Invoke(null, new object[] { culprit });
		}

		static bool Prefix(Building culprit)
		{
			PowerNet powerNet = culprit.PowerComp.PowerNet;
			Map map = culprit.Map;
			float totalEnergy = 0f;
			float totalEnergyHistoric = 0f;

			if (powerNet.batteryComps.Any((CompPowerBattery x) => x.StoredEnergy > 20f))
			{
				foreach (CompPowerBattery batteryComp in powerNet.batteryComps)
				{
					totalEnergy += batteryComp.StoredEnergy;
					batteryComp.DrawPower(batteryComp.StoredEnergy);
				}
				totalEnergyHistoric = totalEnergy;
				foreach (CompPower transmitter in powerNet.transmitters)
				{
					CompSurgeProducer surgeProd = transmitter.parent.GetComp<CompSurgeProducer>();
					if (surgeProd != null)
					{
						totalEnergy -= surgeProd.ProduceThing(totalEnergy);
						if (totalEnergy <= 0) break;
					}
				}
			}
			return false;
		}
	}
}
