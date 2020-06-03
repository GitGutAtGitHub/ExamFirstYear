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
    /// <summary>
    /// Abstract Enemy class for different enemy types.
    /// </summary>
    abstract class Enemy : Component, IEntity
    {
        #region FIELDS

        protected float speed;
        protected int health;
        private int sightRadius;
        //it is a public variable, to be able to edit the specific X and Y values, it has to be a variable.
        public Vector2 Velocity;
        
        protected IState currentState;
        private GameObject target; 
       

        #endregion


        #region PROPERTIES

        public Vector2 TargetPosition { get; set; }
        public int SightRadius { get => sightRadius; set => sightRadius = value; }
        public GameObject Target { get => target; set => target = value; }
  

        #endregion


        #region METHODS

        public abstract void SwitchState(IState newState);

        public abstract void AddTarget();

        protected abstract void Move();

        protected abstract void EnemyDeath();

        /// <summary>
        /// Used by all enemies that drop materials when they die.
        /// This methods instantiates whatever material the enemy needs to drop.
        /// </summary>
        /// <param name="materialID"></param>
        public void DropMaterialUponDeath(byte materialID)
        {
            GameObject droppedMaterial = new GameObject();
            SpriteRenderer spriteRenderer = new SpriteRenderer();
            Movement movementEnemy = new Movement(true, 0, 0);

            droppedMaterial.AddComponent(new Material(materialID));

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
            return Tag.ENEMY;
        }

        #endregion
    }
}
