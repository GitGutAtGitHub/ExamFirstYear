using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExamProjectFirstYear.PathFinding;
using ExamProjectFirstYear;
using Microsoft.Xna.Framework;

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

        }

        [TestMethod]
        public void TestCellRowCount()
        {

        }

        [TestMethod]
        public void TestFindPath()
        {
            PathFinder pathFinder = new PathFinder();

            Vector2 sp = new Vector2(1, 1);
            Vector2 tp = new Vector2(5, 1);

            Stack<Node> expected = new Stack<Node>();

            Node node1 = new Node(new Vector2(1 * NodeManager.Instance.CellSize, 1 * NodeManager.Instance.CellSize), true);
            Node node2 = new Node(new Vector2(2 * NodeManager.Instance.CellSize, 1 * NodeManager.Instance.CellSize), true);
            Node node3 = new Node(new Vector2(3 * NodeManager.Instance.CellSize, 1 * NodeManager.Instance.CellSize), true);
            Node node4 = new Node(new Vector2(4 * NodeManager.Instance.CellSize, 1 * NodeManager.Instance.CellSize), true);
            Node node5 = new Node(new Vector2(5 * NodeManager.Instance.CellSize, 1 * NodeManager.Instance.CellSize), true);
            
            //NodeManager.Instance.Nodes = new Node[NodeManager.Instance.CellRowCountTwo.width, NodeManager.Instance.CellRowCountTwo.height];

            expected.Push(node1);
            expected.Push(node2);
            expected.Push(node3);
            expected.Push(node4);
            expected.Push(node5);
            expected.Reverse();

            //Stack<Node> actual = new Stack<Node>();

            var actual = pathFinder.FindPath(sp, tp);

            CollectionAssert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void TestCalculateDistance()
        {
            PathFinder pathFinder = new PathFinder();
            // These nodes are for visual purposes.
            Node start = new Node(new Vector2(3, 5), true);
            // for when X < Y.
            Node target1 = new Node(new Vector2(1, 4), true);
            // for when X > Y.
            //Node target2 = new Node(new Vector2(1, 4), true);

            // First we calculate start.X minus target1.X. 3 - 1 = 2. (dstX)
            // Then we calcculate start.Y minus target1.Y. 5 - 4 = 1. (dstY)
            // In this case, X > Y.
            // 14 * 3 + 10 * (2 - 3).

            int expected1 = 14 * 3 + 10 * (2 - 1);
            //int expected1 = 52;

            int actual1 = pathFinder.CalculateDistance(start, target1);

            Assert.AreEqual(expected1, actual1);
        }

        [TestMethod]
        public void TestRetracePath()
        {
            PathFinder pathFinder = new PathFinder();

            Stack<Node> expected = new Stack<Node>();

            Node node1 = new Node(new Vector2(1, 3), true);
            Node node2 = new Node(new Vector2(2, 3), true);
            Node node3 = new Node(new Vector2(3, 3), true);

            expected.Push(node1);
            expected.Push(node2);
            expected.Push(node3);
            expected.Reverse();

            //Stack<Node> actual = new Stack<Node>();

            var actual = pathFinder.RetracePath(node1, node2);

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
