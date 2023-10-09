
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace AscensionDubsLib
{
    public class DaoPool_Hediff : HediffWithComps
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
