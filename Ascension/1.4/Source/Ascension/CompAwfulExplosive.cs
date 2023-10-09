using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;
using RimWorld;

namespace Ascension
{
    // Token: 0x020013CD RID: 5069
    public class CompAwfulExplosive : ThingComp
    {
        public CompProperties_AwfulExplosive Props
        {
            get
            {
                return (CompProperties_AwfulExplosive)this.props;
            }
        }
        public void AddThingsIgnoredByExplosion(List<Thing> things)
        {
            if (this.thingsIgnoredByExplosion == null)
            {
                this.thingsIgnoredByExplosion = new List<Thing>();
            }
            this.thingsIgnoredByExplosion.AddRange(things);
        }
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_References.Look<Thing>(ref this.instigator, "instigator", false);
            Scribe_Collections.Look<Thing>(ref this.thingsIgnoredByExplosion, "thingsIgnoredByExplosion", LookMode.Reference, Array.Empty<object>());
            Scribe_Values.Look<bool>(ref this.destroyedThroughDetonation, "destroyedThroughDetonation", false, false);
            Scribe_Values.Look<float?>(ref this.customExplosiveRadius, "explosiveRadius", null, false);
        }


        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            QualityCategory qc = new QualityCategory();
            QualityUtility.TryGetQuality(this.parent, out qc);
            if (((int)qc) < 1)
            {
                this.Detonate(this.parent.MapHeld, true);
            }
        }

        public float ExplosiveRadius()
        {
            float num = 3;
            return num;
        }

        protected void Detonate(Map map, bool ignoreUnspawned = false)
        {
            if (!ignoreUnspawned && !this.parent.SpawnedOrAnyParentSpawned)
            {
                return;
            }
            CompProperties_AwfulExplosive props = this.Props;
            float num = this.ExplosiveRadius();
            if (num <= 0f)
            {
                return;
            }
            if (props.explosiveExpandPerFuel > 0f && this.parent.GetComp<CompRefuelable>() != null)
            {
                this.parent.GetComp<CompRefuelable>().ConsumeFuel(this.parent.GetComp<CompRefuelable>().Fuel);
            }
            if (props.destroyThingOnExplosionSize <= num && !this.parent.Destroyed)
            {
                this.destroyedThroughDetonation = true;
                this.parent.Kill(null, null);
            }
            if (map == null)
            {
                Log.Warning("Tried to detonate CompExplosive in a null map.");
                return;
            }
            if (props.explosionEffect != null)
            {
                Effecter effecter = props.explosionEffect.Spawn();
                effecter.Trigger(new TargetInfo(this.parent.PositionHeld, map, false), new TargetInfo(this.parent.PositionHeld, map, false), -1);
                effecter.Cleanup();
            }
            Thing parent;
            if (this.instigator != null && (!this.instigator.HostileTo(this.parent.Faction) || this.parent.Faction == Faction.OfPlayer))
            {
                parent = this.instigator;
            }
            else
            {
                parent = this.parent;
            }
            GenExplosion.DoExplosion(this.parent.PositionHeld, map, num, props.explosiveDamageType, parent, props.damageAmountBase, props.armorPenetrationBase, props.explosionSound, null, null, null, props.postExplosionSpawnThingDef, props.postExplosionSpawnChance, props.postExplosionSpawnThingCount, this.Props.postExplosionGasType, props.applyDamageToExplosionCellsNeighbors, props.preExplosionSpawnThingDef, props.preExplosionSpawnChance, props.preExplosionSpawnThingCount, props.chanceToStartFire, props.damageFalloff, null, this.thingsIgnoredByExplosion, null, props.doVisualEffects, props.propagationSpeed, 0f, true, null, 1f);
        }

        private Thing instigator;

        public bool destroyedThroughDetonation;

        private List<Thing> thingsIgnoredByExplosion;

        public float? customExplosiveRadius;
    }
}
