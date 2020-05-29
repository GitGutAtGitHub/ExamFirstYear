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

        private void LoadBitmap()
        {
            TestLevel = (Bitmap)Image.FromFile(GetPath("TestLevel"));
        }

        public void InitializeLevel()
        {
            LoadBitmap();
            PopulateLevel(TestLevel);
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

                    int positionX = x * (int)NodeManager.Instance.CellSize;
                    int positionY = y * (int)NodeManager.Instance.CellSize;

                    //if the pixel is black
                    if (input.R == 0 && input.G == 0 && input.B == 0 && SpotOccupied[x, y] == false)
                    {
                        //add a platform
                        CreateObject(Tag.PLATFORM, positionX, positionY, x, y);
                    }

                    //if the pixel is Red
                    if (input.R == 0 && input.G == 255 && input.B == 0)
                    {
                        //add a player
                        CreateObject(Tag.PLAYER, positionX, positionY, x, y);
                    }
                }
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
                    createdObject.AddComponent(new Movement(true, 15, 700));
                    break;

                case Tag.PLATFORM:
                    createdObject.AddComponent(new Platform());
                    break;
            }

            createdObject.AddComponent(spriteRenderer);
            createdObject.Awake();
            createdObject.Start();

            createdObject.Transform.Position = new Vector2(posX, posY);

            if (tag == Tag.PLAYER)
            {
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
                for (int y = 0; y < (int)Math.Round(createdObject.GetObjectHeightInCellSize((SpriteRenderer)createdObject.GetComponent(Tag.SPRITERENDERER))); y++)
                {
                    SpotOccupied[forLoopX + x, forLoopY + y] = true;
                }
            }
        }
    }
}
