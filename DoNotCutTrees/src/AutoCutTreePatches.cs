using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;
using RimWorld;
using HarmonyLib;
using System.Reflection;
using System.Reflection.Emit;

namespace DoNotAutoCutTrees
{
    /// <summary>
    /// Alternative function of the orginal function PawnWillingtoCutPlant_Job from the game
    /// Adds more conditions, when a pawn won't do cutting job.
    /// The condition 'isTree' and 'treeLoversCareIfChopped' is taken over from the orginal function
    /// Then the function checks if at least one Setting with the corresponding condition is true
    /// </summary>
    public static class AutoCutTreePatches
    {
        public static bool PawnWillingtoCutPlant_Job(Thing plant, Pawn pawn)
        {
            if (plant.def.plant.IsTree && plant.def.plant.treeLoversCareIfChopped && 
                (Settings.Get().DoNotAutoCutTreesGeneral ||
                (Settings.Get().DoNotAutoCutTreesIdeoPlayerFaction && PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists.Any(Pawn => Pawn.Ideo.WarnPlayerOnDesignateChopTree == true)) ||
                (Settings.Get().DoNotAutoCutTreesIdeoGuest && PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive.Where(Pawn => Pawn.RaceProps.Humanlike).Any(Pawn => Pawn.GuestStatus == GuestStatus.Guest && Pawn.Ideo.WarnPlayerOnDesignateChopTree == true)) ||
                (Settings.Get().DoNotAutoCutTreesIdeoPrisoner && PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_PrisonersOfColony.Any(Pawn =>  Pawn.Ideo.WarnPlayerOnDesignateChopTree == true))))
            {
                return false;
            }
            return true;
        }
        static Thing plant1;
        static Pawn pawn1;
        public static MethodInfo m_PawnWillingtoCutPlant_Job = SymbolExtensions.GetMethodInfo(() => AutoCutTreePatches.PawnWillingtoCutPlant_Job(plant1, pawn1));


    }

    /// <summary>
    /// Replace the call of orginal game function PawnWillingToCutPlant_Job with the PawnWillingToCutPlant_Job from this mod
    /// </summary>
    [HarmonyPatch(typeof(WorkGiver_GrowerSow), "JobOnCell")]
    public static class WorkGiver_GrowerSow_JobOnCell_Patch
    {

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            bool found = false;
            foreach (var instruction in instructions)
            {
                if ((instruction.operand ?? string.Empty).ToString().Contains("PawnWillingToCutPlant_Job"))
                {
                    yield return new CodeInstruction(OpCodes.Call, AutoCutTreePatches.m_PawnWillingtoCutPlant_Job);
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
    /// Replace the call of orginal game function PawnWillingToCutPlant_Job with the PawnWillingToCutPlant_Job from this mod
    /// </summary>
    [HarmonyPatch(typeof(WorkGiver_GrowerHarvest), "HasJobOnCell")]
    public static class WorkGiver_GrowerHarvest_HasJobOnCell_Patch
    {

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            bool found = false;
            foreach (var instruction in instructions)
            {
                if ((instruction.operand ?? string.Empty).ToString().Contains("PawnWillingToCutPlant_Job"))
                {
                    yield return new CodeInstruction(OpCodes.Call, AutoCutTreePatches.m_PawnWillingtoCutPlant_Job);
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
    /// Replace the call of orginal game function PawnWillingToCutPlant_Job with the PawnWillingToCutPlant_Job from this mod
    /// </summary>
    [HarmonyPatch(typeof(GenConstruct), "HandleBlockingThingJob")]
    public static class GenConstruct_HandleBlockingThingJob_Patch
    {

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            bool found = false;
            foreach (var instruction in instructions)
            {
                if ((instruction.operand ?? string.Empty).ToString().Contains("PawnWillingToCutPlant_Job"))
                {
                    yield return new CodeInstruction(OpCodes.Call, AutoCutTreePatches.m_PawnWillingtoCutPlant_Job);
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
    /// Replace the call of orginal game function PawnWillingToCutPlant_Job with the PawnWillingToCutPlant_Job from this mod
    /// </summary>
    [HarmonyPatch(typeof(RoofUtility), "CanHandleBlockingThing")]
    public static class RoofUtility_CanHandleBlockingThing_Patch
    {

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            bool found = false;
            foreach (var instruction in instructions)
            {
                if ((instruction.operand ?? string.Empty).ToString().Contains("PawnWillingToCutPlant_Job"))
                {
                    yield return new CodeInstruction(OpCodes.Call, AutoCutTreePatches.m_PawnWillingtoCutPlant_Job);
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
    /// Replace the call of orginal game function PawnWillingToCutPlant_Job with the PawnWillingToCutPlant_Job from this mod
    /// </summary>
    [HarmonyPatch(typeof(RoofUtility), "HandleBlockingThingJob")]
    public static class RoofUtility_HandleBlockingThingJob_Patch
    {

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            bool found = false;
            foreach (var instruction in instructions)
            {
                if ((instruction.operand ?? string.Empty).ToString().Contains("PawnWillingToCutPlant_Job"))
                {
                    yield return new CodeInstruction(OpCodes.Call, AutoCutTreePatches.m_PawnWillingtoCutPlant_Job);
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


