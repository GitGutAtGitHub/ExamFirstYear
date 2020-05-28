using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.Components
{
    /// <summary>
    /// The players Inventory.
    /// </summary>
    public class Inventory : Component
    {
        #region Properties

        public int ID { get; set; }
        public List<int> MaterialTypeIDs { get; set; } = new List<int>();

        #endregion


        #region Constructors

        public Inventory(int iD)
        {
            ID = iD;
        }

        #endregion


        #region Methods

        public override Tag ToEnum()
        {
            return Tag.INVENTORY;
        }

        public override void Awake()
        {
            GameObject.Tag = Tag.INVENTORY;
        }

        /// <summary>
        /// Show all the materials that the player currently have.
        /// </summary>
        /// <param name="journalID"></param>
        public void ShowStoredMaterials(int journalID)
        {
            TmpMaterialType tmpMaterialType;
            TmpStoredMaterial tmpStoredMaterial;
            TmpInventory tmpInventory;

            foreach (int materialTypeID in MaterialTypeIDs)
            {
                tmpInventory = SQLiteHandler.Instance.GetInventory();
                tmpStoredMaterial = SQLiteHandler.Instance.GetStoredMaterial(materialTypeID, tmpInventory.TmpID);
                tmpMaterialType = SQLiteHandler.Instance.GetMaterialType(materialTypeID);

                Console.WriteLine($"Material: {tmpMaterialType.TmpName}");
                Console.WriteLine($"Amount: {tmpStoredMaterial.TmpAmount}");
                Console.WriteLine();
            }   
        }

        #endregion
    }

    /// <summary>
    /// Used when fetching the MaterialType table from SQLite.
    /// </summary>
    public struct TmpMaterialType
    {
        public string TmpName { get; set; }

        /// <summary>
        /// Constructor for the TmpMaterialType struct.
        /// </summary>
        /// <param name="tmpName"></param>
        public TmpMaterialType(string tmpName)
        {
            TmpName = tmpName;
        }
    }

    /// <summary>
    /// Constructor for the TmpStoredMaterial struct.
    /// </summary>
    public struct TmpStoredMaterial
    {
        public int TmpAmount { get; set; }
        public int TmpSlot { get; set; }


        /// <summary>
        /// Constructor for the TmpStoredMaterial struct.
        /// </summary>
        /// <param name="tmpName"></param>
        public TmpStoredMaterial(int tmpAmount, int tmpSlot)
        {
            TmpAmount = tmpAmount;
            TmpSlot = tmpSlot;
        }
    }

    /// <summary>
    /// Constructor for the TmpInventory struct.
    /// </summary>
    public struct TmpInventory
    {
        public int TmpID { get; set; }


        /// <summary>
        /// Constructor for the TmpInventory struct.
        /// </summary>
        /// <param name="tmpName"></param>
        public TmpInventory(int tmpID)
        {
            TmpID = tmpID;
        }
    }
}
