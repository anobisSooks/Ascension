using System;
using System.Collections.Generic;
using Verse;
using RimWorld;

namespace Ascension
{
    public class IngestionOutcomeDoer_GoldenPill : IngestionOutcomeDoer
    {
        AscensionSettings settings = LoadedModManager.GetMod<AscensionMod>().GetSettings<AscensionSettings>();
        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested)
        {
            Hediff hediff = HediffMaker.MakeHediff(this.hediffDef, pawn, null);
            float num;

            if (settings.opGoldenPillsBool)
            {
                num = tier;

            } else
            {
                num = this.severity;
            }
            hediff.Severity = num;
            pawn.health.AddHediff(hediff, null, null, null);
        }

        public override IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
        {
            if (parentDef.IsDrug && this.chance >= 1f)
            {
                foreach (StatDrawEntry statDrawEntry in this.hediffDef.SpecialDisplayStats(StatRequest.ForEmpty()))
                {
                    yield return statDrawEntry;
                }
                IEnumerator<StatDrawEntry> enumerator = null;
            }
            yield break;
        }

        public HediffDef hediffDef;

        public float severity = -1f;

        public int tier = -1;

        public ChemicalDef toleranceChemical;
    }
}