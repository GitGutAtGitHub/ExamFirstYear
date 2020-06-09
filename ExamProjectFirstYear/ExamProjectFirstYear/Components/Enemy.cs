using ExamProjectFirstYear.Factories;
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
    /// This class is public to make Unit Testing possible.
    /// </summary>
    public abstract class Enemy : Component, IEntity, IGameListener
    {
        #region FIELDS

        protected float speed;
        protected int health;
        private int sightRadius;

        //it is a public variable, to be able to edit the specific X and Y values, it has to be a variable.
        public Vector2 Velocity;
        private Vector2 prevNode = new Vector2(0, 0);

        private Stack<Node> path = new Stack<Node>();

        private IState currentState;
        private GameObject target;
        protected int enemyID;

        #endregion


        #region PROPERTIES

        public Vector2 TargetPosition { get; set; }
        public int SightRadius { get => sightRadius; set => sightRadius = value; }
        public GameObject Target { get => target; set => target = value; }
        public Stack<Node> Path { get => path; set => path = value; }
        public Vector2 PrevTargetNode { get => prevNode; set => prevNode = value; }
        public IState CurrentState { get => currentState; set => currentState = value; }


        #endregion


        #region METHODS

        public abstract void SwitchState(IState newState);

        public abstract void AddTarget();

        protected abstract void Move();

        protected abstract void EnemyDeath();

        /// <summary>
        /// Used by all enemies that drop materials when they die.
        /// This methods instantiates whatever material the enemy needs to drop through the MaterialFactory.
        /// </summary>
        /// <param name="materialID"></param>
        public void DropMaterialUponDeath(Tag materialType)
        {
            GameObject droppedMaterial = MaterialFactory.Instance.Create(materialType);
			droppedMaterial.Transform.Position = new Vector2(GameObject.Transform.Position.X, GameObject.Transform.Position.Y);

			GameWorld.Instance.Colliders.Add((Collider)droppedMaterial.GetComponent(Tag.COLLIDER));
            GameWorld.Instance.GameObjects.Add(droppedMaterial);
        }

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

        public virtual void FollowPath(bool XAndY)
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

                    if (XAndY == true)
                    {
                        Velocity = new Vector2(vectorX, vectorY);
                    } else
                    {
                        Velocity = new Vector2(vectorX, 0);

                    }

                  

                    //checking the distance between the enemy and the targetposition.
                    dstX = Math.Abs(GameObject.Transform.Position.X - TargetPosition.X);
                    dstY = Math.Abs(GameObject.Transform.Position.Y - TargetPosition.Y);

                    //it has reached the end of the node, and is ready to get instructions for the next node.
                    if (dstX < 8 && dstY < 8 || dstX < 16 && XAndY == false)
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

        public abstract void Notify(GameEvent gameEvent, Component component);

        #endregion
    }
}
