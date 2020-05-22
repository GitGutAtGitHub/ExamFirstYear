using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.PathFinding
{
    class Node
    {
        #region FIELDS
        private Node parent;

        private int gCost = 0;
        private int hCost = 0;

        private Vector2 position;

        private bool walkable;
        #endregion

        #region PROPERTIES
        public Node Parent { get => parent; set => parent = value; }
        public int GCost { get => gCost; set => gCost = value; }
        public int HCost { get => hCost; set => hCost = value; }
        public int FCost { get => hCost + gCost; }
        public bool Walkable { get => walkable; set => walkable = value; }
        public Vector2 Position { get => position; set => position = value; }
        #endregion

        public Node(Vector2 position, bool walkable)
        {
            this.Position = position;
            this.Walkable = walkable;
        }

        public Vector2 GetCoordinate()
        {
            return position / NodeManager.Instance.CellSize;
        }

    }
}
