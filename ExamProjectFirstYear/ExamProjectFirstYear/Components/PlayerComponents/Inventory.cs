using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        private SpriteFont inventoryHeading;
        private SpriteFont inventoryText;

        private float playerPositionX;
        private float playerPositionY;

        #endregion


        #region Properties

        public int InventoryID { get; set; }
        public List<int> MaterialTypeIDs { get; set; } = new List<int>();

        #endregion


        #region Constructors

        /// <summary>
        /// Constructor for the Inventory.
        /// </summary>
        /// <param name="inventoryID"></param>
        public Inventory(int inventoryID)
        {
            InventoryID = inventoryID;
        }

        #endregion


        #region Override methods

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
            inventoryHeading = GameWorld.Instance.Content.Load<SpriteFont>("JournalHeading");
            inventoryText = GameWorld.Instance.Content.Load<SpriteFont>("JournalText");
        }

        public override void Start()
        {
            GameObject.Transform.Translate(new Vector2(30, 180));
        }
        public override void Update(GameTime gameTime)
        {
            HandlePosition();
            HandleInventory();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //Determines which sprite should be drawn, depending on whether the Inventory is open or closed.

            if (inventoryOpen == true)
            {
                inventoryRenderer.SetSprite("InventoryOpen");
                spriteBatch.Draw(inventoryRenderer.Sprite, new Vector2(playerPositionX - 920, playerPositionY - 330), null, Color.White, 0, inventoryRenderer.Origin, 1, SpriteEffects.None, inventoryRenderer.SpriteLayer);
                
                //Draws the text field.
                DrawStoredMaterialStrings(spriteBatch);
            }

            else if (inventoryOpen == false)
            {
                inventoryRenderer.SetSprite("InventoryClosed");
                spriteBatch.Draw(inventoryRenderer.Sprite, new Vector2(playerPositionX - 920, playerPositionY - 330), null, Color.White, 0, inventoryRenderer.Origin, 1, SpriteEffects.None, inventoryRenderer.SpriteLayer);
            }
        }

        #endregion


        #region Other methods

        /// <summary>
        /// Handles the journal position according to the players position.
        /// </summary>
        private void HandlePosition()
        {
            playerPositionX = GameWorld.Instance.player.GameObject.Transform.Position.X;
            playerPositionY = GameWorld.Instance.player.GameObject.Transform.Position.Y;
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
            }
        }

        /// <summary>
        /// Draw all the materials that the player currently have over the Inventory sprite.
        /// </summary>
        /// <param name="journalID"></param>
        private void DrawStoredMaterialStrings(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(inventoryHeading, "Inventory", new Vector2(playerPositionX - 890, playerPositionY - 300),
                                   Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0.72f);

            TmpMaterialType tmpMaterialType;
            TmpStoredMaterial tmpStoredMaterial;
            TmpInventory tmpInventory;

            float positionX = playerPositionX - 890;
            float positionY = (playerPositionY - 310) + 90;

            //Create a text field for every MaterialType in the ID list.
            foreach (int materialTypeID in MaterialTypeIDs)
            {
                tmpInventory = SQLiteHandler.Instance.GetInventory();
                tmpStoredMaterial = SQLiteHandler.Instance.GetStoredMaterial(materialTypeID, tmpInventory.TmpID);
                tmpMaterialType = SQLiteHandler.Instance.GetMaterialType(materialTypeID);

                spriteBatch.DrawString(inventoryText, $"{tmpMaterialType.TmpName}: {tmpStoredMaterial.TmpAmount}",
                                       new Vector2(positionX, positionY),
                                       Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0.72f);

                positionY += 60;
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
    /// Used when fetching the StoredMaterial table from SQLite.
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
    /// Used when fetching the Inventory table from SQLite.
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
