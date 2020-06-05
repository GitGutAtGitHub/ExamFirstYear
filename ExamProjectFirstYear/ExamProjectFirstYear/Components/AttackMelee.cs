using ExamProjectFirstYear.ObjectPools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.Components
{
    class AttackMelee : Component
    {
        #region FIELDS

        private float attackRange = 1.2f;
        private float attackRangeLeft = -1.2f;
        private Vector2 attackDirection;

        #endregion


        public bool canAttack { get; set; } = true;


        #region METHODS

        public void ReleaseMeleeMeleeAttack()
        {
            canAttack = true;
        }

        /// <summary>
        /// Melee attack for Player.
        /// </summary>
        public void PlayerMeleeAttack(Player player)
        {
            if (canAttack)
            {
                GameObject tmpMeleeObject = PlayerMeleeAttackPool.Instance.GetObject();
                SpriteRenderer tmpMeleeRenderer = (SpriteRenderer)tmpMeleeObject.GetComponent(Tag.SPRITERENDERER);
                Collider tmpMeleeCollider = (Collider)tmpMeleeObject.GetComponent(Tag.COLLIDER);

                //Makes sure the attack appears on the right side of the player.
                if(player.Direction.X == 1)
                {
                    attackDirection = new Vector2(player.GameObject.Transform.Position.X +
                                                                (((SpriteRenderer)player.GameObject.GetComponent(Tag.SPRITERENDERER)).Sprite.Width / attackRange),
                                                                player.GameObject.Transform.Position.Y -
                                                                (((SpriteRenderer)player.GameObject.GetComponent(Tag.SPRITERENDERER)).Sprite.Height / 2));

                    tmpMeleeRenderer.spriteEffect = SpriteEffects.None;
                }
                // Makes sure the attack appears on the left side of the player.
                if (player.Direction.X == -1)
                {
                    attackDirection = new Vector2(player.GameObject.Transform.Position.X -
                                                                (((SpriteRenderer)player.GameObject.GetComponent(Tag.SPRITERENDERER)).Sprite.Width * attackRange),
                                                                player.GameObject.Transform.Position.Y -
                                                                (((SpriteRenderer)player.GameObject.GetComponent(Tag.SPRITERENDERER)).Sprite.Height / 2));

                    tmpMeleeRenderer.spriteEffect = SpriteEffects.FlipHorizontally;
                }

                tmpMeleeObject.Transform.Position = attackDirection;

                GameWorld.Instance.Colliders.Add(tmpMeleeCollider);
                GameWorld.Instance.GameObjects.Add(tmpMeleeObject);

                canAttack = false;
            }
        }

        public override Tag ToEnum()
        {
            return Tag.ATTACKMELEE;
        }

        #endregion
    }
}
