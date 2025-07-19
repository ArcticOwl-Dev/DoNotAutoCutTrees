using RimWorld;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using HarmonyLib;

namespace DoNotAutoCutTrees
{
    public class Mod : Verse.Mod
    {
        /// <summary>
        /// A mandatory constructor which resolves the reference to our settings.
        /// </summary>
        /// <param name="content"></param>
        public Mod(ModContentPack content) : base(content)
        {
#if DEBUG
            Harmony.DEBUG = true;
            Log.Message("DoNotCutTreesModLoaded!");
#endif
            Harmony harmony = new Harmony("AllenCaine.DoNotAutoCutTrees");
            harmony.PatchAll();
            this.settings = GetSettings<DoNotAutoCutTrees.Settings>();
        }


        /// <summary>
        /// A reference to our settings.
        /// </summary>
        Settings settings;

        /// <summary>
        /// The (optional) GUI part to set your settings.
        /// </summary>
        /// <param name="inRect">A Unity Rect with the size of the settings window.</param>
        public override void DoSettingsWindowContents(Rect inRect)
        {
            settings.WindowContents(inRect);
            base.DoSettingsWindowContents(inRect);
        }

        /// <summary>
        /// Override SettingsCategory to show up in the list of settings.
        /// Using .Translate() is optional, but does allow for localisation.
        /// </summary>
        /// <returns>The (translated) mod name.</returns>
        public override string SettingsCategory()
        {
            return "DoNotAutoCutTrees".Translate();
        }

    }
}

