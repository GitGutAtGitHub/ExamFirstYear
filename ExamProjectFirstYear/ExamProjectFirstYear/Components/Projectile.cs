using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear
{
	public class Projectile : Component
	{
		private float speed;
		private Vector2 velocity;

		public Projectile(float speed)
		{
			this.speed = speed;
		}

		public Projectile Clone()
		{
			return (Projectile)this.MemberwiseClone();
		}

		public override void Update(GameTime gameTime)
		{
		}

		public override void Destroy()
		{
			// Use GameEvent and Notify, if(OnHitObject, OnPastBorders) then destroy collider
			BossProjectilePool.Instance.ReleaseObject(GameObject);
		}

		public override Tag ToEnum()
		{
			return Tag.PROJECTILE;
		}

	}
}
