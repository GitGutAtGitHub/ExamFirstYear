using ExamProjectFirstYear.Components;
using ExamProjectFirstYear.Components.PlayerComponents;
using ExamProjectFirstYear.PathFinding;
using Microsoft.Xna.Framework;
using System;
using System.Drawing;
//using Microsoft.Xna.Framework;


namespace ExamProjectFirstYear
{
    /// <summary>
    /// public because of unitTest
    /// </summary>
    public class LevelManager
    {
        //event for checking if the level is done populating.
        //Used for setting the enemies target, and making sure the player is intantiated when the target filed is set.
        public delegate void LevelInitializationDoneHandler();
        public event LevelInitializationDoneHandler LevelInitializationDoneEvent;
        
        public Door Door { get; private set; }


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

        //public for unit testing
        public Bitmap TestLevel;

        Bitmap PlatformSection;

        //public because of unit tests
        public void LoadBitmap()
        {
            TestLevel = (Bitmap)Image.FromFile(GetPath("TestLevel"));
            PlatformSection = (Bitmap)Image.FromFile(GetPath("PlatformSection"));
        }

        public void InitializeLevel()
        {
            LoadBitmap();
            //PopulateLevel(PlatformSection);
            PopulateLevel(TestLevel);


            NodeManager.Instance.CellRowCountTwo = new TwoDimensionalSize(PlatformSection.Width, PlatformSection.Height);
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

                    //if the pixel is black - Place platform
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

                    //if the pixel is orange
                    if (input.R == 255 && input.G == 99 && input.B == 0)
                    {
                        //add a platform
                        CreateObject(Tag.MEELEEENEMY, x * (int)NodeManager.Instance.CellSize, y * (int)NodeManager.Instance.CellSize, x, y);
                    }

                    //if the pixel is slightly red
                    if (input.R == 255 && input.G == 100 && input.B == 100)
                    {
                        //add a flying
                        CreateObject(Tag.RANGEDENEMY, x * (int)NodeManager.Instance.CellSize, y * (int)NodeManager.Instance.CellSize, x, y);
                    }

                    //if the pixel is purple-blue
                    if (input.R == 100 && input.G == 100 && input.B == 255)
                    {
                        CreateObject(Tag.DOOR, x * (int)NodeManager.Instance.CellSize, y * (int)NodeManager.Instance.CellSize, x, y);
                    }
                }
            }
            // The event is raised. It calls the method AddTarget,
            // which is added to each enemy in the CreateObject method.

            if (LevelInitializationDoneEvent!= null)
            {
                LevelInitializationDoneEvent();
            }

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
                    createdObject.AddComponent(new Movement(true, 900));
                    createdObject.AddComponent(new LightSource(2.75f, true));
                    createdObject.AddComponent(new Jump(35));
                    createdObject.AddComponent(new RangedAttack());
                    createdObject.AddComponent(new AttackMelee());
                    break;

                case Tag.PLATFORM:
                    //spriteRenderer.Origin = new Vector2(createdObject.Transform.Position.X, createdObject.Transform.Position.Y);
                    createdObject.AddComponent(new Platform());
                    break;

                case Tag.DOOR:
                    createdObject.AddComponent(Door = new Door());
                    break;

                case Tag.FLYINGENEMY:
                    createdObject.Tag = Tag.FLYINGENEMY;
                    createdObject.AddComponent(new FlyingEnemy());
                    createdObject.AddComponent(new LightSource(0.5f, true));
                    // Subscribes each flying enemy to an event, that calls the method AddTarget once the event is raised.
                    LevelInitializationDoneEvent += ((FlyingEnemy)(createdObject.GetComponent(Tag.FLYINGENEMY))).AddTarget;
                    break;

                case Tag.MEELEEENEMY:
                    createdObject.AddComponent(new MeleeEnemy());
                    createdObject.AddComponent(new LightSource(0.5f, true));
                    createdObject.AddComponent(new Movement(true, 900));
                    // Subscribes each flying enemy to an event, that calls the method AddTarget once the event is raised.
                    //createdObject.AddComponent(new Movement(true, 35, 900));
                    LevelInitializationDoneEvent += ((MeleeEnemy)(createdObject.GetComponent(Tag.MEELEEENEMY))).AddTarget;
                    break;

                case Tag.RANGEDENEMY:
                    createdObject.AddComponent(new RangedEnemy());
                    createdObject.AddComponent(new LightSource(0.5f, true));
                    // Subscribes each flying enemy to an event, that calls the method AddTarget once the event is raised.
                    LevelInitializationDoneEvent += ((RangedEnemy)(createdObject.GetComponent(Tag.RANGEDENEMY))).AddTarget;
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
                collider.AttachListener((Jump)createdObject.GetComponent(Tag.JUMP));
            }

            else if (tag == Tag.MEELEEENEMY)
            {
                spriteRenderer.Origin = new Vector2(spriteRenderer.Sprite.Width / 2, spriteRenderer.Sprite.Height / 2);
                //spriteRenderer.Origin = new Vector2(spriteRenderer.Sprite.Width / 2, -spriteRenderer.Sprite.Height);
                collider = new Collider(spriteRenderer, (MeleeEnemy)createdObject.GetComponent(Tag.MEELEEENEMY)) { CheckCollisionEvents = true };
                createdObject.AddComponent(new AttackMelee());
            }

            else if (tag == Tag.FLYINGENEMY)
            {
                collider = new Collider(spriteRenderer, (FlyingEnemy)createdObject.GetComponent(Tag.FLYINGENEMY)) { CheckCollisionEvents = true };
               
            }

            else if (tag == Tag.RANGEDENEMY)
            {
                spriteRenderer.Origin = new Vector2(spriteRenderer.Sprite.Width / 2, spriteRenderer.Sprite.Height / 2 - 20);
                collider = new Collider(spriteRenderer, (RangedEnemy)createdObject.GetComponent(Tag.RANGEDENEMY)) { CheckCollisionEvents = true };
                createdObject.AddComponent(new RangedAttack());
            }

            //else if (tag != Tag.PLATFORM)
            //{
            //    spriteRenderer.Origin = new Vector2(spriteRenderer.Sprite.Width / 2, spriteRenderer.Sprite.Height / 2);
            //    collider = new Collider(spriteRenderer);
            //}

            else if (tag == Tag.DOOR)
            {
                collider = new Collider(spriteRenderer, (Door)createdObject.GetComponent(Tag.DOOR)) { CheckCollisionEvents = true };
            }


            else
            {
                collider = new Collider(spriteRenderer);
            }

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
