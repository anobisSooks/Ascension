

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

            //Ascension_Settings settings = LoadedModManager.GetMod<Ascension_Mod>().GetSettings<Ascension_Settings>();
            if (pawn != null)
            {
                //need to make it so that: 
                bool addPawnPI = false;
                bool addPawnAF = false;
                if (pawn.health != null)
                {
                    if (pawn.health.hediffSet != null)
                    {
                        float randpi = Rand.Range(0, 1f);
                        float randaf = Rand.Range(0, 1f);

                        float chancePI = 0.01f;
                        float chanceAF = 0.07f;

                        if (randpi <= chancePI)
                        {
                            HediffDef pimmortal = AscensionDefOf.PseudoImmortality;
                            pawn.health.AddHediff(pimmortal);
                            addPawnPI = true;
                            randpi = Rand.Range(0, 1f);
                            if (randpi <= 0.1f)
                            {
                                randpi = Rand.Range(0.5f, 1);
                                pawn.health.hediffSet.GetFirstHediffOfDef(pimmortal).Severity = randpi;
                            }else
                            {
                                randpi = Rand.Range(0.1f, 0.5f);
                                pawn.health.hediffSet.GetFirstHediffOfDef(pimmortal).Severity = randpi;
                            }
                        }
                        if (randaf <= chanceAF)
                        {
                            HediffDef afoundation = AscensionDefOf.AscendantFoundation;
                            pawn.health.AddHediff(afoundation);
                            addPawnAF = true;
                            randaf = Rand.Range(0, 1f);
                            if (randaf <= 0.1f)
                            {
                                randaf = Rand.Range(0.5f, 1);
                                pawn.health.hediffSet.GetFirstHediffOfDef(afoundation).Severity = randaf;
                            }
                            else
                            {
                                randaf = Rand.Range(0.1f, 0.5f);
                                pawn.health.hediffSet.GetFirstHediffOfDef(afoundation).Severity = randaf;
                            }
                        }
                        if (addPawnPI)
                        {
                            if (pawn.Dead)
                            {
                                //pimmortalComponent.AddDeadImmortal(pawn, false);
                            }
                            else
                            {
                                pawn.health.AddHediff(AscensionDefOf.PseudoImmortality);
                            }
                        }
                        if (addPawnAF)
                        {
                            if (pawn.Dead)
                            {
                                //pimmortalComponent.AddDeadImmortal(pawn, false);
                            }
                            else
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

