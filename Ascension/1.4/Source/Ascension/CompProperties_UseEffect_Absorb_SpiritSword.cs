using System;
using Verse;
using RimWorld;

namespace Ascension
{
	// Token: 0x020014F0 RID: 5360
	public class CompProperties_UseEffect_Absorb_SpiritSword : CompProperties_UseEffect
	{
		public CompProperties_UseEffect_Absorb_SpiritSword()
		{
			this.compClass = typeof(CompUseEffect_Absorb_SpiritSword);
		}

		public int swordType;

	}
}
