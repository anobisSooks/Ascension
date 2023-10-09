
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Ascension
{
    public class PseudoImmortal_Hediff : HediffWithComps
    {
        public override string SeverityLabel
        {
            get
            {
                return this.Severity.ToString("0.0");
            }
        }
    }

}
