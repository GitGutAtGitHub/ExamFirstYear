﻿using ExamProjectFirstYear.ObjectPools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.Components.PlayerComponents
{
    class RangedAttack : Component
    {
        #region FIELDS

        // Used to make sure the player can only shoot once every X-amount of seconds.
        private float rangedAttackTimer;
        // Used in RangedAttack method to set the break time between ranged attacks.
        private float breakForRangedAttack = 0.7f;

        #endregion


        public RangedAttack()
        {

        }


        #region METHODS
        public void RangedAttackMethod(Component component, Vector2 velocity)
        {
			Tag tmpTag = Tag.PLAYERMELEEATTACK;

			switch (component.ToEnum())
			{
				case Tag.PLAYER:
					tmpTag = Tag.PLAYERPROJECTILE;
					break;
				case Tag.RANGEDENEMY:
					tmpTag = Tag.ENEMYPROJECTILE;
					break;
			}

			// rangedAttackTimer makes sure the ranged attack can only be used once every x-amount of seconds.
			// Mana needs to be higher than 0 or the player can't fire a ranged attack.
			//if ((/*CanShoot == true &&*/ rangedAttackTimer >= breakForRangedAttack && tmpTag == Tag.ENEMYPROJECTILE) || (CanShoot == true && rangedAttackTimer >= breakForRangedAttack && ((Player)component).Mana > 0))
			if (rangedAttackTimer >= breakForRangedAttack)
            {
                
                GameObject tmpProjectileObject = ProjectilePool.Instance.GetObject(tmpTag);
                tmpProjectileObject.Tag = tmpTag;

                tmpProjectileObject.Awake();
                tmpProjectileObject.Start();

                Collider tmpProjectileCollider = (Collider)tmpProjectileObject.GetComponent(Tag.COLLIDER);

                tmpProjectileObject.Transform.Position = component.GameObject.Transform.Position;

                Movement tmpMovement = (Movement)tmpProjectileObject.GetComponent(Tag.MOVEMENT);

                tmpMovement.Velocity = velocity;

                if (velocity.X < 0)
                {
                    ((SpriteRenderer)tmpProjectileObject.GetComponent(Tag.SPRITERENDERER)).spriteEffect = SpriteEffects.None;
                }
                if (velocity.X > 0)
                {
                    ((SpriteRenderer)tmpProjectileObject.GetComponent(Tag.SPRITERENDERER)).spriteEffect = SpriteEffects.FlipHorizontally;
                }
                //tmpMovement.Speed = 1000f;
                GameWorld.Instance.Colliders.Add(tmpProjectileCollider);
                GameWorld.Instance.GameObjects.Add(tmpProjectileObject);

                // Resets the timer so the attack can't be used again, until the timer reaches x-amount of seconds again.
                rangedAttackTimer = 0;
            }
        }

        public override void Update(GameTime gameTime)
        {
            // Makes sure the timer keeps running throughout the game.
            rangedAttackTimer += GameWorld.Instance.DeltaTime;
        }

        public override Tag ToEnum()
        {
            return Tag.RANGEDATTACK;
        }

        #endregion
    }
}
