using ExamProjectFirstYear.Components;
using ExamProjectFirstYear.PathFinding;
using Microsoft.Xna.Framework;
using System;
using System.Drawing;
//using Microsoft.Xna.Framework;


namespace ExamProjectFirstYear
{
    class LevelManager
    {
        //event for checking if the level is done populating.
        //Used for setting the enemies target, and making sure the player is intantiated when the target filed is set.
        public delegate void LevelInitializationDoneHandler();
        public event LevelInitializationDoneHandler LevelInitializationDoneEvent;


        protected virtual void OnLevelInitializationDoneEvent()
        {
            if (LevelInitializationDoneEvent != null)
            {
                LevelInitializationDoneEvent();
            }
        }



        //string path2 = "..\\..\\" + "Levels\\TestLevel.bmp";

        private static LevelManager instance;
        private bool[,] SpotOccupied;

        public static LevelManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LevelManager();
                }

                return instance;
            }
        }

        public string GetPath(string filename)
        {
            return Environment.CurrentDirectory + ($"\\Levels\\{filename}.bmp");

        }

        Bitmap TestLevel;
        Bitmap PlatformSection;

        private void LoadBitmap()
        {
            TestLevel = (Bitmap)Image.FromFile(GetPath("TestLevel"));
            PlatformSection = (Bitmap)Image.FromFile(GetPath("PlatformSection"));
        }

        public void InitializeLevel()
        {
            LoadBitmap();
            PopulateLevel(PlatformSection);

            NodeManager.Instance.CellRowCountTwo = new TwoDimensionalSize(PlatformSection.Width, PlatformSection.Height);

            //PopulateLevel(TestLevel);
        }

        /// <summary>
        /// Scans the entire bitmap and places objects depending on the color of a pixel
        /// </summary>
        /// <param name="level"></param>
        private void PopulateLevel(Bitmap level)
        {

            SpotOccupied = new bool[level.Width, level.Width];

            for (int y = 0; y < level.Height; y++)
            {
                for (int x = 0; x < level.Width; x++)
                {
                    //Saves the current pixels color as a Color field.
                    System.Drawing.Color input = level.GetPixel(x, y);

                    //if the pixel is black
                    if (input.R == 0 && input.G == 0 && input.B == 0 && SpotOccupied[x, y] == false)
                    {
                        //add a platform
                        CreateObject(Tag.PLATFORM, x * (int)NodeManager.Instance.CellSize, y * (int)NodeManager.Instance.CellSize, x, y);
                    }

                    //if the pixel is Red
                    if (input.R == 255 && input.G == 0 && input.B == 0)
                    {
                        //add a flying
                        CreateObject(Tag.FLYINGENEMY, x * (int)NodeManager.Instance.CellSize, y * (int)NodeManager.Instance.CellSize, x, y);
                    }

                    //if the pixel is green
                    if (input.R == 0 && input.G == 255 && input.B == 0)
                    {
                        //add a platform
                        CreateObject(Tag.PLAYER, x * (int)NodeManager.Instance.CellSize, y * (int)NodeManager.Instance.CellSize, x, y);
                    }
                }
            }
            // The event is raised. It calls the method AddTarget,
            // which is added to each enemy in the CreateObject method.
            LevelInitializationDoneEvent();
        }



        public void CreateObject(Tag tag, int posX, int posY, int forLoopX, int forLoopY)
        {
            GameObject createdObject = new GameObject();
            SpriteRenderer spriteRenderer = new SpriteRenderer();
            Collider collider;

            switch (tag)
            {
                case Tag.PLAYER:
                    createdObject.AddComponent(GameWorld.Instance.player);
                    //use this if tall jump
                    createdObject.AddComponent(new Movement(true, 35, 900));
                    break;

                case Tag.PLATFORM:
                    createdObject.AddComponent(new Platform());
                    break;

                case Tag.FLYINGENEMY:
                    createdObject.AddComponent(new FlyingEnemy());
                    // Subscribes each flying enemy to an event, that calls the method AddTarget once the event is raised.
                    LevelInitializationDoneEvent += ((FlyingEnemy)(createdObject.GetComponent(Tag.FLYINGENEMY))).AddTarget;
                    break;

                //case Tag.JOURNAL:
                //    createdObject.AddComponent(GameWorld.Instance.journal);
                //    spriteRenderer.SpriteLayer = 0.9f;
                //    break;

                //case Tag.INVENTORY:
                //    createdObject.AddComponent(GameWorld.Instance.inventory);
                //    spriteRenderer.SpriteLayer = 0.8f;
                //    break;

                //default:
                //    spriteRenderer.SpriteLayer = 0.6f;
                //    break;

            }

            createdObject.AddComponent(spriteRenderer);
            createdObject.Awake();
            createdObject.Start();

            createdObject.Transform.Position = new Vector2(posX, posY);

            if (tag == Tag.PLAYER)
            {
                spriteRenderer.Origin = new Vector2(spriteRenderer.Sprite.Width / 2, spriteRenderer.Sprite.Height / 2);
                collider = new Collider(spriteRenderer, GameWorld.Instance.player) { CheckCollisionEvents = true };
                collider.AttachListener((Movement)createdObject.GetComponent(Tag.MOVEMENT));
            }
            else
            {
                collider = new Collider(spriteRenderer);
            }

            //Skulle sørge for at collider også advarer Movement component men det virker ikke

            createdObject.AddComponent(collider);

            GameWorld.Instance.Colliders.Add(collider);
            GameWorld.Instance.GameObjects.Add(createdObject);

            //Makes sure that it doesn't create a new object right next to it, if the object is bigger than one cell.
            for (int x = 0; x < (int)Math.Round(createdObject.GetObjectWidthInCellSize((SpriteRenderer)createdObject.GetComponent(Tag.SPRITERENDERER))); x++)
            {
                for (int y = 0; y <= (int)Math.Round(createdObject.GetObjectHeightInCellSize((SpriteRenderer)createdObject.GetComponent(Tag.SPRITERENDERER))); y++)
                {
                    SpotOccupied[forLoopX + x, forLoopY + y] = true;
                }
            }
        }
    }
}
