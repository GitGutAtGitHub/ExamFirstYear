using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
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

            float positionX = playerPositionX - 800;
            float positionY = (playerPositionY - 450) + 100;


            List<int> blueprintIDs = GameWorld.Instance.sQLiteHandler.ExecuteIntReader("BlueprintID",
                                   "RecordedBP", $"JournalID={GameWorld.Instance.journal.JournalID}");

            //Create a text field for every RecordedBP in the ID list.
            foreach (int blueprintID in blueprintIDs)
            {
                string blueprintName       = GameWorld.Instance.sQLiteHandler.SelectStringValuesWhere("Name", "Blueprint",
                                           $"ID={blueprintID}", new SQLiteConnection(GameWorld.Instance.sQLiteHandler.LoadSQLiteConnectionString()));
                string blueprintDesciption = GameWorld.Instance.sQLiteHandler.SelectStringValuesWhere("Description",
                                            "Blueprint", $"ID={blueprintID}", new SQLiteConnection(GameWorld.Instance.sQLiteHandler.LoadSQLiteConnectionString()));

                spriteBatch.DrawString(journalText, 
                                       $"Name: {blueprintName} " +
                                       $"\nDescription: {blueprintDesciption}" +
                                       $"\nMaterials:",
                                       new Vector2(positionX, positionY),
                                       Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0.92f);
                positionY += 75;
                positionX += 150;


                List<int> materialTypeIDs = GameWorld.Instance.sQLiteHandler.ExecuteIntReader("MaterialTypeID",
                                   "RequiredMaterial", $"BlueprintID={blueprintID}");

                foreach (int materialTypeID in materialTypeIDs)
                {
                    string materialName = GameWorld.Instance.sQLiteHandler.SelectStringValuesWhere("Name", "MaterialType", $"ID={materialTypeID}", new SQLiteConnection(GameWorld.Instance.sQLiteHandler.LoadSQLiteConnectionString()));
                    int materialAmount  = GameWorld.Instance.sQLiteHandler.SelectIntValuesWhere("Amount", "RequiredMaterial", $"MaterialTypeID={materialTypeID} AND BlueprintID={blueprintID}", new SQLiteConnection(GameWorld.Instance.sQLiteHandler.LoadSQLiteConnectionString()));

                    spriteBatch.DrawString(journalText, $"{materialName} ({materialAmount})", 
                                           new Vector2(positionX, positionY),
                                           Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0.92f);

                    positionY += 40;
                }

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

            float positionX = playerPositionX - 800;
            float positionY = (playerPositionY - 450) + 100;

            List<int> creatureIDs = GameWorld.Instance.sQLiteHandler.ExecuteIntReader("CreatureID",
                                   "RecordedCreature", $"JournalID={GameWorld.Instance.journal.JournalID}");

            //Create a text field for every RecordedCreature in the ID list.
            foreach (int creatureID in creatureIDs)
            {
                string creatureName        = GameWorld.Instance.sQLiteHandler.SelectStringValuesWhere("Name", "Creature",
                                           $"ID={creatureID}", new SQLiteConnection(GameWorld.Instance.sQLiteHandler.LoadSQLiteConnectionString()));
                string creatureType        = GameWorld.Instance.sQLiteHandler.SelectStringValuesWhere("Type",
                                            "Creature", $"ID={creatureID}", new SQLiteConnection(GameWorld.Instance.sQLiteHandler.LoadSQLiteConnectionString()));
                string creatureDescription = GameWorld.Instance.sQLiteHandler.SelectStringValuesWhere("Description",
                                            "Creature", $"ID={creatureID}", new SQLiteConnection(GameWorld.Instance.sQLiteHandler.LoadSQLiteConnectionString()));
                string creatureLocation    = GameWorld.Instance.sQLiteHandler.SelectStringValuesWhere("Location",
                                            "Creature", $"ID={creatureID}", new SQLiteConnection(GameWorld.Instance.sQLiteHandler.LoadSQLiteConnectionString()));
                int materialTypeID         = GameWorld.Instance.sQLiteHandler.SelectIntValuesWhere("MaterialTypeID",
                                            "Creature", $"ID={creatureID}", new SQLiteConnection(GameWorld.Instance.sQLiteHandler.LoadSQLiteConnectionString()));
                string materialName        = GameWorld.Instance.sQLiteHandler.SelectStringValuesWhere("Name", "MaterialType",
                                           $"ID={materialTypeID}", new SQLiteConnection(GameWorld.Instance.sQLiteHandler.LoadSQLiteConnectionString()));

                spriteBatch.DrawString(journalText, 
                                        $"Name: {creatureName} ({creatureType})" +
                                        $"\nDescription: {creatureDescription} " +
                                        $"\nDrops: {materialName}" +
                                        $"\nLocation: {creatureLocation}",
                                        new Vector2(positionX, positionY),
                                        Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0.92f);

                positionY += 190;
            }
        }

        #endregion
    }
}
