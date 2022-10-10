using Verse;
using RimWorld;

namespace Ascension
{
	public class CompUseEffect_Absorb_SpiritSword : CompUseEffect
	{
		public CompProperties_UseEffect_Absorb_SpiritSword Props
		{
			get
			{
				return (CompProperties_UseEffect_Absorb_SpiritSword)this.props;
			}
		}

		//all this class does is let the pawn "absorb" the spirit sword, giving them a hediff. there might be remenants of an attempt to add abilities in the code
		public bool RecentlyFused = false;
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			usedBy.health.AddHediff(AscensionDefOf.SpiritSwordFusion);
			RecentlyFused = true;
			if (PawnUtility.ShouldSendNotificationAbout(usedBy))
			{
				Messages.Message("SpiritSwordFused".Translate(usedBy.Named("USER")), usedBy, MessageTypeDefOf.PositiveEvent, true);
			}
		}

		public override bool CanBeUsedBy(Pawn p, out string failReason)
		{
			if (p.health.hediffSet.HasHediff(HediffDef.Named("SpiritSwordFusion")))
			{
				failReason = "SpiritSwordAlreadyFused".Translate(p.Named("USER"));
				return false;
			}
			return base.CanBeUsedBy(p, out failReason);
		}

		public override TaggedString ConfirmMessage(Pawn p)
		{
			Hediff firstHediffOfDef = p.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicAmplifier, false);
			if (firstHediffOfDef == null)
			{
				return null;
			}
			return null;
		}
	}
}