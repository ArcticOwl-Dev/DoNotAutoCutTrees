using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using RimWorld.Planet;


namespace DoNotAutoCutTrees
{
    /// <summary>A GameComponent to save a list of pawns, which have Gizmo "ManualPawnList" active</summary>
    public class ManualPawnList : GameComponent
    {
        private List<Pawn> PawnList = new List<Pawn>();
        private Game game;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManualPawnList" /> class.
        /// (requiered Constructor)
        /// </summary>
        /// <param name="game">The game.</param>
        public ManualPawnList(Game game)
        {
            this.game = game;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ManualPawnList" /> class.
        /// (requiered Constructor)
        /// </summary>
        public ManualPawnList()
        {
        }
        /// <summary>Determines whether [is in list] [the specified pawn].</summary>
        /// <param name="pawn">The pawn.</param>
        /// <returns>
        /// <c>true</c> if [is in list] [the specified pawn]; otherwise, <c>false</c>.</returns>
        public bool IsInList(Pawn pawn)
        {
            if (pawn == null)
            {
                return false;
            }
            return this.PawnList.Contains(pawn);
        }
        /// <summary>Adds the pawn. If the pawn is not already in the list</summary>
        /// <param name="pawn">The pawn.</param>
        public void AddPawn(Pawn pawn)
        {
            if (!this.IsInList(pawn))
            {
                this.PawnList.Add(pawn);
            }
        }
        /// <summary>Removes the pawn from the list</summary>
        /// <param name="pawn">The pawn.</param>
        public void RemovePawn(Pawn pawn)
        {
            this.PawnList.Remove(pawn);
        }
        /// <summary>Add Pawn if the pawn is not in the list, else removes the pawn from the list</summary>
        /// <param name="pawn">The pawn.</param>
        public void TogglePawn(Pawn pawn)
        {
            if (this.IsInList(pawn))
            {
                this.RemovePawn(pawn);
            }
            else
            {
                this.AddPawn(pawn);
            }
        }
        /// <summary>Clears this list of all pawns</summary>
        public void Clear()
        {
            this.PawnList.Clear();
        }

        /// <summary>Determines whether PawnList is empty.</summary>
        /// <returns>
        ///   <c>true</c> if PawnList is empty; otherwise, <c>false</c>.</returns>
        public bool IsEmpty()
        {
            return !this.PawnList.Any();
        }
        /// <summary>
        /// Exposes the data.
        /// Override function from game to save the mod data
        /// </summary>
        public override void ExposeData()
        {
            this.RemoveIrrelevantPawns();
            Scribe_Collections.Look<Pawn>(ref PawnList, "PawnList", LookMode.Value);
        }
        /// <summary>Converts PawnList to string.</summary>
        /// <returns>A <see cref="System.String" /> of all pawn names in the PawnList.</returns>
        /// <exception cref="PawnList.EnumerableNullOrEmpty()">return <see cref="System.String" /> "Empty"</exception>
        public override string ToString()
        {
            if (this.IsEmpty()) return "Empty";
            return this.PawnList.ToStringSafeEnumerable();
        }
        /// <summary>Gets the Toggle ManualPawnList Gizmo</summary>
        /// <param name="pawn">The pawn.</param>
        /// <returns>
        ///   <para>
        ///     <see cref="Command_Toggle" /> to add or remove a pawn from the <see cref="this.pawnList">PawnList</see></para>
        /// </returns>
        public Gizmo GetGizmo(Pawn pawn)
        {
            Command_Toggle command_toggle = new Command_Toggle()
            {
                defaultLabel = "ManualPawnListGizmoLabel".Translate(),
                defaultDesc = "ManualPawnListGizmoDesc".Translate(),
                icon = PlaySettingsPatch.LoadStartup_DoNotAutoCutTrees.DoNotAutoCutTreesIcon,
                hotKey = KeyBindingDefOf.Designator_Deconstruct,
                order = 30f,
                isActive = (() => this.IsInList(pawn)),
                toggleAction = delegate()
                {
                    this.TogglePawn(pawn);
                },

            };
            return command_toggle;
        }
        public void RemoveIrrelevantPawns()
        {
            IEnumerable<Pawn> AllAlivePawnsWithTreeMoodDebuff = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive.Where(pawn => pawn.RaceProps.Humanlike && PlantUtility.CheckTreeMoodDebuff(pawn));
            this.PawnList = this.PawnList.Intersect(AllAlivePawnsWithTreeMoodDebuff).ToList();
        }
    }

    /// <summary>A static class to get <see cref="ManualPawnList" /> GameComponent</summary>
    public static class GetManualPawnList
    {
        /// <summary>Gets the game component <see cref="ManualPawnList" />.</summary>
        /// <param name="game">The game.</param>
        /// <returns>
        ///   Instance Object of <see cref="ManualPawnList" />
        /// </returns>
        /// <remarks>
        /// Returns the ManualPawnList GameComponents from all GameComponents. This is needed because you can't call a no static class from a static class
        /// </remarks>
        public static ManualPawnList GetGameComponentManualPawnList(Game game)
        {
            if (game.GetComponent<ManualPawnList>() == null)
            {
                game.components.Add(new ManualPawnList(game));
            }
            return game.GetComponent<ManualPawnList>();
        }
    }

}

