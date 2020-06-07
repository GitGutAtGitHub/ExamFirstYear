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
    /// <summary>
    /// public for unit test
    /// </summary>
    public class AttackMelee : Component
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

        public void MeleeAttack(GameObject sender, Tag meleeAttackType, Vector2 velocity)
        {
            if (canAttack)
            {
                GameObject tmpMeleeObject = ProjectileFactory.Instance.Create(meleeAttackType);
                SpriteRenderer tmpMeleeRenderer = (SpriteRenderer)tmpMeleeObject.GetComponent(Tag.SPRITERENDERER);
                Collider tmpMeleeCollider = (Collider)tmpMeleeObject.GetComponent(Tag.COLLIDER);

                ((SoundComponent)sender.GetComponent(Tag.SOUNDCOMPONENT)).StartPlayingSoundEffect("Whoosh m. reverb");

                //Makes sure the attack appears on the right side of the player.
                if (velocity.X > 0)
                {
                    attackDirection = new Vector2(sender.Transform.Position.X +
                                                                (((SpriteRenderer)sender.GetComponent(Tag.SPRITERENDERER)).Sprite.Width / attackRange),
                                                                sender.Transform.Position.Y -
                                                                (((SpriteRenderer)sender.GetComponent(Tag.SPRITERENDERER)).Sprite.Height / 2));

                    tmpMeleeRenderer.spriteEffect = SpriteEffects.None;
                }
                // Makes sure the attack appears on the left side of the player.
                if (velocity.X < 0)
                {
                    attackDirection = new Vector2(sender.Transform.Position.X -
                                                                (((SpriteRenderer)sender.GetComponent(Tag.SPRITERENDERER)).Sprite.Width * attackRange),
                                                                sender.Transform.Position.Y -
                                                                (((SpriteRenderer)sender.GetComponent(Tag.SPRITERENDERER)).Sprite.Height / 2));

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
