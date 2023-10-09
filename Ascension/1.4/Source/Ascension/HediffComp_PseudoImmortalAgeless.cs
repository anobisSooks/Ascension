using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Ascension
{
    public class HediffComp_PseudoImmortalAgeless : HediffComp
    {

        public override void CompPostMake()
        {
            if (parent.pawn.ageTracker.AgeBiologicalYears > 21)
            {
                this.parent.pawn.ageTracker.AgeBiologicalTicks = 75600000;
                this.parent.pawn.ageTracker.ResetAgeReversalDemand(Pawn_AgeTracker.AgeReversalReason.ViaTreatment, false);
            }
        }
    }
}