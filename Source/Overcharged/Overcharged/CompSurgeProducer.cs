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
	public class CompSurgeProducer : ThingComp
	{
		private CompProperties_SurgeProducer properties
		{
			get
			{
				return (CompProperties_SurgeProducer)this.props;
			}
		}
		public float surgeMitigation
		{
			get
			{
				return properties.surgeMitigation;
			}
		}

		private CompFlickable compFlickable;

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			compFlickable = parent.TryGetComp<CompFlickable>();
		}

		public float ProduceThing(float amount)
		{
			if (compFlickable != null)
				{
					compFlickable.ResetToOn();
					compFlickable.DoFlick();
					FlickUtility.UpdateFlickDesignation(parent);
					return surgeMitigation;
				}
			return 0.0f;
		}
	}
}
