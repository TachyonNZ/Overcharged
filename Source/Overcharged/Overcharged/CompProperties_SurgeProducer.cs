using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Verse;
using RimWorld;
using RT_Fuse;

namespace Overcharged
{
    public class CompProperties_SurgeProducer : CompProperties
	{
		public float surgeMitigation = 1000.0f;

		public CompProperties_SurgeProducer()
		{
			compClass = typeof(CompSurgeProducer);
		}
	}
}
