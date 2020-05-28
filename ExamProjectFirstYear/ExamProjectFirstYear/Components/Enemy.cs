﻿using ExamProjectFirstYear.StatePattern;
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

        protected float speed;
        protected int health;
        private int sightRadius;
        private Vector2 targetPosition;
        //it is a public variable, to be able to edit the specific X and Y values, it has to be a variable.
        public Vector2 Velocity;
        private bool alive = true;
        protected IState currentState;
        private GameObject target = GameWorld.Instance.player.GameObject;
       

        #endregion


        #region PROPERTIES

        public Vector2 TargetPosition { get; set; }
        public int SightRadius { get => sightRadius; set => sightRadius = value; }
        public GameObject Target { get => target; set => target = value; }
        public bool Alive { get => alive; set => alive = value; }

        #endregion


        #region METHODS

        public abstract void SwitchState(IState newState);

        protected abstract void Notify();

        protected abstract void Move();

        public override Tag ToEnum()
        {
            return Tag.ENEMY;
        }

        #endregion
    }
}
