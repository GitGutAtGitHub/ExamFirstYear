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
    /// The players Journal.
    /// </summary>
    public class Journal : Component
    {
        #region Fields

        private bool journalOpen;
        private bool canOperateJournal = true;
        private bool canChangePage = true;

        private SpriteRenderer journalRenderer;

        private SpriteFont journalHeading;
        private SpriteFont journalText;

        private float playerPositionX;
        private float playerPositionY;

        private int page;

        private SQLiteHandler sQLiteHandler = GameWorld.Instance.SQLiteHandler;

        #endregion


        #region Properties

        public int JournalID { get; set; }
        public bool CanOperateJournal { get => canOperateJournal; set => canOperateJournal = value; }
        public bool CanChangePage { get => canChangePage; set => canChangePage = value; }

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
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //Determines which sprite should be drawn, depending on whether the Journal is open or closed.

            if (journalOpen == true)
            {
                journalRenderer.SetSprite("OpenJournal");
                spriteBatch.Draw(journalRenderer.Sprite, new Vector2(playerPositionX - 920, playerPositionY - 500), null, Color.White, 0, journalRenderer.Origin, 1 * GameWorld.Instance.Scale, SpriteEffects.None, journalRenderer.SpriteLayer);

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
                spriteBatch.Draw(journalRenderer.Sprite, new Vector2(playerPositionX - 920, playerPositionY - 500), null, Color.White, 0, journalRenderer.Origin, 1 * GameWorld.Instance.Scale, SpriteEffects.None, journalRenderer.SpriteLayer);
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
        /// Checks whether Journal is open or closed and closes or opens the journal
        /// depending on whether it's already open or not.
        /// </summary>
        public void HandleJournal()
        {
            if (canOperateJournal == true)
            {
                //If Journal is closed, it can be opened.
                if (journalOpen == false)
                {
                    journalOpen = true;
                    //Makes sure a page is set when the Journal opens.
                    page = 1;
                }

                //If Journal is open, it can be closed.
                else if (journalOpen == true)
                {
                    journalOpen = false;
                    journalRenderer.SetSprite("ClosedJournal");
                }

                canOperateJournal = false;
            }
        }

        /// <summary>
        /// For changing the Journal page.
        /// </summary>
        public void ChangePage()
        {
            if (canChangePage == true)
            {
                if (page == 1)
                {
                    page = 2;
                }
                else
                {
                    page = 1;
                }

                canChangePage = false;
            }
        }

        /// <summary>
        /// Draw all blueprints that the player has recorded.
        /// </summary>
        private void DrawRecordedBlueprintStrings(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(journalHeading, "Recorded Blueprints", new Vector2(playerPositionX - 800, playerPositionY - 450),
                                   Color.Black, 0, Vector2.Zero, 1 * GameWorld.Instance.Scale, SpriteEffects.None, 0.92f);

            float positionX = playerPositionX - 800;
            float positionY = (playerPositionY - 450) + 100;


            List<int> blueprintIDs = sQLiteHandler.ExecuteIntReader("BlueprintID",
                                    "RecordedBP", $"JournalID={JournalID}");

            //Create a text field for every RecordedBP in the ID list.
            foreach (int blueprintID in blueprintIDs)
            {
                string blueprintName = sQLiteHandler.SelectStringValuesWhere("Name", "Blueprint",
                                           $"ID={blueprintID}", new SQLiteConnection(sQLiteHandler.LoadSQLiteConnectionString()));
                string blueprintDesciption = sQLiteHandler.SelectStringValuesWhere("Description",
                                            "Blueprint", $"ID={blueprintID}", new SQLiteConnection(sQLiteHandler.LoadSQLiteConnectionString()));

                spriteBatch.DrawString(journalText,
                                       $"Name: {blueprintName} " +
                                       $"\nDescription: {blueprintDesciption}" +
                                       $"\nMaterials:",
                                       new Vector2(positionX, positionY),
                                       Color.Black, 0, Vector2.Zero, 1 * GameWorld.Instance.Scale, SpriteEffects.None, 0.92f);
                positionY += 75;
                positionX += 150;


                List<int> materialTypeIDs = sQLiteHandler.ExecuteIntReader("MaterialTypeID",
                                           "RequiredMaterial", $"BlueprintID={blueprintID}");

                foreach (int materialTypeID in materialTypeIDs)
                {
                    string materialName = sQLiteHandler.SelectStringValuesWhere("Name", "MaterialType", $"ID={materialTypeID}",
                                          new SQLiteConnection(sQLiteHandler.LoadSQLiteConnectionString()));
                    int materialAmount = sQLiteHandler.SelectIntValuesWhere("Amount", "RequiredMaterial", $"MaterialTypeID={materialTypeID} " +
                                        $"AND BlueprintID={blueprintID}", new SQLiteConnection(sQLiteHandler.LoadSQLiteConnectionString()));

                    spriteBatch.DrawString(journalText, $"{materialName} ({materialAmount})",
                                           new Vector2(positionX, positionY),
                                           Color.Black, 0, Vector2.Zero, 1 * GameWorld.Instance.Scale, SpriteEffects.None, 0.92f);

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
                                   Color.Black, 0, Vector2.Zero, 1 * GameWorld.Instance.Scale, SpriteEffects.None, 0.92f);

            float positionX = playerPositionX - 800;
            float positionY = (playerPositionY - 450) + 100;

            List<int> creatureIDs = sQLiteHandler.ExecuteIntReader("CreatureID",
                                   "RecordedCreature", $"JournalID={JournalID}");

            //Create a text field for every RecordedCreature in the ID list.
            foreach (int creatureID in creatureIDs)
            {
                string creatureName = sQLiteHandler.SelectStringValuesWhere("Name", "Creature",
                                           $"ID={creatureID}", new SQLiteConnection(sQLiteHandler.LoadSQLiteConnectionString()));
                string creatureType = sQLiteHandler.SelectStringValuesWhere("Type",
                                            "Creature", $"ID={creatureID}", new SQLiteConnection(sQLiteHandler.LoadSQLiteConnectionString()));
                string creatureDescription = sQLiteHandler.SelectStringValuesWhere("Description",
                                            "Creature", $"ID={creatureID}", new SQLiteConnection(sQLiteHandler.LoadSQLiteConnectionString()));
                string creatureLocation = sQLiteHandler.SelectStringValuesWhere("Location",
                                            "Creature", $"ID={creatureID}", new SQLiteConnection(sQLiteHandler.LoadSQLiteConnectionString()));
                int materialTypeID = sQLiteHandler.SelectIntValuesWhere("MaterialTypeID",
                                            "Creature", $"ID={creatureID}", new SQLiteConnection(sQLiteHandler.LoadSQLiteConnectionString()));
                string materialName = sQLiteHandler.SelectStringValuesWhere("Name", "MaterialType",
                                           $"ID={materialTypeID}", new SQLiteConnection(sQLiteHandler.LoadSQLiteConnectionString()));

                spriteBatch.DrawString(journalText,
                                        $"Name: {creatureName} ({creatureType})" +
                                        $"\nDescription: {creatureDescription} " +
                                        $"\nDrops: {materialName}" +
                                        $"\nLocation: {creatureLocation}",
                                        new Vector2(positionX, positionY),
                                        Color.Black, 0, Vector2.Zero, 1 * GameWorld.Instance.Scale, SpriteEffects.None, 0.92f);

                positionY += 190;
            }
        }

        #endregion
    }
}
