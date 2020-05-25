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
    abstract class Enemy : Component, IEntity
    {
        #region FIELDS

        protected int speed;
        protected int health;
        private int sightRadius;
     

        //it is a public variable, to be able to edit the specific X and Y values, it has to be a variable.
        public Vector2 Velocity;
        //LOOK AT THIS LATER!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        protected bool alive = true;
     
        protected IState currentState;
        protected Thread EnemyThread;
        private GameObject target = GameWorld.Instance.player.GameObject;

        #endregion


        #region PROPERTIES

        public int SightRadius { get => sightRadius; set => sightRadius = value; }
        //public Vector2 Velocity { get; set; }
        public GameObject Target { get => target; set => target = value; }

        #endregion


        #region METHODS

        protected abstract void ThreadUpdate();

        protected abstract void SwitchState(IState newState);

        protected abstract void Notify();

        protected abstract void Move();

        public override Tag ToEnum()
        {
            return Tag.ENEMY;
        }

        #endregion
    }
}
