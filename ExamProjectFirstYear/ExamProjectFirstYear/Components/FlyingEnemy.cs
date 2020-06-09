using ExamProjectFirstYear.PathFinding;
using ExamProjectFirstYear.StatePattern;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.Components
{
    /// <summary>
    /// Flying Enemy component class.
    /// </summary>
    class FlyingEnemy : Enemy
    {
        #region Fields


        #endregion


        #region PROPERTIES


        #endregion


        #region Override methods

        public override void Awake()
        {
            SightRadius = 6 * NodeManager.Instance.CellSize;
            health = 2;
            speed = 200f;
            GameObject.Tag = Tag.MEELEEENEMY;
            SwitchState(new EnemyIdleState());
            ((SoundComponent)GameObject.GetComponent(Tag.SOUNDCOMPONENT)).AddSound("MothDeath", false);
            ((SoundComponent)GameObject.GetComponent(Tag.SOUNDCOMPONENT)).AddSound("MothMove", true);
            enemyID = 3;
        }

        public override void Start()
        {
            GameObject.SpriteName = "FlyingEnemy";
        }

        public override void SwitchState(IState newState)
        {
            // Makes sure the state isn't null when exiting a state.
            // This is done to avoid an exception.
            if (CurrentState != null)
            {
                CurrentState.Exit();
            }

            CurrentState = newState;
            // "This" means the FlyingEnemy.
            CurrentState.Enter(this);
        }

        public override void Update(GameTime gameTime)
        {
            CurrentState.Execute();
            Move();
            EnemyDeath();
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

        /// <summary>
        /// Adds the player as enemy's target.
        /// </summary>
        public override void AddTarget()
        {
            Target = GameWorld.Instance.Player.GameObject;
        }

        /// <summary>
        /// If the enemies health reaches 0, the enemy is removed from the game.
        /// As it dies, it drops a(n) material/item using the DropMaterialUponDeath method.
        /// </summary>
        protected override void EnemyDeath()
        {
            if (health <= 0)
            {
                ((SoundComponent)GameObject.GetComponent(Tag.SOUNDCOMPONENT)).StartPlayingSoundEffect("MothDeath");

                GameObject.Destroy();

                DropMaterialUponDeath(Tag.MOTHWING);

                GameWorld.Instance.SQLiteHandler.AddRecordedCreature(enemyID, GameWorld.Instance.Journal.JournalID);
            }
        }

        public override Tag ToEnum()
        {
            return Tag.FLYINGENEMY;
        }

        public override void Notify(GameEvent gameEvent, Component component)
        {
            // If the enemy is hit by players projectile from the ranged attack or the melee attack,
            // the projectile is removed from the game and enemy looses 1 hp.
            if (gameEvent.Title == "Colliding" && component.GameObject.Tag == Tag.PLAYERPROJECTILE ||
                gameEvent.Title == "Colliding" && component.GameObject.Tag == Tag.PLAYERMELEEATTACK)
            {
                component.GameObject.Destroy();
                health--;
            }
        }

        #endregion
    }
}
