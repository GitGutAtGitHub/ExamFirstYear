using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
        private bool canOperateInventory = true;

        private SpriteRenderer inventoryRenderer;

        private SpriteFont inventoryHeading;
        private SpriteFont inventoryText;

        private float playerPositionX;
        private float playerPositionY;

        private SQLiteHandler sQLiteHandler = GameWorld.Instance.SQLiteHandler;

        #endregion


        #region Properties

        public int InventoryID { get; set; }
        public bool CanOperateInventory { get => canOperateInventory; set => canOperateInventory = value; }

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

            inventoryHeading = GameWorld.Instance.Content.Load<SpriteFont>("JournalHeading");
            inventoryText = GameWorld.Instance.Content.Load<SpriteFont>("JournalText");
        }

        public override void Start()
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            HandlePosition();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //Determines which sprite should be drawn, depending on whether the Inventory is open or closed.

            if (inventoryOpen == true)
            {
                inventoryRenderer.SetSprite("InventoryOpen");
                spriteBatch.Draw(inventoryRenderer.Sprite, new Vector2(playerPositionX - 920, playerPositionY - 330), 
                            null, Color.White, 0, inventoryRenderer.Origin, 1 * GameWorld.Instance.Scale, SpriteEffects.None, inventoryRenderer.SpriteLayer);

                //Draws the text field.
                DrawStoredMaterialStrings(spriteBatch);
            }

            else if (inventoryOpen == false)
            {
                inventoryRenderer.SetSprite("InventoryClosed");
                spriteBatch.Draw(inventoryRenderer.Sprite, new Vector2(playerPositionX - 920, playerPositionY - 330), 
                            null, Color.White, 0, inventoryRenderer.Origin, 1 * GameWorld.Instance.Scale, SpriteEffects.None, inventoryRenderer.SpriteLayer);
            }
        }

        #endregion


        #region Other methods

        /// <summary>
        /// Handles the journal position according to the players position.
        /// </summary>
        private void HandlePosition()
        {
            playerPositionX = GameWorld.Instance.Player.GameObject.Transform.Position.X;
            playerPositionY = GameWorld.Instance.Player.GameObject.Transform.Position.Y;
        }

        /// <summary>
        /// Checks whether Journal is open or closed,
        /// and makes sure it can be opened or closed accordingly.
        /// </summary>
        public void HandleInventory()
        {
            if (CanOperateInventory == true)
            {
                //If Journal is closed, it can be opened.
                if (inventoryOpen == false)
                {
                    inventoryOpen = true;
                }

                //If Journal is open, it can be closed.
                else if (inventoryOpen == true)
                {
                    inventoryOpen = false;
                }

                CanOperateInventory = false;
            }
        }

        /// <summary>
        /// Draw all the materials that the player currently have over the Inventory sprite.
        /// </summary>
        /// <param name="journalID"></param>
        private void DrawStoredMaterialStrings(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(inventoryHeading, "Inventory", new Vector2(playerPositionX - 890, playerPositionY - 300),
                                   Color.Black, 0, Vector2.Zero, 1 * GameWorld.Instance.Scale, SpriteEffects.None, 0.72f);

            float positionX = playerPositionX - 890;
            float positionY = (playerPositionY - 310) + 90;

            List<int> materialTypeIDs = sQLiteHandler.ExecuteIntReader("MaterialTypeID", "StoredMaterial", 
                                        $"InventoryID={InventoryID}");

            //Create a text field for every MaterialType in the ID list.
            foreach (int materialTypeID in materialTypeIDs)
            {
                string materialName = sQLiteHandler.SelectStringValuesWhere("Name", "MaterialType",
                                    $"ID={materialTypeID}", new SQLiteConnection(sQLiteHandler.LoadSQLiteConnectionString()));
                int storedAmount    = sQLiteHandler.SelectIntValuesWhere("Amount", "StoredMaterial",
                                    $"MaterialTypeID={materialTypeID}", new SQLiteConnection(sQLiteHandler.LoadSQLiteConnectionString()));

                spriteBatch.DrawString(inventoryText,
                                       $"{materialName}: {storedAmount}",
                                       new Vector2(positionX, positionY),
                                       Color.Black, 0, Vector2.Zero, 1 * GameWorld.Instance.Scale, SpriteEffects.None, 0.72f);

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
}
