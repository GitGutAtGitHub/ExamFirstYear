﻿using Microsoft.Xna.Framework;
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
        private float breakForRangedAttack = 0.3f;

        #endregion


        #region PROPERTIES

        public bool CanShoot { get; set; } = true;

        #endregion


        public RangedAttack()
        {

        }


        #region METHODS

        /// <summary>
        /// Ranged attack for Player.
        /// </summary>
        public void PlayerRangedAttack(Player player)
        {
            // CANSHOOT SKAL MÅSKE SLETTES. TROR IKKE DEN ER NØDVENDIG MERE !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            // rangedAttackTimer makes sure the ranged attack can only be used once every x-amount of seconds.
            // Mana needs to be higher than 0 or the player can't fire a ranged attack.
            if (CanShoot == true && rangedAttackTimer >= breakForRangedAttack && player.Mana > 0)
            {
                GameObject tmpProjectileObject = PlayerProjectilePool.Instance.GetObject();
                Collider tmpProjectileCollider = (Collider)tmpProjectileObject.GetComponent(Tag.COLLIDER);

                tmpProjectileObject.Transform.Position = player.GameObject.Transform.Position;

                Movement tmpMovement = (Movement)tmpProjectileObject.GetComponent(Tag.MOVEMENT);

                tmpMovement.Velocity = player.Direction;

                if (player.Direction.X < 0)
                {
                    ((SpriteRenderer)tmpProjectileObject.GetComponent(Tag.SPRITERENDERER)).spriteEffect = SpriteEffects.None;
                }
                if (player.Direction.X > 0)
                {
                    ((SpriteRenderer)tmpProjectileObject.GetComponent(Tag.SPRITERENDERER)).spriteEffect = SpriteEffects.FlipHorizontally;
                }
                //tmpMovement.Speed = 1000f;
                GameWorld.Instance.Colliders.Add(tmpProjectileCollider);
                GameWorld.Instance.GameObjects.Add(tmpProjectileObject);

                player.Mana--;

                CanShoot = false;
                // Makes sure the timer in the Player-class is resat. This is for mana regeneration purposes.
                player.CanRegenerateMana = false;
                // Resets the timer so the attack can't be used again, until the timer reaches x-amount of seconds again.
                rangedAttackTimer = 0;
            }
        }

        public override void Update(GameTime gameTime)
        {
            // Makes sure the timer keeps running throughout the game.
            rangedAttackTimer += GameWorld.Instance.DeltaTime;
        }

        /// <summary>
        /// Methods used when the ranged attack button is released. Sets CanShoot to true.
        /// </summary>
        public void PlayerReleaseRangedAttack()
        {
            CanShoot = true;
        }

        public override Tag ToEnum()
        {
            return Tag.RANGEDATTACK;
        }

        #endregion
    }
}
