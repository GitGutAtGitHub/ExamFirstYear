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
    class AttackMelee : Component, IGameListener
    {
        private float attackRange = 1.2f;
        private float attackRangeLeft = -1.2f;
        private Vector2 attackDirection;

        public bool canAttack { get; set; } = true;


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
                //tmpMeleeRenderer.Origin = new Vector2(tmpMeleeRenderer.Sprite.Width / 2, tmpMeleeRenderer.Sprite.Height / 2);
                //Collider tmpMeleeCollider = new Collider(tmpMeleeRenderer, (AttackMelee)tmpMeleeObject.GetComponent(Tag.ATTACKMELEE)) { CheckCollisionEvents = true };
                Collider tmpMeleeCollider = (Collider)tmpMeleeObject.GetComponent(Tag.COLLIDER);

                if(player.Direction.X == 1)
                {
                    attackDirection = new Vector2(player.GameObject.Transform.Position.X +
                                                                (((SpriteRenderer)player.GameObject.GetComponent(Tag.SPRITERENDERER)).Sprite.Width / attackRange),
                                                                player.GameObject.Transform.Position.Y -
                                                                (((SpriteRenderer)player.GameObject.GetComponent(Tag.SPRITERENDERER)).Sprite.Height / 2));
                }
                if (player.Direction.X == -1)
                {
                    attackDirection = new Vector2(player.GameObject.Transform.Position.X -
                                                                (((SpriteRenderer)player.GameObject.GetComponent(Tag.SPRITERENDERER)).Sprite.Width * attackRange),
                                                                player.GameObject.Transform.Position.Y -
                                                                (((SpriteRenderer)player.GameObject.GetComponent(Tag.SPRITERENDERER)).Sprite.Height / 2));
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

        public void Notify(GameEvent gameEvent, Component component)
        {

        }
    }
}
