using System;
using System.Collections.Generic;
using Verse;
using RimWorld;

namespace Ascension
{
    public class IngestionOutcomeDoer_GiveHediffFromQuality : IngestionOutcomeDoer
    {
        protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested)
        {
            Hediff hediff = HediffMaker.MakeHediff(this.hediffDef, pawn, null);
            float num;
            num = this.severity;
            if (this.severity > 0f)
            {
                QualityCategory qc = new QualityCategory();
                QualityUtility.TryGetQuality(ingested, out qc);
                if (((int)qc) == 0)
                {
                    num = this.severity / 2;
                } else if (((int)qc) == 1) 
                {
                    num = (this.severity * 2)-2 ;
                }else if ((((int)qc) > 1) && (((int)qc) < 6))
                {
                    num = this.severity * (int)qc;
                }else if (((int)qc) == 6)
                {
                    num = this.severity * 10;
                }
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

        public ChemicalDef toleranceChemical;
    }
}