using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExamProjectFirstYear.PathFinding;

using ExamProjectFirstYear;
using Microsoft.Xna.Framework;
using ExamProjectFirstYear.Components;
using System.Drawing;

namespace Testing
{
    [TestClass]
    public class PathFinderTest
    {
        // DENNE KLASSE SKAL HEDDE PathFinderTest I STEDET!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        [TestMethod]
        public void TestFindMeleeTargetNode()
        {
            PathFinder pathFinder = new PathFinder();


            NodeManager.Instance.CellRowCountTwo = new TwoDimensionalSize(20, 20);
            NodeManager.Instance.InitializeGrid();

            Node startNode = NodeManager.Instance.Nodes[6, 4];

            //making five nodes unwalkable, to simulate a platform.
            //for (int i = 0; i <= 4; i++)
            //{
            //    NodeManager.Instance.Nodes[5+i, 5].Walkable = false;
            //}
            NodeManager.Instance.Nodes[5, 5].Walkable = false;
            NodeManager.Instance.Nodes[6, 5].Walkable = false;
            NodeManager.Instance.Nodes[7, 5].Walkable = false;
            NodeManager.Instance.Nodes[8, 5].Walkable = false;

            Vector2 actual = pathFinder.FindMeleeTargetNode(startNode.Position);

            //checking if it has found the 
            Vector2 expected = new Vector2((int)NodeManager.Instance.Nodes[8,4].Position.X, (int)NodeManager.Instance.Nodes[8, 4].Position.Y);

            Assert.AreEqual(expected, actual);

        }

        public void TestPopulateLevel()
        {
            NodeManager.Instance.CellRowCountTwo = new TwoDimensionalSize(20, 20);

            NodeManager.Instance.InitializeGrid();
            //NodeManager.Instance.UpdateGrid();
        }

        /// <summary>
        /// Testing if Getcoordinate returns the correct position in the node array, even though the coordinates is not specificly 96 x 1 or 2 etc...
        /// The value is parsed as an int, so it drops the decimal numbers.
        /// /// </summary>
        [TestMethod]
        public void TestGetCoordinate()
        {
            Node testNode = new Node(new Vector2(395, 285), true);

            var actual = new Vector2((int)testNode.GetCoordinate().X, (int)testNode.GetCoordinate().Y);

            var expected = new Vector2(4, 2);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestFindPath()
        {
            NodeManager.Instance.CellRowCountTwo = new TwoDimensionalSize(20, 20);

            NodeManager.Instance.InitializeGrid();
            //NodeManager.Instance.UpdateGrid();

            Node startNode = NodeManager.Instance.Nodes[1, 5];
            Node targetNode = NodeManager.Instance.Nodes[5, 7];

            PathFinder pathFinder = new PathFinder();

            var actual = pathFinder.FindPath(startNode.Position, targetNode.Position);

            List<Vector2> actual2 = new List<Vector2>();

            foreach (Node node in actual)
            {
                actual2.Add(new Vector2((int)node.GetCoordinate().X, (int)node.GetCoordinate().Y));
            }

            Stack<Node> expected = new Stack<Node>();

            List<Vector2> expected2 = new List<Vector2>();
            expected2.Add(new Vector2(2, 6));
            expected2.Add(new Vector2(3, 7));
            expected2.Add(new Vector2(4, 7));
            expected2.Add(new Vector2(5, 7));

            expected.Push(NodeManager.Instance.Nodes[2, 6]);
            expected.Push(NodeManager.Instance.Nodes[3, 7]);
            expected.Push(NodeManager.Instance.Nodes[4, 7]);
            expected.Push(NodeManager.Instance.Nodes[5, 7]);

            expected2.Reverse<Vector2>();

            //NodeManager.Instance.Nodes = new Node[NodeManager.Instance.CellRowCountTwo.width, NodeManager.Instance.CellRowCountTwo.height];

            //var actual = pathFinder.FindPath(sp, tp);

            CollectionAssert.AreEqual(actual2, expected2);
        }

        [TestMethod]
        public void TestCalculateDistance()
        {
            NodeManager.Instance.CellRowCountTwo = new TwoDimensionalSize(20, 20);

            NodeManager.Instance.InitializeGrid();
            //NodeManager.Instance.UpdateGrid();

            Node startNode = NodeManager.Instance.Nodes[1, 5];
            Node targetNode = NodeManager.Instance.Nodes[5, 7];

            PathFinder pathFinder = new PathFinder();

            int actual = pathFinder.CalculateDistance(startNode, targetNode);
            int expected = 48;

            Assert.AreEqual(actual, expected);
        }

       
    }
}
