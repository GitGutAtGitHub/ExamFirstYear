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

        #endregion


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
            GameObject.SpriteName = "ClosedJournal";
            journalRenderer = (SpriteRenderer)GameObject.GetComponent(Tag.SPRITERENDERER);
            journalHeading = GameWorld.Instance.Content.Load<SpriteFont>("JournalHeading");
        }

        public override void Start()
        {
            GameObject.Transform.Translate(new Vector2(30, 30));

            SQLiteHandler.Instance.AddRecordedBP(1, JournalID);
            SQLiteHandler.Instance.AddRecordedCreature(1, JournalID);
        }

        public override void Update(GameTime gameTime)
        {
            HandleJournal();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (journalOpen == true)
            {
                journalRenderer.SetSprite("OpenJournal");
                spriteBatch.Draw(journalRenderer.Sprite, GameObject.Transform.Position, null, Color.White, 0, journalRenderer.Origin, 1, SpriteEffects.None, 1);
                spriteBatch.DrawString(journalHeading, "Recorded Blueprints", new Vector2(GameObject.Transform.Position.X + 20, GameObject.Transform.Position.Y + 20),
                                                        Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
            }

            else if (journalOpen == false)
            {
                journalRenderer.SetSprite("ClosedJournal");
                spriteBatch.Draw(journalRenderer.Sprite, GameObject.Transform.Position, null, Color.White, 0, journalRenderer.Origin, 1, SpriteEffects.None, 1);        
            }
        }


        /// <summary>
        /// Checks whether Journal is open or closed.
        /// </summary>
        private void HandleJournal()
        {
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
        }

        /// <summary>
        /// If J key is pressed, open Journal.
        /// </summary>
        private void OpenJournal()
        {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            if (currentKeyboardState.IsKeyUp(Keys.J) && previousKeyboardState.IsKeyDown(Keys.J))
            {
                journalOpen = true;
                ShowRecordedBlueprints();
            }
        }

        /// <summary>
        /// If J key is pressed, close Journal.
        /// </summary>
        private void CloseJournal()
        {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            if (currentKeyboardState.IsKeyUp(Keys.J) && previousKeyboardState.IsKeyDown(Keys.J))
            {
                journalOpen = false;
                journalRenderer.SetSprite("ClosedJournal");
            }
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
