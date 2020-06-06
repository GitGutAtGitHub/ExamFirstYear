using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExamProjectFirstYear.Components
{
    /// <summary>
    /// The players Journal.
    /// </summary>
    public class Journal : Component
    {
        #region Fields

        private bool journalOpen;

        private KeyboardState previousKeyboardState;
        private KeyboardState currentKeyboardState;

        private SpriteRenderer journalRenderer;

        private SpriteFont journalHeading;
        private SpriteFont journalText;

        private float playerPositionX;
        private float playerPositionY;

        private int page;

        #endregion


        #region Properties

        public int JournalID { get; set; }

        public List<int> RecordedBlueprintIDs { get; set; } = new List<int>();
        public List<int> RecordedCreatureIDs { get; set; } = new List<int>();

        public TmpJournal TmpJournal { get; private set; }

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


        #region Override methods

        public override Tag ToEnum()
        {
            return Tag.JOURNAL;
        }

        public override void Awake()
        {
            GameObject.Tag = Tag.JOURNAL;
            GameObject.SpriteName = "ClosedJournal";

            journalRenderer = (SpriteRenderer)GameObject.GetComponent(Tag.SPRITERENDERER);

            journalHeading = GameWorld.Instance.Content.Load<SpriteFont>("JournalHeading");
            journalText = GameWorld.Instance.Content.Load<SpriteFont>("JournalText");
        }

        public override void Start()
        {
            if (!RecordedBlueprintIDs.Contains(1))
            {
                GameWorld.Instance.sQLiteHandler.AddRecordedBP(1, JournalID);
            }
        }

        public override void Update(GameTime gameTime)
        {
            HandlePosition();
            HandleJournal();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //Determines which sprite should be drawn, depending on whether the Journal is open or closed.

            if (journalOpen == true)
            {
                journalRenderer.SetSprite("OpenJournal");
                spriteBatch.Draw(journalRenderer.Sprite, new Vector2(playerPositionX - 920, playerPositionY - 500), null, Color.White, 0, journalRenderer.Origin, 1, SpriteEffects.None, journalRenderer.SpriteLayer);

                //If the page is 1, draws the text field for RecordedBP.
                if (page == 1)
                {
                    DrawRecordedBlueprintStrings(spriteBatch);
                }

                //If the page is 2, draws the text field for RecordedCreature.
                else if (page == 2)
                {
                    DrawRecordedCreatureStrings(spriteBatch);
                }
            }

            else if (journalOpen == false)
            {
                journalRenderer.SetSprite("ClosedJournal");
                spriteBatch.Draw(journalRenderer.Sprite, new Vector2(playerPositionX - 920, playerPositionY - 500), null, Color.White, 0, journalRenderer.Origin, 1, SpriteEffects.None, journalRenderer.SpriteLayer);
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
        private void HandleJournal()
        {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            //If Journal is closed, it can be opened.
            if (journalOpen == false)
            {
                OpenJournal();
            }

            //If Journal is open, it can be closed.
            else if (journalOpen == true)
            {
                CloseJournal();
            }

            //For changing the Journal page.
            if (currentKeyboardState.IsKeyUp(Keys.D1) && previousKeyboardState.IsKeyDown(Keys.D1))
            {
                page = 1;
            }

            else if (currentKeyboardState.IsKeyUp(Keys.D2) && previousKeyboardState.IsKeyDown(Keys.D2))
            {
                page = 2;
            }
        }

        /// <summary>
        /// If J key is pressed, open Journal.
        /// </summary>
        private void OpenJournal()
        {
            if (currentKeyboardState.IsKeyUp(Keys.J) && previousKeyboardState.IsKeyDown(Keys.J))
            {
                journalOpen = true;

                //Makes sure a page is set when the Journal opens.
                page = 1;
            }
        }

        /// <summary>
        /// If J key is pressed, close Journal.
        /// </summary>
        private void CloseJournal()
        {
            if (currentKeyboardState.IsKeyUp(Keys.J) && previousKeyboardState.IsKeyDown(Keys.J))
            {
                journalOpen = false;
                journalRenderer.SetSprite("ClosedJournal");
            }
        }

        /// <summary>
        /// Draw all blueprints that the player has recorded.
        /// </summary>
        private void DrawRecordedBlueprintStrings(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(journalHeading, "Recorded Blueprints", new Vector2(playerPositionX - 800, playerPositionY - 450),
                                   Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0.92f);

            TmpBlueprint tmpBlueprint;
            TmpRequiredMaterial tmpRequiredMaterial;
            TmpMaterialType tmpMaterialType;

            float positionX = playerPositionX - 800;
            float positionY = (playerPositionY - 450) + 100;

            //Create a text field for every RecordedBP in the ID list.
            foreach (int blueprintID in RecordedBlueprintIDs)
            {
                tmpBlueprint = GameWorld.Instance.sQLiteHandler.GetBlueprint(blueprintID);
                tmpRequiredMaterial = GameWorld.Instance.sQLiteHandler.GetRequiredMaterial(blueprintID);
                tmpMaterialType = GameWorld.Instance.sQLiteHandler.GetMaterialType(tmpRequiredMaterial.TmpMaterialTypeID);

                spriteBatch.DrawString(journalText, $"Name: {tmpBlueprint.TmpName} \nDescription: {tmpBlueprint.TmpDescription}" +
                                       $"\nMaterials: {tmpMaterialType.TmpName} ({tmpRequiredMaterial.TmpAmount})",
                                       new Vector2(positionX, positionY),
                                       Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0.92f);

                positionY += 150;
            }
        }

        /// <summary>
        /// Show all creatures that the player has recorded.
        /// </summary>
        private void DrawRecordedCreatureStrings(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(journalHeading, "Recorded Creatures", new Vector2(playerPositionX - 800, playerPositionY - 450),
                                   Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0.92f);

            TmpCreature tmpCreature;
            TmpMaterialType tmpMaterialType;

            float positionX = playerPositionX - 800;
            float positionY = (playerPositionY - 450) + 100;

            //Create a text field for every RecordedCreature in the ID list.
            foreach (int creatureID in RecordedCreatureIDs)
            {
                tmpCreature = GameWorld.Instance.sQLiteHandler.GetCreature(creatureID);
                tmpMaterialType = GameWorld.Instance.sQLiteHandler.GetMaterialType(tmpCreature.TmpMaterialTypeID);

                spriteBatch.DrawString(journalText, $"Name: {tmpCreature.TmpName} ({tmpCreature.TmpType})" +
                                        $"\nDescription: {tmpCreature.TmpDescription} " +
                                        $"\nDrops: {tmpMaterialType.TmpName}\nLocation: {tmpCreature.TmpLocation}",
                                        new Vector2(positionX, positionY),
                                        Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0.92f);

                positionY += 190;
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

    /// <summary>
    /// Used when fetching the Journal table from SQLite.
    /// </summary>
    public struct TmpJournal
    {
        public int TmpJournalID { get; set; }
        public int TmpInventoryID { get; set; }
        public int TmpHealth { get; set; }
        public int TmpOpenDoor { get; set; }
        public int TmpPositionX { get; set; }
        public int TmpPositionY { get; set; }
        public int TmpMana { get; set; }

        /// <summary>
        /// Constructor for the TmpJournal struct.
        /// </summary>
        /// <param name="tmpJournalID"></param>
        /// <param name="tmpInventoryID"></param>
        /// <param name="tmpHealth"></param>
        /// <param name="tmpPositionX"></param>
        /// <param name="tmpPositionY"></param>
        /// <param name="tmpOpenDoor"></param>
        /// <param name="tmpMana"></param>
        public TmpJournal(int tmpJournalID, int tmpInventoryID, int tmpHealth, int tmpPositionX, int tmpPositionY, int tmpOpenDoor, int tmpMana)
        {
            TmpJournalID = tmpJournalID;
            TmpInventoryID = tmpInventoryID;
            TmpHealth = tmpHealth;
            TmpPositionX = tmpPositionX;
            TmpPositionY = tmpPositionY;
            TmpOpenDoor = tmpOpenDoor;
            TmpMana = tmpMana;
        }
    }
}
