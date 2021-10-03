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

//Code only compiles if DEBUG Variable is set
#if DEBUG


	[HarmonyPatch(typeof(PlaySettings), "DoPlaySettingsGlobalControls")]
		internal class PlaySettingsPatch
		{
		/// <summary>
		/// Adds an ButtonIcon to the WidgetRow down right, which runs the PrintDebugMessage Function
		/// </summary>
		public static void Postfix(WidgetRow row, bool worldView)
			{
				if (worldView)
				{
					return;
				}
				if (row.ButtonIcon(LoadStartup_DoNotAutoCutTrees.debugTexture, "PrintDebugMessage", null, null, null, true, -1f))
				{
					PlaySettingsPatch.DebugFunction();
				}
			}
		/// <summary>
		/// A debug function which is called by the debug button in the WidgetRow. 
		/// Can Log different things in to the Debug Menu 
		/// </summary>
		public static void DebugFunction()
        {

			List<Pawn> Pawns = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive.Where(Pawn => Pawn.RaceProps.Humanlike).ToList();
			Log.Message("-------------------");
			foreach (Pawn P in Pawns)
			{
				Log.Message("Name: " + P.ToString());
				Log.Message("-> IsColonist: " + P.IsColonist + " - HomeFaction: " + P.HomeFaction + " - BaseFaction: " + P.Faction);
				if (P.Ideo != null)
                {
					Log.Message("-> IdeoName: " + P.Ideo.name + " - IdeoWarnTree: " + P.Ideo.WarnPlayerOnDesignateChopTree);
				}
				if (P.guest.HostFaction != null)
				{
					Log.Message("-> HostFaction: " + P.guest.HostFaction.ToStringSafe() + " - GuestStatus: " + P.guest.GuestStatus.ToStringSafe());
				}
			}
			Log.Message(Pawns.ToStringSafeEnumerable());
			Log.Message("Any Humanlike Pawn with Ideo WarnTreeChop: " + Pawns.Any(Pawn => Pawn.Ideo.WarnPlayerOnDesignateChopTree == true));
			Log.Message("Any Guest          with Ideo WarnTreeChop: " + Pawns.Any(Pawn => Pawn.GuestStatus == GuestStatus.Guest && Pawn.Ideo.WarnPlayerOnDesignateChopTree == true));
			Log.Message("Any Colonist       with Ideo WarnTreeChop: " + PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists.Any(Pawn => Pawn.Ideo.WarnPlayerOnDesignateChopTree == true));
			Log.Message("Any Prisoner       with Ideo WarnTreeChop: " + PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_PrisonersOfColony.Any(Pawn => Pawn.Ideo.WarnPlayerOnDesignateChopTree == true));

		}
	}

	/// <summary>
	/// Loads the debug.png from the Textures folder to use it in the game
	/// </summary>
	[StaticConstructorOnStartup]
	internal class LoadStartup_DoNotAutoCutTrees
    {
		public static readonly Texture2D debugTexture = ContentFinder<Texture2D>.Get("debug", true);
	}


//End of DEBUG Code
#endif
}

