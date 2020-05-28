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

        public bool findPathFromEnemy;
     
        private Stack<Node> flyingPath;
        private Vector2 prevNode = new Vector2(0, 0);

        #region PROPERTIES

        public Stack<Node> FlyingPath { get => flyingPath; set => flyingPath = value; }
        public PathFinder EnemyPathFinder { get => enemyPathFinder; set => EnemyPathFinder = value; }
        public Vector2 PrevTargetNode { get => prevNode; set => prevNode = value; }

        #endregion


        public override void Awake()
        {
            enemyPathFinder = new PathFinder(this);
            SightRadius = 6 * NodeManager.Instance.CellSize;
            Alive = true;
            speed = 200f;
            GameObject.Tag = Tag.FLYINGENEMY;
            SwitchState(new EnemyAttackState());
        }


        public override void Start()
        {
            GameObject.SpriteName = "smol";
        }


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
