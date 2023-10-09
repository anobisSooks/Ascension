using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Ascension
{
    public class HediffComp_PseudoImmortalEffect : HediffComp
    {
        public override void CompPostMake()
        {
            //later need to only do fake lightning strike if it is visible as it is purely visual effect.
            //base.CompPostMake();
            if (Pawn != null && Find.CurrentMap != null)
            {
                if (Pawn.Position != null)
                {
                    Find.CurrentMap.weatherManager.eventHandler.AddEvent(new WeatherEvent_FakeLightningStrike(Find.CurrentMap, parent.pawn.Position));
                }
            }
        }
    }
}