using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace AscensionNotDub
{

    //dao hediff is gained from consuming Dao Energy

    //Dao Pool: This being has a pool of Dao Energy within its soul. When this being suffers from malnutrition thier body will convert Dao Energy within this pool into nutrients, feeding it.


    //this comp checks if they have malnutrition then feeds them and reduces the parent hediffs severity.
    public class HediffComp_DaoFeed : HediffComp
    {
        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);

            if (this.Pawn.needs.food.Starving)
            {
                this.Pawn.needs.food.CurLevel = this.Pawn.RaceProps.FoodLevelPercentageWantEat;
                this.parent.Severity = (this.parent.Severity - 0.1f);
            }
        }

    }
}