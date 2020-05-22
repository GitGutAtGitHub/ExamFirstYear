using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.PathFinding
{
    class NodeManager
    {
        #region FIELDS

        private static NodeManager instance;
        Texture2D gridSprite;
        Texture2D chosenPathgridSprite;
        private List<Node> grid;
        private static int cellRowCount = 20;
        private float cellSize = 96;
        private Stack<Node> path;

        //SKAL RETTES TIL GAMEWORLD ELLER ANDET SENERE, SÅ DET ER NEMMERE AT REDIGERE STØRRELSE SENERE.
        private Node[,] nodes = new Node[cellRowCount, cellRowCount];

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
        public float CellSize { get => cellSize; set => cellSize = value; }

        public int CellRowCount { get => cellRowCount; set => cellRowCount = value; }

        #endregion


        #region METHODS

        public  void InitializeGrid()
        {
            //Checks both Y and X rows.
            for (int y = 0; y < CellRowCount; y++)
            {
                for(int x = 0; x < CellRowCount; x++)
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
                for (int y = 0; y < CellRowCount; y++)
                {
                    for (int x = 0; x < CellRowCount; x++)
                    {
                        if (gO.GetCoordinate().X == x && gO.GetCoordinate().Y == y)
                        {
                            gO.GetObjectWidthInCellSize((SpriteRenderer)gO.GetComponent(Tag.SPRITERENDERER));

                            if(gO.Components.ContainsKey(Tag.PLATFORM))
                            {
                                Nodes[x, y].Walkable = false;
                                /*
                                Nodes[x+1, y].Walkable = false;
                                Nodes[x+2, y].Walkable = false;
                                */
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
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            PathFinder pf = new PathFinder();

            path = pf.FindPath(Nodes[0, 0], Nodes[8, 8]);

            foreach (Node node in Nodes)
            {
                spriteBatch.Draw(gridSprite, node.Position, Color.White);
            }

            foreach (Node node in path)
            {
                spriteBatch.Draw(chosenPathgridSprite, node.Position, Color.White);
            }

        }
        #endregion 
    }
}
