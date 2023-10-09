using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using Verse;

namespace Ascension
{

    public class HediffComp_PseudoImmortalHeal : HediffComp
    {
        //heals very common bad hediffs (malnutrition, bloodloss) first before the actual heal to prevent them from preventing the heal from fixing other hediffs.
        private static void PreHeal(Pawn pawn)
        {
            AscensionSettings settings = LoadedModManager.GetMod<AscensionMod>().GetSettings<AscensionSettings>();
            int healTimes = 1;
            List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
            for (int i = 0; i < hediffs.Count; i++)
            {

                Hediff hediff_Injury = hediffs[i];
                //we dont want to remove a hediff that has already been removed, and we dont care about visibility as we know what we are removing.
                if (hediff_Injury != null)
                {
                    if (hediff_Injury.def.defName == "Malnutrition" || hediff_Injury.def.defName == "BloodLoss")
                    {
                        if (settings.logHealsBool)
                        {
                            Log.Message($"Prehealed: {hediff_Injury.def.defName} on {pawn.LabelShort}");
                        }
                        pawn.health.RemoveHediff(hediff_Injury);
                    }

                    if (hediff_Injury.def.defName == "RegenerationComa")
                    {
                        if (!pawn.health.ShouldBeDead())
                        {
                            if (settings.logHealsBool)
                            {
                                Log.Message($"Prehealed: {hediff_Injury.def.defName} on {pawn.LabelShort}");
                            }
                            pawn.health.RemoveHediff(hediff_Injury);
                        }
                    }
                }
            }
        }


        private int ticksToHeal;  

        public static readonly int[] tickRates = { 420000, 240000, 120000, 60000, 15000, 2500};
        AscensionSettings settings = LoadedModManager.GetMod<AscensionMod>().GetSettings<AscensionSettings>();
        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            this.ticksToHeal--;
            if (parent.Severity >= 2 && parent.Severity < 7)
            {
                if (this.ticksToHeal > tickRates[((int)Math.Floor(this.parent.Severity) - 1)])
                {
                    if (settings.logHealsBool)
                    {
                        Log.Message($"Reseting heal time due to tier increase on {Pawn.LabelShort}");
                    }
                    this.Reset();
                }
            }

            if (this.ticksToHeal <= 0)
            {
                PreHeal(this.Pawn);
                HealthUtility.FixWorstHealthCondition(Pawn);
                if (settings.logHealsBool)
                {
                    Log.Message($"Healed pawn {Pawn.LabelShort}");
                }
                this.Reset();
            }
        }

        private void Reset()
        {
            if (parent.Severity > 76.99)
            {
                ticksToHeal = 42;
            } else if (parent.Severity < 2)
            {
                ticksToHeal = tickRates[0];
            }
            else if (parent.Severity >= 2 && parent.Severity < 7)
            {
                //first array element is 0 so we -1 the severity to get it in line with the array index
                ticksToHeal = tickRates[((int)Math.Floor(this.parent.Severity)-1)];
            } else if (parent.Severity >= 7 && parent.Severity < 77)
            {
                ticksToHeal = tickRates[5];
            }
            if (settings.logHealsBool)
            {
                Log.Message($"Next heal will take {ticksToHeal / 2500} hours.");
            }
        }

        public override void CompExposeData()
        {
            Scribe_Values.Look<int>(ref this.ticksToHeal, "ticksToHeal", 0, false);
        }

        public override string CompDebugString()
        {
            return "ticksToHeal: " + this.ticksToHeal;
        }
    }
}