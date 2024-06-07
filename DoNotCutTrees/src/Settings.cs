using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using UnityEngine;
using Verse;

namespace DoNotAutoCutTrees
{
    public class Settings : ModSettings
    {
        /// <summary>
        /// Declare the settings of the mod
        /// </summary>

        public bool ShowQuickSettingButton = true;

        public bool DoNotAutoCutTreesGeneral = false;
        public bool DoNotAutoCutTreesIdeoManualPawnList = false;
 
        public bool DoNotAutoCutTreesIdeoIsColonist = true;
        public bool DoNotAutoCutTreesIdeoIsSlave = true;
        public bool DoNotAutoCutTreesIdeoIsGuest = true;
        public bool DoNotAutoCutTreesIdeoIsPrisoner = true;

        public bool DoNotAutoCutTreesAutoExtract = false;

        /// <summary>
        /// The part that writes our settings to file. Note that saving is by ref.
        /// </summary>
        public override void ExposeData()
        {
            Scribe_Values.Look(ref ShowQuickSettingButton, "ShowQuickSettingButton");

            Scribe_Values.Look(ref DoNotAutoCutTreesGeneral, "DoNotAutoCutTreesGeneral");
            Scribe_Values.Look(ref DoNotAutoCutTreesIdeoManualPawnList, "DoNotAutoCutTreesIdeoManuellList");

            Scribe_Values.Look(ref DoNotAutoCutTreesIdeoIsColonist, "DoNotAutoCutTreesIdeoIsColonist");
            Scribe_Values.Look(ref DoNotAutoCutTreesIdeoIsSlave, "DoNotAutoCutTreesIdeoIsSlave");
            Scribe_Values.Look(ref DoNotAutoCutTreesIdeoIsGuest, "DoNotAutoCutTreeIdeoIsGuest");
            Scribe_Values.Look(ref DoNotAutoCutTreesIdeoIsPrisoner, "DoNotAutoCutTreeIdeoIsPrisoner");

            Scribe_Values.Look(ref DoNotAutoCutTreesAutoExtract, "DoNotAutoCutTreesAutoExtract");
            base.ExposeData();
        }

        /// <summary>
        /// Shorter writing to get the settings of the mod
        /// </summary>
        public static Settings Get()
        {
            return LoadedModManager.GetMod<Mod>().GetSettings<Settings>();
        }

        /// <summary>
        /// Write the content of the setting menu
        /// </summary>
        /// <param name="inRect">A Unity Rect with the size of the settings window.</param>
        public void WindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.Label("SettingLabel".Translate());
            listingStandard.CheckboxLabeled("SettingIdeoIsColonist".Translate(), ref DoNotAutoCutTreesIdeoIsColonist, "SettingIdeoIsColonistDesc".Translate());
            listingStandard.CheckboxLabeled("SettingIdeoIsSlave".Translate(), ref DoNotAutoCutTreesIdeoIsSlave, "SettingIdeoIsSlave".Translate());
            listingStandard.CheckboxLabeled("SettingIdeoIsGuest".Translate(), ref DoNotAutoCutTreesIdeoIsGuest, "SettingIdeoIsGuestDesc".Translate());
            listingStandard.CheckboxLabeled("SettingIdeoIsPrisoner".Translate(), ref DoNotAutoCutTreesIdeoIsPrisoner, "SettingIdeoIsPrisonerDesc".Translate());
            listingStandard.CheckboxLabeled("SettingManualPawnList".Translate(), ref DoNotAutoCutTreesIdeoManualPawnList, "SettingManualPawnListDesc".Translate());
            if(Current.ProgramState == ProgramState.Playing && DoNotAutoCutTreesIdeoManualPawnList)
            {
                listingStandard.Gap();
                listingStandard.Label("ManualPawnList".Translate() + ": " + GetManualPawnList.GetGameComponentManualPawnList(Current.Game).ToString());
                if (!GetManualPawnList.GetGameComponentManualPawnList(Current.Game).IsEmpty() && listingStandard.ButtonText("ManualPawnListClear".Translate()))
                {
                    GetManualPawnList.GetGameComponentManualPawnList(Current.Game).Clear();
                }
            }
            listingStandard.Gap();
            listingStandard.CheckboxLabeled("SettingGeneral".Translate(), ref DoNotAutoCutTreesGeneral, "SettingGeneralDesc".Translate());
            listingStandard.CheckboxLabeled("ShowQuickSettingButton".Translate(), ref ShowQuickSettingButton, "ShowQuickSettingButtonDesc".Translate());
            listingStandard.Gap();
            listingStandard.CheckboxLabeled("SettingAutoExtract".Translate(), ref DoNotAutoCutTreesAutoExtract, "SettingAutoExtractDesc".Translate());
            listingStandard.End();
        }

    }

}
