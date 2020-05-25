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
        private Vector2 targetPosition;
        private Stack<Node> flyingPath;

        public Stack<Node> FlyingPath { get => flyingPath; set => flyingPath = value; }
        public PathFinder EnemyPathFinder { get => enemyPathFinder; set => EnemyPathFinder = value; }
        public Vector2 TargetPosition { get; set; }

       protected override void ThreadUpdate()
        {
            while (alive == true)
            {
                SwitchState(new EnemyAttackState());
                currentState.Execute();
                Move();

            }
        }

        public override void Awake()
        {
            
            enemyPathFinder = new PathFinder();
            SightRadius = 30 * NodeManager.Instance.CellSize;
            alive = true;
            speed = 20;
            GameObject.Tag = Tag.FLYINGENEMY;
            Thread flyingEnemyThread = new Thread(ThreadUpdate);
            flyingEnemyThread.Start();
        }

        public override void Start()
        {
            GameObject.SpriteName = "FlyingEnemy";
        }

        protected override void SwitchState(IState newState)
        {
            // Makes sure the state isn't null when exiting a state.
            // This is done to avoid an exception.
            if (currentState != null)
            {
                currentState.Exit();
            }

            currentState = newState;
            // This means the FlyingEnemy.
            currentState.Enter(this);
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
