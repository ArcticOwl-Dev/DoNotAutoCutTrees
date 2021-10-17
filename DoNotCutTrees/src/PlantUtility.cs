using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Verse;

namespace DoNotAutoCutTrees
{
    public static class PlantUtility
    {
        /// <summary>
        /// Alternative function of the orginal function PawnWillingtoCutPlant_Job from the game
        /// Adds more conditions, when a pawn won't do cutting job.
        /// </summary>
        /// <param name="plant">plant which should be cut</param>
        /// <param name="P">pawn which should do the cutting</param>
        /// <returns> <c>true</c> when the plant should not be cut. <c>false</c> when the plant can be cut</returns>
        /// <remarks>
        /// The condition 'isTree' and 'treeLoversCareIfChopped' is taken over from the original function. <br />Then the function checks if at least one Setting is true and any pawn in the corresponding group gets a TreeMoodDebuff
        /// </remarks>
        public static bool PawnWillingtoCutPlant_Job(Thing plant, Pawn P)
        {
            
            IEnumerable<Pawn> AllAlivePawns = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive.Where(pawn => pawn.RaceProps.Humanlike);
            if (plant.def.plant.IsTree && plant.def.plant.treeLoversCareIfChopped && (
                (Settings.Get().DoNotAutoCutTreesGeneral) ||
                (Settings.Get().DoNotAutoCutTreesIdeoManualPawnList && AllAlivePawns.Any(pawn => CheckTreeMoodDebuff(pawn) && GetManualPawnList.GetGameComponentManualPawnList(Current.Game).IsInList(pawn))) ||
                (Settings.Get().DoNotAutoCutTreesIdeoIsColonist && AllAlivePawns.Any(pawn => pawn.IsFreeColonist && (pawn.HomeFaction == Faction.OfPlayer) && CheckTreeMoodDebuff(pawn))) ||
                (Settings.Get().DoNotAutoCutTreesIdeoIsSlave && AllAlivePawns.Any(pawn => pawn.IsSlaveOfColony && CheckTreeMoodDebuff(pawn))) ||
                (Settings.Get().DoNotAutoCutTreesIdeoIsGuest && AllAlivePawns.Any(pawn => pawn.IsFreeColonist && (pawn.HomeFaction != Faction.OfPlayer) && CheckTreeMoodDebuff(pawn))) ||
                (Settings.Get().DoNotAutoCutTreesIdeoIsPrisoner && AllAlivePawns.Any(pawn => pawn.IsPrisonerOfColony && CheckTreeMoodDebuff(pawn)))))
            {
                return false;

            }
            return true;
        }
        /// <summary>
        /// Checks if the Pawn has any precept, which has a negative effect on pawns mood, when a tree is cut.
        /// </summary>
        /// <param name="pawn"></param>
        /// <returns>
        /// <c>true</c> if has debuff, else <c>false</c>
        /// </returns>
        /// <remarks>
        /// With the condition "WarnPlayerOnDesignateChopTree" are the Precepts "TreeCutting_Prohibited", "TreeCutting_Horrible" and "TreeCutting_Disapproved" included.
        /// With the second condition we include extra the Precept "Trees_Desired"
        /// </remarks>
        public static bool CheckTreeMoodDebuff(Pawn pawn)
        {
            return (pawn.Ideo.WarnPlayerOnDesignateChopTree || pawn.Ideo.PreceptsListForReading.Any(percept => percept.def.defName == "Trees_Desired"));
        }


        /// <summary>static MethodInfo for the function <see cref="PlantUtility.PawnWillingtoCutPlant_Job" /></summary>
        public static MethodInfo m_PawnWillingtoCutPlant_Job = SymbolExtensions.GetMethodInfo(() => PlantUtility.PawnWillingtoCutPlant_Job(null, null));


    }



    /// <summary>
    /// Harmony Patch for the function <see cref="WorkGiver_GrowerSow.JobOnCell"></see></summary>
    /// <remarks>
    /// This function is called when a plant is in a growing zone, which should be sowed.
    /// </remarks>
    [HarmonyPatch(typeof(WorkGiver_GrowerSow), "JobOnCell")]
    public static class WorkGiver_GrowerSow_JobOnCell_Patch
    {
        /// <summary>
        /// Harmony Transpiler to replace the call of <see cref="RimWorld.PlantUtility.PawnWillingToCutPlant_Job" /> with <see cref="DoNotAutoCutTrees.PlantUtility.PawnWillingToCutPlant_Job" />
        /// </summary>
        /// <param name="instructions">List of all CodeInstructions of the function in IL-Code</param>
        /// <see cref="https://harmony.pardeike.net/articles/patching-transpiler-codes.html" />
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            bool found = false;
            foreach (var instruction in instructions)
            {
                if ((instruction.operand ?? string.Empty).ToString().Contains("PawnWillingToCutPlant_Job"))
                {
                    yield return new CodeInstruction(OpCodes.Call, PlantUtility.m_PawnWillingtoCutPlant_Job);
                    found = true;
                }
                else
                {
                    yield return instruction;
                }

            }
            if (found is false)
                Verse.Log.Error("Cannot find 'PawnWillingToCutPlant_Job' in WorkGiver_GrowerSow.JobOnCell");
        }
    }


    /// <summary>
    /// Harmony Patch for the function <see cref="WorkGiver_GrowerHarvest.HasJobOnCell"></see>
    /// </summary>
    /// <remarks>
    /// This function is called when a full grown Tree is standing in a growing zone. Cutting full grown Tree is harvesting.
    /// </remarks>
    [HarmonyPatch(typeof(WorkGiver_GrowerHarvest), "HasJobOnCell")]
    public static class WorkGiver_GrowerHarvest_HasJobOnCell_Patch
    {
        /// <summary>
        /// Harmony Transpiler to replace the call of <see cref="RimWorld.PlantUtility.PawnWillingToCutPlant_Job" /> with <see cref="DoNotAutoCutTrees.PlantUtility.PawnWillingToCutPlant_Job" /></summary>
        /// <param name="instructions">List of all CodeInstructions of the function in IL-Code</param>
        /// <see cref="https://harmony.pardeike.net/articles/patching-transpiler-codes.html" />
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            bool found = false;
            foreach (var instruction in instructions)
            {
                if ((instruction.operand ?? string.Empty).ToString().Contains("PawnWillingToCutPlant_Job"))
                {
                    yield return new CodeInstruction(OpCodes.Call, PlantUtility.m_PawnWillingtoCutPlant_Job);
                    found = true;
                }
                else
                {
                    yield return instruction;
                }

            }
            if (found is false)
                Verse.Log.Error("Cannot find 'PawnWillingToCutPlant_Job' in WorkGiver_GrowerHarvest.HasJobOnCell");
        }
    }

    /// <summary>
    /// Harmony Patch for the function <see cref="GenConstruct.HandleBlockingThingJob"></see>
    /// </summary>
    /// <remarks>
    /// This function is called when a plant blocks the building of a structure.
    /// </remarks>
    [HarmonyPatch(typeof(GenConstruct), "HandleBlockingThingJob")]
    public static class GenConstruct_HandleBlockingThingJob_Patch
    {
        /// <summary>
        /// Harmony Transpiler to replace the call of <see cref="RimWorld.PlantUtility.PawnWillingToCutPlant_Job" /> with <see cref="DoNotAutoCutTrees.PlantUtility.PawnWillingToCutPlant_Job" />
        /// </summary>
        /// <param name="instructions">List of all CodeInstructions of the function in IL-Code</param>
        /// <see cref="https://harmony.pardeike.net/articles/patching-transpiler-codes.html" />
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            bool found = false;
            foreach (var instruction in instructions)
            {
                if ((instruction.operand ?? string.Empty).ToString().Contains("PawnWillingToCutPlant_Job"))
                {
                    yield return new CodeInstruction(OpCodes.Call, PlantUtility.m_PawnWillingtoCutPlant_Job);
                    found = true;
                }
                else
                {
                    yield return instruction;
                }

            }
            if (found is false)
                Verse.Log.Error("Cannot find 'PawnWillingToCutPlant_Job' in GenConstruct.HandleBlockingThingJob");
        }
    }

    /// <summary>
    /// Harmony Patch for the function <see cref="RoofUtility.CanHandleBlockingThing"></see>
    /// </summary>
    /// <remarks>
    /// This function is called when a plant blocks the building of a roof.
    /// </remarks>
    [HarmonyPatch(typeof(RoofUtility), "CanHandleBlockingThing")]
    public static class RoofUtility_CanHandleBlockingThing_Patch
    {
        /// <summary>
        /// Harmony Transpiler to replace the call of <see cref="RimWorld.PlantUtility.PawnWillingToCutPlant_Job" /> with <see cref="DoNotAutoCutTrees.PlantUtility.PawnWillingToCutPlant_Job" />
        /// </summary>
        /// <param name="instructions">List of all CodeInstructions of the function in IL-Code</param>
        /// <see cref="https://harmony.pardeike.net/articles/patching-transpiler-codes.html" />
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            bool found = false;
            foreach (var instruction in instructions)
            {
                if ((instruction.operand ?? string.Empty).ToString().Contains("PawnWillingToCutPlant_Job"))
                {
                    yield return new CodeInstruction(OpCodes.Call, PlantUtility.m_PawnWillingtoCutPlant_Job);
                    found = true;
                }
                else
                {
                    yield return instruction;
                }

            }
            if (found is false)
                Verse.Log.Error("Cannot find 'PawnWillingToCutPlant_Job' in RoofUtility_CanHandleBlockingThing");
        }
    }

    /// <summary>
    /// Harmony Patch for the function <see cref="RoofUtility.HandleBlockingThingJob"></see>
    /// </summary>
    /// <remarks>
    /// This function is called when a full grown Tree is standing in a growing zone. Cutting full grown Tree is harvesting.
    /// </remarks>
    [HarmonyPatch(typeof(RoofUtility), "HandleBlockingThingJob")]
    public static class RoofUtility_HandleBlockingThingJob_Patch
    {
        /// <summary>
        /// Harmony Transpiler to replace the call of <see cref="RimWorld.PlantUtility.PawnWillingToCutPlant_Job" /> with <see cref="DoNotAutoCutTrees.PlantUtility.PawnWillingToCutPlant_Job" />
        /// </summary>
        /// <param name="instructions">List of all CodeInstructions of the function in IL-Code</param>
        /// <see cref="https://harmony.pardeike.net/articles/patching-transpiler-codes.html" />
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            bool found = false;
            foreach (var instruction in instructions)
            {
                if ((instruction.operand ?? string.Empty).ToString().Contains("PawnWillingToCutPlant_Job"))
                {
                    yield return new CodeInstruction(OpCodes.Call, PlantUtility.m_PawnWillingtoCutPlant_Job);
                    found = true;
                }
                else
                {
                    yield return instruction;
                }

            }
            if (found is false)
                Verse.Log.Error("Cannot find 'PawnWillingToCutPlant_Job' in RoofUtility_HandleBlockingThingJob");
        }
    }


}


