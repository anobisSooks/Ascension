using System;
using UnityEngine;
using Verse;
using Verse.Sound;
using RimWorld;

namespace Ascension
{
    // Token: 0x02000F07 RID: 3847
    [StaticConstructorOnStartup]
    public class WeatherEvent_FakeLightningStrike : WeatherEvent_LightningFlash
    {
        public WeatherEvent_FakeLightningStrike(Map map) : base(map)
        {
        }
        public WeatherEvent_FakeLightningStrike(Map map, IntVec3 forcedStrikeLoc) : base(map)
        {
            this.strikeLoc = forcedStrikeLoc;
        }
        public override void FireEvent()
        {
            WeatherEvent_FakeLightningStrike.DoStrike(this.strikeLoc, this.map, ref this.boltMesh);
        }
        public static void DoStrike(IntVec3 strikeLoc, Map map, ref Mesh boltMesh)
        {
            SoundDefOf.Thunder_OffMap.PlayOneShotOnCamera(map);
            if (!strikeLoc.IsValid)
            {
                strikeLoc = CellFinderLoose.RandomCellWith((IntVec3 sq) => sq.Standable(map) && !map.roofGrid.Roofed(sq), map, 1000);
            }
            boltMesh = LightningBoltMeshPool.RandomBoltMesh;
            if (!strikeLoc.Fogged(map))
            {
                Vector3 loc = strikeLoc.ToVector3Shifted();
                for (int i = 0; i < 4; i++)
                {
                    FleckMaker.ThrowSmoke(loc, map, 1.5f);
                    FleckMaker.ThrowMicroSparks(loc, map);
                    FleckMaker.ThrowLightningGlow(loc, map, 1.5f);
                }
            }
            SoundInfo info = SoundInfo.InMap(new TargetInfo(strikeLoc, map, false), MaintenanceType.None);
            SoundDefOf.Thunder_OnMap.PlayOneShot(info);
        }
        public override void WeatherEventDraw()
        {
            Graphics.DrawMesh(this.boltMesh, this.strikeLoc.ToVector3ShiftedWithAltitude(AltitudeLayer.Weather), Quaternion.identity, FadedMaterialPool.FadedVersionOf(WeatherEvent_FakeLightningStrike.LightningMat, this.LightningBrightness), 0);
        }

        private IntVec3 strikeLoc = IntVec3.Invalid;

        private Mesh boltMesh;

        private static readonly Material LightningMat = MatLoader.LoadMat("Weather/LightningBolt", -1);
    }
}
