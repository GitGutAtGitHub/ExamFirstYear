﻿using ExamProjectFirstYear.PathFinding;
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

        private Stack<Node> flyingPath;

        #endregion


        #region PROPERTIES

        public Stack<Node> FlyingPath { get => flyingPath; set => flyingPath = value; }



        #endregion


        #region Override methods

        public override void Awake()
        {
            SightRadius = 6 * NodeManager.Instance.CellSize;
            health = 5;
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


            // FOR DEBUGGING. DELETE LATER !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.L))
            {
                LooseHp();
            }
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

        /// <summary>
        /// Adds the player as enemy's target.
        /// </summary>
        public override void AddTarget()
        {
            Target = GameWorld.Instance.player.GameObject;
        }

        /// <summary>
        /// If the enemies health reaches 0, tthe enemy is removed from the game.
        /// As it dies, it drops a(n) material/item using the DropMaterialUponDeath method.
        /// </summary>
        protected override void EnemyDeath()
        {
            if (health <= 0)
            {
                GameObject.Destroy();
                DropMaterialUponDeath();
            }
        }

        // FOR DEBUGGING. DELETE LATER !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        public void LooseHp()
        {
            health--;
            Console.WriteLine(health);
        }

        /// <summary>
        /// Instantiates a new material.
        /// </summary>
        protected override void DropMaterialUponDeath()
        {
            //LevelManager.Instance.CreateObject(Tag.MATERIAL, (int)GameObject.Transform.Position.X, (int)GameObject.Transform.Position.Y,
            //                                                 (int)GameObject.Transform.Position.X, (int)GameObject.Transform.Position.Y);

            GameObject droppedMaterial = new GameObject();
            SpriteRenderer spriteRenderer = new SpriteRenderer();
            Movement movementEnemy = new Movement(true, 0, 0);

            droppedMaterial.AddComponent(new Material(1));
            droppedMaterial.AddComponent(movementEnemy);
            droppedMaterial.AddComponent(spriteRenderer);

            droppedMaterial.Awake();
            droppedMaterial.Start();

            droppedMaterial.Transform.Position = new Vector2(GameObject.Transform.Position.X, GameObject.Transform.Position.Y);

            Collider collider = new Collider(spriteRenderer, (Material)droppedMaterial.GetComponent(Tag.MATERIAL)) { CheckCollisionEvents = true };

            droppedMaterial.AddComponent(collider);

            GameWorld.Instance.Colliders.Add(collider);
            GameWorld.Instance.GameObjects.Add(droppedMaterial);
        }

        public override Tag ToEnum()
        {
            return Tag.FLYINGENEMY;
        }


        #endregion
    }
}
