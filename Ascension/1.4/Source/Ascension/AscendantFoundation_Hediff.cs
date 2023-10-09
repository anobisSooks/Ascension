
using System.Collections.Generic;
using Verse;

namespace Ascension
{
    //need to make
    //when a
    public class AscendantFoundation_Hediff : HediffWithComps
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
