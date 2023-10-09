

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;
using OpCodes = System.Reflection.Emit.OpCodes;



namespace Ascension
{
    [StaticConstructorOnStartup]
    public static class AscensionHarmony
    {
        static AscensionHarmony()
        {
            Harmony harmony = new Harmony ("anobis.epicmods.ascension");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Harmony.DEBUG = false;

            harmony.Patch(original: AccessTools.Method(type: typeof(PawnGenerator), name: "GenerateInitialHediffs"), prefix: null, postfix: null,
                transpiler: new HarmonyMethod(typeof(AscensionHarmony), nameof(BookIconTranspiler)));

        }
        public static IEnumerable<CodeInstruction> BookIconTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> instructionList = instructions.ToList();
            for (int i = 0; i < instructionList.Count; i++)
            {
                CodeInstruction instruction = instructionList[i];


                if (instruction.opcode == OpCodes.Call && instruction.operand == AccessTools.Method(type: typeof(PawnAddictionHediffsGenerator), name: "GenerateAddictionsAndTolerancesFor"))
                {
                    yield return new CodeInstruction(opcode: OpCodes.Ldarg_0);
                    yield return new CodeInstruction(opcode: OpCodes.Call, operand: AccessTools.Method(type: typeof(AscensionHarmony), name: nameof(AscensionHarmony.ChanceAddAscension)));
                }
                yield return instruction;
            }
        }
        public static void ChanceAddAscension(Pawn pawn)
        {
            //some settings for psuedo immortality and ascendancy chance should be done as a game condition later instead of an actual setting to discourage unbalanced cheese and allow specific items to change them instead
            AscensionSettings settings = LoadedModManager.GetMod<AscensionMod>().GetSettings<AscensionSettings>();
            if (pawn != null)
            {
                bool addPawnPI = false;
                bool addPawnAF = false;
                if (pawn.health != null)
                {
                    bool condition = pawn.health.hediffSet != null;
                    if (settings.humanoidOnlyBool)
                    {
                        condition = (pawn.health.hediffSet != null) && (pawn.def.race.Humanlike);
                    }
                    if (!settings.machineCultivatorBool)
                    {
                        if (pawn.def.race.IsMechanoid || !pawn.def.race.IsFlesh)
                        {
                            condition = false;
                        }
                    }
                    if (condition)
                    {
                        float randpi = Rand.Range(0, 1f);
                        float randaf = Rand.Range(0, 1f);
                        
                        float chancePI = settings.PIChance;
                        float chanceAF = settings.AFChance;
                        HediffDef afoundation = AscensionDefOf.AscendantFoundation;
                        //if the chance number is greater or equal to the rng number it gives the hediff, this makes it so a 0.07 chance would be a 7% chance.
                        if (randpi <= chancePI)
                        {
                            //grabs psuedo immortality defof to add to pown
                            HediffDef pimmortal = AscensionDefOf.PseudoImmortality;
                            pawn.health.AddHediff(pimmortal);
                            addPawnPI = true;
                            randpi = Rand.Range(0, 1f);
                            //0.1f makes it a 10% chance for a high level
                            if (randpi <= 0.1f)
                            {
                                //higher/rare psuedo immortality tiers 4 to 6 (Saint, Celestial, Grand Emperor). Does not include Supreme God Emperor.
                                randpi = Rand.Range(4, 6.6f);
                                pawn.health.hediffSet.GetFirstHediffOfDef(pimmortal).Severity = randpi;
                            }else
                            {
                                //lower/common psuedo immortality tiers 1 to 3 (king, Sovereign, sage)
                                randpi = Rand.Range(0.01f, 3);
                                pawn.health.hediffSet.GetFirstHediffOfDef(pimmortal).Severity = randpi;
                            }
                            //psuedo immortals always have max foundation
                            pawn.health.AddHediff(afoundation);
                            pawn.health.hediffSet.GetFirstHediffOfDef(afoundation).Severity = 12;
                        }
                        else if (randaf <= chanceAF)
                        {
                            //if they failed to get psuedo immortality roll for ascendant foundation.
                            pawn.health.AddHediff(afoundation);
                            addPawnAF = true;
                            randaf = Rand.Range(0, 1f);
                            if (randaf <= 0.1f)
                            {
                                //higher/rare foundation tiers 8 to 12
                                randaf = Rand.Range(8, 12);
                                pawn.health.hediffSet.GetFirstHediffOfDef(afoundation).Severity = randaf;
                            }
                            else
                            {
                                //lower/common foundation tiers 1 to 7
                                randaf = Rand.Range(0.01f, 7);
                                pawn.health.hediffSet.GetFirstHediffOfDef(afoundation).Severity = randaf;
                            }
                        }
                        if (addPawnPI)
                        {
                            //adds it if the pawn is not dead
                            if (!pawn.Dead)
                            {
                                pawn.health.AddHediff(AscensionDefOf.PseudoImmortality);
                            }
                        }
                        if (addPawnAF)
                        {
                            if (!pawn.Dead)
                            {
                                pawn.health.AddHediff(AscensionDefOf.AscendantFoundation);
                            }
                        }
                    }
                }
            }
        }
    }
    [AttributeUsage(AttributeTargets.Class)]
    public class HotSwappableAttribute : Attribute
    {
    }
}

