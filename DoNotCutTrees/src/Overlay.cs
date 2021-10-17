using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using RimWorld;
using Verse;
using UnityEngine;





namespace DoNotAutoCutTrees
{




    [HarmonyPatch(typeof(PlaySettings), "DoPlaySettingsGlobalControls")]
    internal class PlaySettingsPatch
    {
        /// <summary>Postfix to add ButtonIcons to the WidgetRow in the bottom right corner.</summary>
        /// <param name="row"></param>
        /// <param name="worldView"></param>
        public static void Postfix(WidgetRow row, bool worldView)
        {
            if (worldView)
            {
                return;
            }
            //Code only compiles if DEBUG Variable is set
#if DEBUG
            if (row.ButtonIcon(LoadStartup_Debug_DoNotAutoCutTrees.debugTexture, "PrintDebugMessage", null, null, null, true, -1f))
            {
                PlaySettingsPatch.DebugFunction();
            }
            //End of DEBUG Code
#endif
            if (Settings.Get().ShowQuickSettingButton)
            {
                if (row.ButtonIcon(LoadStartup_DoNotAutoCutTrees.DoNotAutoCutTreesIcon, "SelectCleaningArea".Translate(), null, null, null, true, -1f))
                {
                    PlaySettingsPatch.ShowFloatingMenuFunction();
                }
            }
        }


        /// <summary>Shows the floating menu.</summary>
        private static void ShowFloatingMenuFunction()
        {
            FloatMenu floatMenu = PlaySettingsPatch.CreateFlowMenu();
            if (floatMenu != null)
            {
                Find.WindowStack.Add(floatMenu);
            }
        }

        /// <summary>Create FlowMenu</summary>
        /// <returns>FloatMenu with all FloatMenuOptions <br /></returns>
        private static FloatMenu CreateFlowMenu()
        {
            List<FloatMenuOption> list = new List<FloatMenuOption>();


            list.Add(new FloatMenuOption("SettingIdeoIsColonist".Translate(), delegate ()
                    {
                        Settings.Get().DoNotAutoCutTreesIdeoIsColonist = !Settings.Get().DoNotAutoCutTreesIdeoIsColonist;
                    }, (Settings.Get().DoNotAutoCutTreesIdeoIsColonist ? Verse.Widgets.CheckboxOnTex : Verse.Widgets.CheckboxOffTex),
                    Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));

            list.Add(new FloatMenuOption("SettingIdeoIsSlave".Translate(), delegate ()
                    {
                        Settings.Get().DoNotAutoCutTreesIdeoIsSlave = !Settings.Get().DoNotAutoCutTreesIdeoIsSlave;
                    }, (Settings.Get().DoNotAutoCutTreesIdeoIsSlave ? Verse.Widgets.CheckboxOnTex : Verse.Widgets.CheckboxOffTex),
                    Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));

            list.Add(new FloatMenuOption("SettingIdeoIsGuest".Translate(), delegate ()
                    {
                        Settings.Get().DoNotAutoCutTreesIdeoIsGuest = !Settings.Get().DoNotAutoCutTreesIdeoIsGuest;
                    }, (Settings.Get().DoNotAutoCutTreesIdeoIsGuest ? Verse.Widgets.CheckboxOnTex : Verse.Widgets.CheckboxOffTex),
                    Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));

            list.Add(new FloatMenuOption("SettingIdeoIsPrisoner".Translate(), delegate ()
                    {
                        Settings.Get().DoNotAutoCutTreesIdeoIsPrisoner = !Settings.Get().DoNotAutoCutTreesIdeoIsPrisoner;
                    }, (Settings.Get().DoNotAutoCutTreesIdeoIsPrisoner ? Verse.Widgets.CheckboxOnTex : Verse.Widgets.CheckboxOffTex),
                    Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));

            list.Add(new FloatMenuOption("SettingManualPawnListShort".Translate(), delegate ()
                    {
                        Settings.Get().DoNotAutoCutTreesIdeoManualPawnList = !Settings.Get().DoNotAutoCutTreesIdeoManualPawnList;
                    }, (Settings.Get().DoNotAutoCutTreesIdeoManualPawnList ? Verse.Widgets.CheckboxOnTex : Verse.Widgets.CheckboxOffTex),
                    Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));

            list.Add(new FloatMenuOption("SettingGeneralShort".Translate(), delegate ()
                    {
                        Settings.Get().DoNotAutoCutTreesGeneral = !Settings.Get().DoNotAutoCutTreesGeneral;
                    }, (Settings.Get().DoNotAutoCutTreesGeneral ? Verse.Widgets.CheckboxOnTex : Verse.Widgets.CheckboxOffTex),
                    Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));

            return new FloatMenu(list);
        }


        /// <summary>
        /// Loads the Textures from the Textures folder to use it in the game
        /// </summary>
        [StaticConstructorOnStartup]
        internal class LoadStartup_DoNotAutoCutTrees
        {
            public static readonly Texture2D DoNotAutoCutTreesIcon = ContentFinder<Texture2D>.Get("DoNotAutoCutTreesIcon", true);
        }

        /// <summary>Harmony Patch for the function <see cref="Pawn.GetGizmos"></see></summary>
        [StaticConstructorOnStartup]
        [HarmonyPatch(typeof(Pawn), "GetGizmos")]
        public static class ManuellPawnListGizmo
        {
            /// <summary>
            /// Postfix to add the ManualPawnList Gizmo if the ManualPawnList setting is true and the pawn has a tree cutting debuff
            /// </summary>
            /// <param name="__result">The result from the original function</param>
            /// <param name="__instance">The Pawn instance from the original function</param>
            public static void Postfix(ref IEnumerable<Gizmo> __result, Pawn __instance)
            {
                if (RimWorld.Planet.WorldRendererUtility.WorldRenderedNow) return;

                if (Settings.Get().DoNotAutoCutTreesIdeoManualPawnList && PlantUtility.CheckTreeMoodDebuff(__instance))
                {
                    List<Gizmo> result = __result.ToList();
                    result.Add(GetManualPawnList.GetGameComponentManualPawnList(Current.Game).GetGizmo(__instance));
                    __result = result;
                }
            }
        }


        //Code only compiles if DEBUG Variable is set
#if DEBUG

        /// <summary>
        /// A debug function which is called by the debug button in the WidgetRow. 
        /// Can Log different things in to the Debug Menu 
        /// </summary>
        /// 
        public static void DebugFunction()
        {

            List<Pawn> AllAlivePawns = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive.Where(pawn => pawn.RaceProps.Humanlike).ToList();
            Log.Message("-------------------");
            foreach (Pawn P in AllAlivePawns)
            {
                Boolean HasHostFaction = false;
                if (P.guest.HostFaction != null) HasHostFaction = true;
                Log.Message("  ");
                Log.Message(P.ToString());
                Log.Message("-> IsFreeColonist: " + P.IsFreeColonist + " - HomeFaction: " + P.HomeFaction + " - BaseFaction: " + P.Faction);
                Log.Message("-> IsSlaveOfColony:    " + P.IsSlaveOfColony + " - IsPrisonerOfColony:  " + P.IsPrisonerOfColony + " - HasHostFaction: " + HasHostFaction);
                Log.Message("-> HostFaction: " + P.guest.HostFaction.ToStringSafe() + " - GuestStatus: " + P.guest.GuestStatus.ToStringSafe());
                if (P.guest.HostFaction != null)
                {
                    Log.Message("-> HostFaction: " + P.guest.HostFaction.ToStringSafe() + " - GuestStatus: " + P.guest.GuestStatus.ToStringSafe());
                }
                if (P.Ideo != null)
                {
                    Log.Message("-> IdeoName: " + P.Ideo.name + " - IdeoWarnTree: " + P.Ideo.WarnPlayerOnDesignateChopTree);
                    //foreach (Precept precept in P.Ideo.PreceptsListForReading)
                    //{
                    //	Log.Message("--> PreceptList: " + precept.Label + " - " + precept.def.defName);
                    //}

                }
                Log.Message("-> SetColonist: " + (P.IsFreeColonist && (P.HomeFaction == Faction.OfPlayer)) + " SetGuest: " + (P.IsFreeColonist && (P.HomeFaction != Faction.OfPlayer)));

            }

            Log.Message("Always DoNotAutoCutTrees: " + Settings.Get().DoNotAutoCutTreesGeneral);
            Log.Message("Any Pawn in ManualPawnList         : " + AllAlivePawns.Any(pawn => PlantUtility.CheckTreeMoodDebuff(pawn) && GetManualPawnList.GetGameComponentManualPawnList(Current.Game).IsInList(pawn)));
            Log.Message("Any Colonist with Ideo WarnTreeChop: " + AllAlivePawns.Any(pawn => pawn.IsFreeColonist && (pawn.HomeFaction == Faction.OfPlayer) && PlantUtility.CheckTreeMoodDebuff(pawn)));
            Log.Message("Any Slave    with Ideo WarnTreeChop: " + AllAlivePawns.Any(pawn => pawn.IsSlaveOfColony && PlantUtility.CheckTreeMoodDebuff(pawn)));
            Log.Message("Any Guest    with Ideo WarnTreeChop: " + AllAlivePawns.Any(pawn => pawn.IsFreeColonist && (pawn.HomeFaction != Faction.OfPlayer) && PlantUtility.CheckTreeMoodDebuff(pawn)));
            Log.Message("Any Prisoner with Ideo WarnTreeChop: " + AllAlivePawns.Any(pawn => pawn.IsPrisonerOfColony && PlantUtility.CheckTreeMoodDebuff(pawn)));
            Log.Message("----------------------------------------------");
            Log.Message("ManualPawnList: " + GetManualPawnList.GetGameComponentManualPawnList(Current.Game).ToString());
        }

        /// <summary>
        /// Loads the debug.png from the Textures folder to use it in the game
        /// </summary>
        [StaticConstructorOnStartup]
        internal class LoadStartup_Debug_DoNotAutoCutTrees
        {
            public static readonly Texture2D debugTexture = ContentFinder<Texture2D>.Get("debug", true);
        }

        //End of DEBUG Code
#endif
    }
}

