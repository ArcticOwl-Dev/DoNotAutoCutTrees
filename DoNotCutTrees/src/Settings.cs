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
        public bool DoNotAutoCutTreesGeneral = false;
        public bool DoNotAutoCutTreesIdeoPlayerFaction = true;
        public bool DoNotAutoCutTreesIdeoGuest = true;
        public bool DoNotAutoCutTreesIdeoPrisoner = false;
 
        /// <summary>
        /// The part that writes our settings to file. Note that saving is by ref.
        /// </summary>
        public override void ExposeData()
        {
            Scribe_Values.Look(ref DoNotAutoCutTreesGeneral, "DoNotAutoCutTreesGeneral");
            Scribe_Values.Look(ref DoNotAutoCutTreesIdeoPlayerFaction, "DoNotAutoCutTreesIdeoPlayerfaction");
            Scribe_Values.Look(ref DoNotAutoCutTreesIdeoGuest, "DoNotAutoCutTreeIdeoGuest");
            Scribe_Values.Look(ref DoNotAutoCutTreesIdeoPrisoner, "DoNotAutoCutTreeIdeoPrisoner");
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
            listingStandard.CheckboxLabeled("SettingIdeoPlayerFaction".Translate(), ref DoNotAutoCutTreesIdeoPlayerFaction, "SettingIdeoPlayerFactionDesc".Translate());
            listingStandard.CheckboxLabeled("SettingIdeoGuest".Translate(), ref DoNotAutoCutTreesIdeoGuest, "SettingIdeoGuestDesc".Translate());
            listingStandard.CheckboxLabeled("SettingIdeoPrisoner".Translate(), ref DoNotAutoCutTreesIdeoPrisoner, "SettingIdeoPrisonerDesc".Translate());
            listingStandard.CheckboxLabeled("SettingGeneral".Translate(), ref DoNotAutoCutTreesGeneral, "SettingGeneralDesc".Translate());
            listingStandard.End();
        }

    }

}
