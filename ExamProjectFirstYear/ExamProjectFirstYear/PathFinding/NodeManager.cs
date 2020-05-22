using Microsoft.Xna.Framework;
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

        private List<Node> grid;
        private static int cellRowCount = 22;
        private float cellSize = 96;

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

        public  void /*List<Node>*/ InitializeGrid()
        {
            //grid = new List<Node>();

            //Checks both Y and X rows.
            for (int y = 0; y < CellRowCount; y++)
            {
                for(int x = 0; x < CellRowCount; x++)
                {
                    Node tmpNode = new Node(new Vector2(x * CellSize, y * CellSize), true);
                    Nodes[x, y] = tmpNode;

                    grid.Add(tmpNode);
                }
            }
           // return grid;
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
    }
}
