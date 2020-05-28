using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.Components
{
    public class Blueprint : Component
    {
        #region Fields

        private MouseState previousMouseState;
        private MouseState currentMouseState;

        private bool isRecorded;

        #endregion


        #region Properties

        public int BlueprintID { get; set; }

        #endregion


        #region Constructors

        public Blueprint(int ID)
        {
            this.BlueprintID = ID;
        }

        public Blueprint()
        {

        }

        #endregion


        #region Methods

        public override Tag ToEnum()
        {
            return Tag.BLUEPRINT;
        }

        public override void Awake()
        {
            GameObject.Tag = Tag.BLUEPRINT;
        }

        public override void Start()
        {

        }

        public void ShowBlueprint(int blueprintID)
        {
            //CheckRecordedBP(blueprintID, GameWorld.Instance.player.JournalID);

            //if (isRecorded == true)
            //{
                TmpBlueprint tmpBlueprint;
                tmpBlueprint = SQLiteHandler.Instance.GetBlueprint(blueprintID);

                Console.WriteLine($"Name: {tmpBlueprint.TmpName}");
                Console.WriteLine($"Description: {tmpBlueprint.TmpDescription}");

                ShowRequiredMaterial(blueprintID);
            //}

            //else
            //{
            //    Console.WriteLine("No such recorded blueprint.");
            //}
        }

        public void CheckRecordedBP(int blueprintID, int playerJournalID)
        {
            int fetchedJournalID;

            fetchedJournalID = Convert.ToInt32(SQLiteHandler.Instance.SelectIntValuesWhere("JournalID", "RecordedBP", $"BlueprintID = {blueprintID}"));

            Console.WriteLine(fetchedJournalID);

            fetchedJournalID = Convert.ToInt32(SQLiteHandler.Instance.SelectIntValuesWhere("JournalID", "RecordedBP", $"BlueprintID = {blueprintID}"));

            if (fetchedJournalID == playerJournalID)
            {
                isRecorded = true;
            }

            else
            {
                isRecorded = false;
            }
        }

        public void ShowRequiredMaterial(int blueprintID)
        {
            //CheckRecordedBP(blueprintID, GameWorld.Instance.player.JournalID);

            if (isRecorded == true)
            {
                TmpRequiredMaterial tmpRequiredMaterial;
                tmpRequiredMaterial = SQLiteHandler.Instance.GetRequiredMaterial(blueprintID);

                TmpMaterialType tmpMaterialType;
                tmpMaterialType = SQLiteHandler.Instance.GetMaterialType(tmpRequiredMaterial.TmpMaterialTypeID);


                Console.WriteLine($"Material Type: {tmpMaterialType.TmpName}");
                Console.WriteLine($"Amount: {tmpRequiredMaterial.TmpAmount}");
            }

            else
            {
                Console.WriteLine("No such recorded blueprint.");
            }

        }

        public void ShowCreature(int cretureID)
        {
            CheckRecordedCreature(cretureID, GameWorld.Instance.player.JournalID);

            if (isRecorded == true)
            {
                TmpCreature tmpCreature;
                tmpCreature = SQLiteHandler.Instance.GetCreature(cretureID);

                TmpMaterialType tmpMaterialType;
                tmpMaterialType = SQLiteHandler.Instance.GetMaterialType(tmpCreature.TmpMaterialTypeID);

                Console.WriteLine($"Name: {tmpCreature.TmpName}");
                Console.WriteLine($"Type: {tmpCreature.TmpType}");
                Console.WriteLine($"Description: {tmpCreature.TmpDescription}");
                Console.WriteLine($"Drops: {tmpMaterialType.TmpName}");
                Console.WriteLine($"Location: {tmpCreature.TmpLocation}");
            }

            else
            {
                Console.WriteLine("No such recorded creature.");
            }
        }

        public void CheckRecordedCreature(int creatureID, int playerJournalID)
        {
            int fetchedJournalID;

            fetchedJournalID = Convert.ToInt32(SQLiteHandler.Instance.SelectIntValuesWhere("JournalID", "RecordedCreature", $"CreatureID = {creatureID}"));

            if (fetchedJournalID == playerJournalID)
            {
                isRecorded = true;
            }

            else
            {
                isRecorded = false;
            }
        }

        #endregion
    }

    public struct TmpBlueprint
    {
        public int TmpID { get; set; }

        public string TmpName { get; set; }
        public string TmpDescription { get; set; }


        public TmpBlueprint(int tmpID, string tmpName, string tmpDescription)
        {
            TmpID = tmpID;
            TmpName = tmpName;
            TmpDescription = tmpDescription;
        }
    }

    public struct TmpRequiredMaterial
    {
        public int TmpMaterialTypeID { get; set; }
        public int TmpAmount { get; set; }


        public TmpRequiredMaterial(int tmpMaterialTypeID, int tmpAmound)
        {
            TmpMaterialTypeID = tmpMaterialTypeID;
            TmpAmount = tmpAmound;
        }
    }

    public struct TmpCreature
    {
        public int TmpMaterialTypeID { get; set; }
        public string TmpName { get; set; }
        public string TmpType { get; set; }
        public string TmpDescription { get; set; }
        public string TmpLocation { get; set; }


        public TmpCreature(int tmpMaterialTypeID, string tmpName,  string tmpType, string tmpDescription, string tmpLocation)
        {
            TmpMaterialTypeID = tmpMaterialTypeID;
            TmpName = tmpName;
            TmpType = tmpType;
            TmpDescription = tmpDescription;
            TmpLocation = tmpLocation;
        }
    }
}
