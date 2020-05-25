using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear
{
	class ProjectileFactory : IFactory
	{
		private float speed;
		private Vector2 velocity;

		private Projectile bossProjectile;
		private SpriteRenderer bossProjectileRenderer;

		private Projectile playerProjectile;
		private SpriteRenderer playerProjectileRenderer;

		private Projectile enemyProjectile;
		private SpriteRenderer enemyProjectileRenderer;

		private static ProjectileFactory instance;

		public static ProjectileFactory Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new ProjectileFactory();
				}

				return instance;
			}
		}

		private ProjectileFactory()
		{
			CreateProjectilePrototype(ref bossProjectile, ref bossProjectileRenderer, "OopPlayerSprite2", 500);
			CreateProjectilePrototype(ref playerProjectile, ref playerProjectileRenderer, "OopPlayerSprite2", 500);
			CreateProjectilePrototype(ref enemyProjectile, ref enemyProjectileRenderer, "OopPlayerSprite2", 500);
		}

		private void CreateProjectilePrototype(ref Projectile projectile, ref SpriteRenderer renderer, string spriteName, float speed)
		{
			projectile = new Projectile(speed);
			renderer = new SpriteRenderer(spriteName);
		}

		public GameObject Create(Tag type)
		{
			GameObject gameObject = new GameObject();
			gameObject.Tag = type;

			switch (type)
			{
				case Tag.BOSSPROJECTILE:
					gameObject.AddComponent(bossProjectile.Clone());
					gameObject.AddComponent(bossProjectileRenderer.Clone());
					gameObject.AddComponent(new Collider(bossProjectileRenderer));
					break;
				case Tag.PLAYERPROJECTILE:
					gameObject.AddComponent(playerProjectile.Clone());
					gameObject.AddComponent(playerProjectileRenderer.Clone());
					gameObject.AddComponent(new Collider(playerProjectileRenderer));
					break;
				case Tag.ENEMYPROJECTILE:
					gameObject.AddComponent(enemyProjectile.Clone());
					gameObject.AddComponent(enemyProjectileRenderer.Clone());
					gameObject.AddComponent(new Collider(enemyProjectileRenderer));
					break;
			}

			return gameObject;
		}
	}
}
