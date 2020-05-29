using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.Components
{
    /// <summary>
    /// The players Journal.
    /// </summary>
    public class Journal : Component
    {
        #region Properties

        public int JournalID { get; set; }

        public List<int> RecordedBlueprintIDs { get; set; } = new List<int>();
        public List<int> RecordedCreatureIDs { get; set; } = new List<int>();

        #endregion


        #region Constructors

        /// <summary>
        /// Constructor for the Journal.
        /// </summary>
        /// <param name="journalID"></param>
        public Journal(int journalID)
        {
            JournalID = journalID;
        }

        #endregion


        #region Methods

        public override Tag ToEnum()
        {
            return Tag.JOURNAL;
        }

        public override void Awake()
        {
            GameObject.Tag = Tag.JOURNAL;
        }

        /// <summary>
        /// Show all blueprints that the player has recorded.
        /// </summary>
        public void ShowRecordedBlueprints()
        {
            TmpBlueprint tmpBlueprint;

            foreach (int blueprintID in RecordedBlueprintIDs)
            {
                tmpBlueprint = SQLiteHandler.Instance.GetBlueprint(blueprintID);

                Console.WriteLine($"Name: {tmpBlueprint.TmpName}");
                Console.WriteLine($"Description: {tmpBlueprint.TmpDescription}");

                ShowRequiredMaterial(blueprintID);

                Console.WriteLine();
            }
        }

        /// <summary>
        /// Show required materials for blueprints. Can be used when displaying blueprints.
        /// </summary>
        /// <param name="blueprintID"></param>
        public void ShowRequiredMaterial(int blueprintID)
        {
            TmpRequiredMaterial tmpRequiredMaterial;
            tmpRequiredMaterial = SQLiteHandler.Instance.GetRequiredMaterial(blueprintID);

            TmpMaterialType tmpMaterialType;
            tmpMaterialType = SQLiteHandler.Instance.GetMaterialType(tmpRequiredMaterial.TmpMaterialTypeID);


            Console.WriteLine($"Materials: {tmpMaterialType.TmpName}");
            Console.WriteLine($"Amount: {tmpRequiredMaterial.TmpAmount}");
        }

        /// <summary>
        /// Show all creatures that the player has recorded.
        /// </summary>
        public void ShowRecordedCreature()
        {
            TmpCreature tmpCreature;
            TmpMaterialType tmpMaterialType;

            foreach (int creatureID in RecordedCreatureIDs)
            {
                tmpCreature = SQLiteHandler.Instance.GetCreature(creatureID);
                tmpMaterialType = SQLiteHandler.Instance.GetMaterialType(tmpCreature.TmpMaterialTypeID);

                Console.WriteLine($"Name: {tmpCreature.TmpName}");
                Console.WriteLine($"Type: {tmpCreature.TmpType}");
                Console.WriteLine($"Description: {tmpCreature.TmpDescription}");
                Console.WriteLine($"Drops: {tmpMaterialType.TmpName}");
                Console.WriteLine($"Location: {tmpCreature.TmpLocation}");

                Console.WriteLine();
            }
        }

        #endregion
    }

    /// <summary>
    /// Used when fetching the Blueprint table from SQLite.
    /// </summary>
    public struct TmpBlueprint
    {
        public int TmpID { get; set; }

        public string TmpName { get; set; }
        public string TmpDescription { get; set; }

        /// <summary>
        /// Constructor for the TmpBlueprint struct.
        /// </summary>
        /// <param name="tmpID"></param>
        /// <param name="tmpName"></param>
        /// <param name="tmpDescription"></param>
        public TmpBlueprint(int tmpID, string tmpName, string tmpDescription)
        {
            TmpID = tmpID;
            TmpName = tmpName;
            TmpDescription = tmpDescription;
        }
    }

    /// <summary>
    /// Used when fetching the RequiredMaterial table from SQLite.
    /// </summary>
    public struct TmpRequiredMaterial
    {
        public int TmpMaterialTypeID { get; set; }
        public int TmpAmount { get; set; }

        /// <summary>
        /// Constructor for the TmpRequiredMaterial struct.
        /// </summary>
        /// <param name="tmpMaterialTypeID"></param>
        /// <param name="tmpAmound"></param>
        public TmpRequiredMaterial(int tmpMaterialTypeID, int tmpAmound)
        {
            TmpMaterialTypeID = tmpMaterialTypeID;
            TmpAmount = tmpAmound;
        }
    }

    /// <summary>
    /// Used when fetching the Creature table from SQLite.
    /// </summary>
    public struct TmpCreature
    {
        public int TmpMaterialTypeID { get; set; }
        public string TmpName { get; set; }
        public string TmpType { get; set; }
        public string TmpDescription { get; set; }
        public string TmpLocation { get; set; }

        /// <summary>
        /// Constructor for the TmpCreature struct.
        /// </summary>
        /// <param name="tmpMaterialTypeID"></param>
        /// <param name="tmpName"></param>
        /// <param name="tmpType"></param>
        /// <param name="tmpDescription"></param>
        /// <param name="tmpLocation"></param>
        public TmpCreature(int tmpMaterialTypeID, string tmpName, string tmpType, string tmpDescription, string tmpLocation)
        {
            TmpMaterialTypeID = tmpMaterialTypeID;
            TmpName = tmpName;
            TmpType = tmpType;
            TmpDescription = tmpDescription;
            TmpLocation = tmpLocation;
        }
    }
}
