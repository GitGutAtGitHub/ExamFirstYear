using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
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
        #region Fields

        private bool inventoryOpen;

        private KeyboardState previousKeyboardState;
        private KeyboardState currentKeyboardState;

        private SpriteRenderer inventoryRenderer;

        #endregion


        #region Properties

        public int InventoryID { get; set; }
        public List<int> MaterialTypeIDs { get; set; } = new List<int>();

        #endregion


        #region Constructors

        public Inventory(int inventoryID)
        {
            InventoryID = inventoryID;
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
            GameObject.SpriteName = "InventoryClosed";
            inventoryRenderer = (SpriteRenderer)GameObject.GetComponent(Tag.SPRITERENDERER);
            MaterialTypeIDs.Add(1);
        }

        public override void Start()
        {
            GameObject.Transform.Translate(new Vector2(30, 180));
        }

        public override void Update(GameTime gameTime)
        {
            HandleInventory();
        }

        /// <summary>
        /// Checks whether Journal is open or closed.
        /// </summary>
        private void HandleInventory()
        {
            //If Journal is closed, it can be opened.
            if (inventoryOpen == false)
            {
                OpenInventory();
            }

            //If Journal is open, it can be closed.
            else if (inventoryOpen == true)
            {
                CloseInventory();
            }
        }

        /// <summary>
        /// If J key is pressed, open Inventory.
        /// </summary>
        private void OpenInventory()
        {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            if (currentKeyboardState.IsKeyUp(Keys.I) && previousKeyboardState.IsKeyDown(Keys.I))
            {
                inventoryOpen = true;
                inventoryRenderer.SetSprite("InventoryOpen");
                ShowStoredMaterials();
            }
        }

        /// <summary>
        /// If J key is pressed, close Inventory.
        /// </summary>
        private void CloseInventory()
        {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            if (currentKeyboardState.IsKeyUp(Keys.I) && previousKeyboardState.IsKeyDown(Keys.I))
            {
                inventoryOpen = false;
                inventoryRenderer.SetSprite("InventoryClosed");
            }
        }

        /// <summary>
        /// Show all the materials that the player currently have.
        /// </summary>
        /// <param name="journalID"></param>
        public void ShowStoredMaterials()
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
