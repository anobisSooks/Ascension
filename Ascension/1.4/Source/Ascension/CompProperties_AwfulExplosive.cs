using System;
using System.Collections.Generic;
using Verse;
using RimWorld;

namespace Ascension
{
    // Token: 0x02000BB0 RID: 2992
    public class CompProperties_AwfulExplosive : CompProperties
    {
        // Token: 0x06004A67 RID: 19047 RVA: 0x0019AE2C File Offset: 0x0019902C
        public CompProperties_AwfulExplosive()
        {
            this.compClass = typeof(CompAwfulExplosive);
        }

        // Token: 0x06004A68 RID: 19048 RVA: 0x0019AEBE File Offset: 0x001990BE
        public override void ResolveReferences(ThingDef parentDef)
        {
            base.ResolveReferences(parentDef);
            if (this.explosiveDamageType == null)
            {
                this.explosiveDamageType = DamageDefOf.Flame;
            }
        }

        // Token: 0x06004A69 RID: 19049 RVA: 0x0019AEDA File Offset: 0x001990DA
        public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
        {
            foreach (string text in base.ConfigErrors(parentDef))
            {
                yield return text;
            }
            IEnumerator<string> enumerator = null;
            yield break;
            yield break;
        }

        public float explosiveRadius = 1.9f;

        public DamageDef explosiveDamageType;

        public int damageAmountBase = -1;

        public float armorPenetrationBase = -1f;

        public ThingDef postExplosionSpawnThingDef;

        public float postExplosionSpawnChance;

        public int postExplosionSpawnThingCount = 1;

        public bool applyDamageToExplosionCellsNeighbors;

        public ThingDef preExplosionSpawnThingDef;

        public float preExplosionSpawnChance;

        public int preExplosionSpawnThingCount = 1;

        public float chanceToStartFire;

        public bool damageFalloff;

        public bool explodeOnKilled;

        public GasType? postExplosionGasType;

        public bool doVisualEffects = true;

        public float propagationSpeed = 1f;

        public float explosiveExpandPerStackcount;

        public float explosiveExpandPerFuel;

        public EffecterDef explosionEffect;

        public SoundDef explosionSound;

        public float destroyThingOnExplosionSize;
    }
}