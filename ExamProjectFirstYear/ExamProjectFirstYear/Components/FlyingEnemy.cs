using ExamProjectFirstYear.PathFinding;
using ExamProjectFirstYear.StatePattern;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.Components
{
    class FlyingEnemy : Enemy
    {
        private PathFinder enemyPathFinder;

        private Stack<Node> flyingPath;
        private Vector2 prevNode = new Vector2(0, 0);

        public Stack<Node> FlyingPath { get => flyingPath; set => flyingPath = value; }
        public PathFinder EnemyPathFinder { get => enemyPathFinder; set => EnemyPathFinder = value; }
        public Vector2 PrevTargetNode { get => prevNode; set => prevNode = value; }
        public bool CanFollowPlayer { get; set; }


        protected override void ThreadUpdate()
        {
            while (Alive == true)
            {
                //to make sure that it only runs 60 times a second
                if (GameWorld.Instance.TimeElapsed % 1.16f == 0)
                {
                    currentState.Execute();
                    Move();
                }
            }
        }

        public override void Awake()
        {
            enemyPathFinder = new PathFinder();
            SightRadius = 30 * NodeManager.Instance.CellSize;
            Alive = true;
            speed = 200f;
            GameObject.Tag = Tag.FLYINGENEMY;
            SwitchState(new EnemyAttackState());

            Thread flyingEnemyThread = new Thread(ThreadUpdate);

            flyingEnemyThread.Start();
        }


        public override void Start()
        {
            GameObject.SpriteName = "smol";
        }

        /// <summary>
        /// SKAL VÆRE PUBLIC FOR AT KUNNE VIRKE TIL STATES
        /// Used to switch between states and enter the "Enter" method in whichever state the enemy is in.
        /// </summary>
        /// <param name="newState"></param>
        public override void SwitchState(IState newState)
        {
            // Makes sure the state isn't null when exiting a state.
            // This is done to avoid an exception.
            if (currentState != null)
            {
                currentState.Exit();
            }

            currentState = newState;
            // "This" means the FlyingEnemy.
            currentState.Enter(this);
        }



        public override void Update(GameTime gameTime)
        {
            currentState.Execute();
            Move();
        }


        protected override void Notify()
        {
            throw new NotImplementedException();
        }



        protected override void Move()
        {
            if (Velocity != Vector2.Zero)
            {
                Velocity.Normalize();
            }

            Velocity *= speed;

            GameObject.Transform.Translate(Velocity * GameWorld.Instance.DeltaTime);
        }

        public override Tag ToEnum()
        {
            return Tag.FLYINGENEMY;
        }
    }
}
