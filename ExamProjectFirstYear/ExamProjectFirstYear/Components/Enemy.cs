using ExamProjectFirstYear.PathFinding;
using ExamProjectFirstYear.StatePattern;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.Components
{
    /// <summary>
    /// Abstract Enemy class for different enemy types.
    /// </summary>
    abstract class Enemy : Component, IEntity
    {
        #region FIELDS

        protected float speed;
        protected int health;
        private int sightRadius;
        private Vector2 targetPosition;
        //it is a public variable, to be able to edit the specific X and Y values, it has to be a variable.
        public Vector2 Velocity;
        private bool hasPath = false;

        private Vector2 prevNode = new Vector2(0, 0);

        private Stack<Node> path = new Stack<Node>();

        protected IState currentState;
        private GameObject target; 
       

        #endregion


        #region PROPERTIES

        public Vector2 TargetPosition { get; set; }
        public int SightRadius { get => sightRadius; set => sightRadius = value; }
        public GameObject Target { get => target; set => target = value; }
        public bool HasPath { get => hasPath; set => hasPath = value; }

        public Stack<Node> Path { get => path; set => path = value; }

        public Vector2 PrevTargetNode { get => prevNode; set => prevNode = value; }


        #endregion


        #region METHODS

        public abstract void SwitchState(IState newState);

        protected abstract void Notify();

        public abstract void AddTarget();

        protected abstract void Move();

        public override Tag ToEnum()
        {
            return Tag.ENEMY;
        }

        public virtual void GeneratePath()
        {
            // Find new path if target (players) position has changed since it last generated a path.
            if (PrevTargetNode !=
                new Vector2((int)(Target.Transform.Position.X / NodeManager.Instance.CellSize),
                            (int)(Target.Transform.Position.Y / NodeManager.Instance.CellSize)))
            {

                if (!PathFinder.Instance.EnemiesNeedingPath.Contains(this))
                {
                    // Adds the flying enemy to the list of enemies that need to find a path.
                    PathFinder.Instance.EnemiesNeedingPath.Enqueue(this);
                }

            }
        }

        public virtual void FollowPath()
        {
            float dstX;
            float dstY;

            if (Path != null)
            {
                // To avoid null exception
                if (Path.Count > 0)
                {
                    TargetPosition = new Vector2((Path.Peek().Position.X + NodeManager.Instance.CellSize / 2),
                                                                        (Path.Peek().Position.Y + NodeManager.Instance.CellSize / 2));

                    //calculating the direction-vector between the enemy and its target position
                    float vectorX = (TargetPosition.X) - (GameObject.Transform.Position.X);
                    float vectorY = (TargetPosition.Y) - (GameObject.Transform.Position.Y);

                    Velocity = new Vector2(vectorX, vectorY);

                    //checking the distance between the enemy and the targetposition.
                    dstX = Math.Abs(GameObject.Transform.Position.X - TargetPosition.X);
                    dstY = Math.Abs(GameObject.Transform.Position.Y - TargetPosition.Y);

                    //it has reached the end of the node, and is ready to get instructions for the next node.
                    if (dstX < 8 && dstY < 8)
                    {
                        //if there is still path nodes, then pop the current
                        if (Path.Count > 0)
                        {
                            Path.Pop();
                            Velocity = new Vector2(0, 0);
                        }
                    }
                }
            }
        }


        #endregion
    }
}
