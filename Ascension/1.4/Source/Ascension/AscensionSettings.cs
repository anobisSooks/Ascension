using System.Collections.Generic;
using Verse;
using UnityEngine;
using System;
using System.Reflection.Emit;

namespace Ascension
{
    public class AscensionSettings : ModSettings
    {
        /// <summary>
        /// Default settings.
        /// </summary>
        public bool humanoidOnlyBool = false;
        public bool logHealsBool = false;
        public bool opGoldenPillsBool = false;
        public bool machineCultivatorBool = false;
        //psuedo immortality default chance must be way lower than foundation chance, as psuedo immortality and beyond are considered as heavy time and resource investments.
        public float AFChance = 0.07f;
        public float PIChance = 0.01f;
        public override void ExposeData()
        {
            Scribe_Values.Look(ref humanoidOnlyBool, "humanoidOnlyBool");
            Scribe_Values.Look(ref machineCultivatorBool, "machineCultivatorBool");
            Scribe_Values.Look(ref opGoldenPillsBool, "opGoldenPillsBool");
            Scribe_Values.Look(ref logHealsBool, "logHealsBool");
            Scribe_Values.Look(ref AFChance, "AFChance", 200f);
            Scribe_Values.Look(ref PIChance, "PIChance", 200f);
            base.ExposeData();
        }
    }

    public class AscensionMod : Mod
    {
        AscensionSettings settings;

        /// <summary>
        /// A mandatory constructor which resolves the reference to our settings.
        /// </summary>
        /// <param name="content"></param>
        public AscensionMod(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<AscensionSettings>();
        }
        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);

            listingStandard.CheckboxLabeled("Humanoid Cultivator Spawns Only", ref settings.humanoidOnlyBool, "Enable if you want the spawn chance to only affect humanoid pawns.");
            listingStandard.CheckboxLabeled("Machine Cultivators", ref settings.machineCultivatorBool, "Enable if you want the spawn chance to affect mechanical pawns. Enabling is not recommended for Biotech users unless you want to randomly gestate pseudo-immortal mechs.");
            listingStandard.CheckboxLabeled("Same Ascendant Foundation Tier As Golden Pill Tier", ref settings.opGoldenPillsBool, "Enable if you want Golden Pills to give the same tier of Ascendant Foundation as the tier of the Golden Pill. This makes Ascendant Foundation building extremely easy compared to when this option is disabled. Most of this mod is balanced around this option being disabled so keep that in mind before enabling.");
            listingStandard.CheckboxLabeled("Log Heals", ref settings.logHealsBool, "Enable if you want to log heals from this mod in console. May flood the console if too many heals take place.");
            listingStandard.Label("Ascendant Foundation Spawn Chance: " + Math.Floor(this.settings.AFChance * 1000) / 10 + "%" + "\nDetermines the probability of a pawn recieving an Ascendant Foundation when spawned.");
            settings.AFChance = listingStandard.Slider(settings.AFChance, 0, 1f);
            listingStandard.Label("Pseudo-Immortal Spawn Chance: " + Math.Floor(this.settings.PIChance * 1000) / 10 + "%" + "\nDetermines the probability of a pawn recieving Pseudo-Immortalality when spawned.");
            settings.PIChance = listingStandard.Slider(settings.PIChance, 0, 1f);
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "Ascension";
        }
    }
}