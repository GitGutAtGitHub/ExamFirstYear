using ExamProjectFirstYear.ObjectPools;
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
        private float breakForRangedAttack;

        public bool HasShot { get; private set; }

        #endregion

        public RangedAttack(float breakForRangedAttack)
        {
            this.breakForRangedAttack = breakForRangedAttack;
        }

        #region METHODS
        public void RangedAttackMethod(GameObject sender, Tag projectileType, Vector2 velocity)
		{
            // rangedAttackTimer makes sure the ranged attack can only be used once every x-amount of seconds.
            // Mana needs to be higher than 0 or the player can't fire a ranged attack.
            //if ((/*CanShoot == true &&*/ rangedAttackTimer >= breakForRangedAttack && tmpTag == Tag.ENEMYPROJECTILE) || (CanShoot == true && rangedAttackTimer >= breakForRangedAttack && ((Player)component).Mana > 0))
            if (rangedAttackTimer >= breakForRangedAttack)
            {
                GameObject tmpProjectileObject =ProjectileFactory.Instance.Create(projectileType);

                Collider tmpProjectileCollider = (Collider)tmpProjectileObject.GetComponent(Tag.COLLIDER);

                tmpProjectileObject.Transform.Position = sender.Transform.Position;

                Movement tmpMovement = (Movement)tmpProjectileObject.GetComponent(Tag.MOVEMENT);

                tmpMovement.Velocity = velocity;

                if (velocity.X < 0)
                {
                    ((SpriteRenderer)tmpProjectileObject.GetComponent(Tag.SPRITERENDERER)).SpriteEffect = SpriteEffects.None;
                }
                if (velocity.X > 0)
                {
                    ((SpriteRenderer)tmpProjectileObject.GetComponent(Tag.SPRITERENDERER)).SpriteEffect = SpriteEffects.FlipHorizontally;
                }
                //tmpMovement.Speed = 1000f;
                GameWorld.Instance.Colliders.Add(tmpProjectileCollider);
                GameWorld.Instance.GameObjects.Add(tmpProjectileObject);

                // Resets the timer so the attack can't be used again, until the timer reaches x-amount of seconds again.
                rangedAttackTimer = 0;
                ((SoundComponent)sender.GetComponent(Tag.SOUNDCOMPONENT)).StartPlayingSoundInstance("RangedAttack3");


                HasShot = true;
            }
			else
			{
                HasShot = false;
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
