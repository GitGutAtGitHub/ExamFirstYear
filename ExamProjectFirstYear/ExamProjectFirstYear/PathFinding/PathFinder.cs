using ExamProjectFirstYear.Components;
using ExamProjectFirstYear.PathFinding;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExamProjectFirstYear
{
    class PathFinder
    {
        Enemy enemy;
        public bool FindPathBool;

        public PathFinder(Enemy enemy)
        {
            this.enemy = enemy;
            Thread pathFinderThread = new Thread(ThreadUpdate);
            pathFinderThread.Name = "EnemyPathFinder";
            pathFinderThread.Start();
        }

        /// <summary>
        /// This is the method run by the PathFinder thread.
        /// It only rungs the FindPath method when FindPathFromEnemy is true,
        /// to avoid having the method run over and over again for no reason.
        /// If the if-statement isn't true, it sleeps for 500 miliseconds
        /// before checking the if-statement again.
        /// </summary>
        public void ThreadUpdate()
        {
            while ((enemy as FlyingEnemy).Alive == true)
            {
                if ((enemy as FlyingEnemy).findPathFromEnemy == true)
                {
                    (enemy as FlyingEnemy).FlyingPath = FindPath(enemy.GameObject.Transform.Position, enemy.Target.Transform.Position);
                }
                else
                {
                    Thread.Sleep(500);
                }
            }

        }
                                 ///////IMPORTANT\\\\\\\\
        
        // All of the code from here on in this class is taken from a former project made by the same group.
        // There are a few changes made for this program to work as intended.

        #region Astar Pathfinding
        /// <summary>
        /// Calculates the distance between two nodes
        /// </summary>
        /// <param name="inputNode"></param>
        /// <param name="targetNode"></param>
        /// <returns></returns>
        private int CalculateDistance(Node inputNode, Node targetNode)
        {
            //distance between the two nodes, x and y
            int dstX = Math.Abs((int)inputNode.GetCoordinate().X - (int)targetNode.GetCoordinate().X);
            int dstY = Math.Abs((int)inputNode.GetCoordinate().Y - (int)targetNode.GetCoordinate().Y);

            //if the x posittion is bigger than the y value

            if (dstX > dstY)
            {
                //formula: y + (x-y)
                //formula with values: 14y + 10*(x-y);

                //Y-afstanden er hvor langt den går op, men diagonalt.
                //the Y distance is how far it goes up or down, but diagonally
                //Then it findes the x-distance by subracting the lowest number (this time y) from the biggest number(x) to calculate how long the remaining x-distance is.

                return 14 * dstY + 10 * (dstX - dstY);
            }
            else
            {
                //The opposite if y is bigger.
                return 14 * dstX + 10 * (dstY - dstX);
            }
        }

        /// <summary>
        /// Returning a list of the surrounding neighbours
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();

            //searching in a 3x3 grid around the node
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    //it skips the input node because it cant be a neighbour to itself
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }

                    // GetCoordinate is used so every tile has coordinates such as 1.2 or 3.5 etc, instead of 120.241 or 312.589.
                    // Saves the position it's at, so it knows where to search from.
                    // This is done to make it more readable.
                    int checkX = (int)node.GetCoordinate().X + x;
                    int checkY = (int)node.GetCoordinate().Y + y;

                    // Checks if it's inside the grid that makes out the playable field.
                    if (checkX >= 0 && checkX < NodeManager.Instance.CellRowCount && checkY >= 0 && checkY < NodeManager.Instance.CellRowCount)
                    {
                        //Adds Neighour to the nodeArray
                        neighbours.Add(NodeManager.Instance.Nodes[checkX, checkY]);
                    }
                }
            }
            return neighbours;
        }

        /// <summary>
        /// returns a stack with a Path retraced from the parents. 
        /// </summary>
        /// <param name="startNode"></param>
        /// <param name="endNode"></param>
        /// <returns></returns>
        private Stack<Node> RetracePath(Node startNode, Node endNode)
        {
            // Makes a new list with the found path.
            Stack<Node> path = new Stack<Node>();

            // Starts at the end node.
            Node currentNode = endNode;

            // Runs as longs as it hasn't reached the start.
            while (currentNode != startNode)
            {
                // Adds the current node to the path list.
                path.Push(currentNode);

                // Sets the current node to be the current node's parrent node. Meaning the next one in the path.
                currentNode = currentNode.Parent;
            }
            // We reverse the created path because otherwise it starts from the targetnode and walks to the start node.
            // This way, it starts from the startNode and ends on the targetNode.
            path.Reverse();
            return path;
        }

        /// <summary>
        /// Method for finding a path from one point to another.
        /// </summary>
        /// <param name="startNode"></param>
        /// <param name="targetNode"></param>
        /// <returns></returns>
        public Stack<Node> FindPath(Vector2 startPosition, Vector2 targetPosition)
        {
            Node startNode = NodeManager.Instance.Nodes[(int)(startPosition.X / NodeManager.Instance.CellSize), (int)(startPosition.Y / NodeManager.Instance.CellSize)];
            Node targetNode = NodeManager.Instance.Nodes[(int)(targetPosition.X / NodeManager.Instance.CellSize), (int)(targetPosition.Y / NodeManager.Instance.CellSize)];

            Stack<Node> pathStack = new Stack<Node>();

            //List with all cells, with calculated fCost.
            List<Node> openList = new List<Node>();

            // Already evaluated cells.
            List<Node> closedList = new List<Node>();

            //Adds the first cell to the list.
            openList.Add(startNode);

            while (openList.Count > 0)
            {
                // Starts at start node. Current node is the node with the lowest fCost.
                Node currentNode = openList[0];

                // Goes through the list.
                for (int i = 0; i < openList.Count; i++)
                {
                    // Checks if another node has a smaller fCost than the current. If they are the same, their hCost are comapred.
                    if (openList[i].FCost < currentNode.FCost || (openList[i].FCost == currentNode.FCost && openList[i].HCost < currentNode.HCost))
                    {
                        currentNode = openList[i];
                    }
                }

                //Done checking the cell and it is removed from the open list and added to the closed list.
                openList.Remove(currentNode);
                closedList.Add(currentNode);

                // If it has found the right cell, it needs to trace the way back to the start cell.
                if (currentNode == targetNode)
                {
                    pathStack = RetracePath(startNode, targetNode);
                }

                foreach (Node neighbour in GetNeighbours(currentNode))
                {
                    //runs through the neighbour list
                    //if that node is not walkable, like and obstruction, or has already been reviewed and is on the closed list
                    //skip that node

                    if (neighbour.Walkable != true || closedList.Contains(neighbour))
                    {
                        continue;
                    }

                    // The G-cost is recalculated here (or if it is the first run through 'calculated')
                    int recalculatedGCostNeighbor = currentNode.GCost + CalculateDistance(currentNode, neighbour);

                    // If the recalculatedGcost for the neighboring node is lower than the neighbors currently listed G-cost
                    // or if the neighbor is not in the OpenList
                    if (recalculatedGCostNeighbor < neighbour.GCost || !openList.Contains(neighbour))
                    {
                        // Neighbours gCost = the distance cost from startNode to neighbor.
                        neighbour.GCost = recalculatedGCostNeighbor;

                        // Neighbours hCost = the distance cost from the neighbour to targetNode.
                        neighbour.HCost = CalculateDistance(neighbour, targetNode);

                        // Sets the neighbours parrent to the current node it came from, so it will always know where it came from when it needs to.
                        neighbour.Parent = currentNode;

                        // If the neighbour isn't in the open list, it's added.
                        if (!openList.Contains(neighbour))
                        {
                            openList.Add(neighbour);
                        }
                    }
                }
            }


            // Saves the previous target node (players position).
            // Used to prevent astar calculating for the same targetposition over and over again.
            (enemy as FlyingEnemy).PrevTargetNode = new Vector2(
                 (int)enemy.Target.Transform.Position.X / NodeManager.Instance.CellSize,
                 (int)enemy.Target.Transform.Position.Y / NodeManager.Instance.CellSize);

            // Makes sure the method doesn't run again right away.
            (enemy as FlyingEnemy).findPathFromEnemy = false;

            return pathStack;
        }
        #endregion
    }
}
