using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = Microsoft.Xna.Framework.Color;

namespace ExamProjectFirstYear.PathFinding
{
    /// <summary>
    /// This class is public to make Unit Testing possible.
    /// </summary>
    public class NodeManager
    {
        #region FIELDS

        public List<Node> DebugPath = new List<Node>();
        private static NodeManager instance;
        public Texture2D gridSprite;
        public Texture2D searchedSprite;
        public Texture2D chosenPathgridSprite;
        Texture2D unwalkableSprite;
        SpriteFont spriteFont;
        private List<Node> grid;
        //fix så det ikke nødve´ndigvis er uniform
        //private static int cellRowCount = 200;
        private static TwoDimensionalSize cellRowCount;
        //private int cellRowCountWidth = Bitmap.GetPixelFormatSize();
        private int cellSize = 96;
        private Stack<Node> path;
        public int debugcount = 0;

        //SKAL RETTES TIL GAMEWORLD ELLER ANDET SENERE, SÅ DET ER NEMMERE AT REDIGERE STØRRELSE SENERE.
        private Node[,] nodes;

        #endregion


        #region PROPERTIES

        public static NodeManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NodeManager();
                }

                return instance;

            }
        }

        // Needed to access it from the PathFinder class.
        public Node[,] Nodes { get => nodes; }
        public int CellSize { get => cellSize; set => cellSize = value; }

        //public int CellRowCount { get => cellRowCount; set => cellRowCount = value; }
        public TwoDimensionalSize CellRowCountTwo { get => cellRowCount; set => cellRowCount = value; }

        #endregion


        #region METHODS

        public  void InitializeGrid()
        {
            nodes = new Node[cellRowCount.width, cellRowCount.height];
            //nodes = new Node[200, 75];
            //Checks both Y and X rows.
            for (int y = 0; y < cellRowCount.height; y++)
            {
                for(int x = 0; x < cellRowCount.width; x++)
                {
                    Node tmpNode = new Node(new Vector2(x * CellSize, y * CellSize), true);
                    Nodes[x, y] = tmpNode;
                }
            }
        }

        /// <summary>
        /// Used to update the grid. It checks for objects on node positions
        /// and makes sure a node is unwalkable, if there is an object on top of it.
        /// </summary>
        public void UpdateGrid()
        {
            foreach(GameObject gO in GameWorld.Instance.GameObjects)
            {
                for (int y = 0; y < CellRowCountTwo.height; y++)
                {
                    for (int x = 0; x < CellRowCountTwo.width; x++)
                    {
                        if (gO.GetCoordinate().X == x && gO.GetCoordinate().Y == y)
                        {
                            gO.GetObjectWidthInCellSize((SpriteRenderer)gO.GetComponent(Tag.SPRITERENDERER));

                            //outdated CODE
                            if(gO.Components.ContainsKey(Tag.PLATFORM))
                            {
                                for (int i = 0; i < (int)Math.Round(gO.GetObjectWidthInCellSize((SpriteRenderer)gO.GetComponent(Tag.SPRITERENDERER))); i++)
                                {
                                    Nodes[x + i, y].Walkable = false;
                                }
                            }
                        }
                    }
                } 
            }
        }
        #endregion


        #region DRAW NODES FOR DEBUG ONLY
        public void LoadContent(ContentManager contentManager)
        {
            gridSprite = contentManager.Load<Texture2D>("NodeGridTexture");
            chosenPathgridSprite = contentManager.Load<Texture2D>("ChoosenPathTexture");
            unwalkableSprite = contentManager.Load<Texture2D>("UnWalkableNode");
            searchedSprite = contentManager.Load<Texture2D>("searched area");
            spriteFont = contentManager.Load<SpriteFont>("spritefont");

            foreach (Node node in Nodes)
            {
                node.NodeSprite = gridSprite;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            foreach (Node node in Nodes)
            {
                if (node.Walkable == true)
                {
                    spriteBatch.Draw(node.NodeSprite, node.Position, Color.White);
                }
                else
                {
                    spriteBatch.Draw(unwalkableSprite, node.Position, Color.White);
                }

                spriteBatch.DrawString(spriteFont, $"G: {node.GCost}", new Vector2((node.Position.X), (node.Position.Y)), Color.DarkRed, 0, Vector2.Zero, 1, SpriteEffects.None, 0.92f);
                spriteBatch.DrawString(spriteFont, $"H: {node.HCost}", new Vector2((node.Position.X + 60), (node.Position.Y)), Color.DarkBlue, 0, Vector2.Zero, 1, SpriteEffects.None, 0.92f);
                spriteBatch.DrawString(spriteFont, $"F: {node.FCost}", new Vector2((node.Position.X), (node.Position.Y + 80)), Color.DarkBlue, 0, Vector2.Zero, 1, SpriteEffects.None, 0.92f);

            }



        }
        #endregion 
    }
}
