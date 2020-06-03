using ExamProjectFirstYear.PathFinding;
using ExamProjectFirstYear.StatePattern;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.Components.GameObjects.Enemy
{
    /// <summary>
    /// Flying Enemy component class.
    /// </summary>
    class FlyingEnemy : Enemy
    {
        #region Fields

        private Stack<Node> flyingPath;
      
        #endregion


        #region PROPERTIES

        public Stack<Node> FlyingPath { get => flyingPath; set => flyingPath = value; }
       
    

        #endregion


        #region Override methods

        public override void Awake()
        {            
            SightRadius = 6 * NodeManager.Instance.CellSize;         
            speed = 200f;
            GameObject.Tag = Tag.MEELEEENEMY;
            SwitchState(new EnemyIdleState());
        }

        public override void Start()
        {
            GameObject.SpriteName = "FlyingEnemy";
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

      

        public override void AddTarget()
        {
            Target = GameWorld.Instance.player.GameObject;
        }

        public override Tag ToEnum()
        {
            return Tag.FLYINGENEMY;
        }

        #endregion
    }
}
