using ExamProjectFirstYear.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear
{
	class ProjectileFactory : IFactory
	{
		private Projectile bossProjectile;
		private SpriteRenderer bossProjectileRenderer;

		private Projectile playerMeleeObject;
		private SpriteRenderer playerMeleeObjectRenderer;

		private Projectile enemyMeleeObject;
		private SpriteRenderer enemyMeleeObjectRenderer;

		private Projectile playerProjectile;
		private SpriteRenderer playerProjectileRenderer;

		private Projectile enemyProjectile;
		private SpriteRenderer enemyProjectileRenderer;

		//private Projectile projectile;
		//private SpriteRenderer projectileRenderer;

		//private Projectile meleeObject;
		//private SpriteRenderer meleeObjectRenderer;

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
			CreateProjectilePrototype(ref bossProjectile, ref bossProjectileRenderer, "OopPlayerSprite2");
			CreateProjectilePrototype(ref playerProjectile, ref playerProjectileRenderer, "FlyingEnemy");
			CreateProjectilePrototype(ref playerMeleeObject, ref playerMeleeObjectRenderer, "MeleeObject2");
			CreateProjectilePrototype(ref enemyProjectile, ref enemyProjectileRenderer, "OopPlayerSprite2");
			CreateProjectilePrototype(ref enemyMeleeObject, ref enemyMeleeObjectRenderer, "OopPlayerSprite2");
			//CreateProjectilePrototype(ref meleeObject, ref meleeObjectRenderer);
			//CreateProjectilePrototype(ref projectile, ref projectileRenderer);
		}

		private void CreateProjectilePrototype(ref Projectile projectile, ref SpriteRenderer renderer, string spriteName)
		{
			projectile = new Projectile();
			renderer = new SpriteRenderer(spriteName);
			renderer.Origin = new Vector2(renderer.Sprite.Width / 2, renderer.Sprite.Height / 2);
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
					gameObject.AddComponent(new Collider(bossProjectileRenderer, (Projectile)gameObject.GetComponent(Tag.PROJECTILE)));
					gameObject.AddComponent(new Movement(false, 1000));
					break;
				case Tag.PLAYERPROJECTILE:
					gameObject.AddComponent(playerProjectile.Clone());
					gameObject.AddComponent(playerProjectileRenderer.Clone());
					gameObject.AddComponent(new Collider(playerProjectileRenderer, (Projectile)gameObject.GetComponent(Tag.PROJECTILE)));
					gameObject.AddComponent(new Movement(false, 1000));
					gameObject.AddComponent(new LightSource(0.25f, true));
					break;
				case Tag.PLAYERMELEEATTACK:
					gameObject.AddComponent(playerMeleeObject.Clone());
					gameObject.AddComponent(playerMeleeObjectRenderer.Clone());
					gameObject.AddComponent(new Collider(playerMeleeObjectRenderer, (Projectile)gameObject.GetComponent(Tag.PROJECTILE)));
					break;
				case Tag.ENEMYPROJECTILE:
					gameObject.AddComponent(enemyProjectile.Clone());
					gameObject.AddComponent(enemyProjectileRenderer.Clone());
					gameObject.AddComponent(new Collider(enemyProjectileRenderer, (Projectile)gameObject.GetComponent(Tag.PROJECTILE)));
					gameObject.AddComponent(new Movement(false, 1000));
					break;
				case Tag.ENEMYMELEEATTACK:
					gameObject.AddComponent(enemyMeleeObject.Clone());
					gameObject.AddComponent(enemyMeleeObjectRenderer.Clone());
					gameObject.AddComponent(new Collider(enemyMeleeObjectRenderer, (Projectile)gameObject.GetComponent(Tag.PROJECTILE)));
					break;
					//case Tag.MELEEATTACK:
					//                gameObject.AddComponent(meleeObject.Clone());
					//                gameObject.AddComponent(meleeObjectRenderer.Clone());
					//	gameObject.AddComponent(new Collider(meleeObjectRenderer, (Projectile)gameObject.GetComponent(Tag.PROJECTILE)));
					//                break;
					//            case Tag.PROJECTILE:
					//                gameObject.AddComponent(projectile.Clone());
					//                gameObject.AddComponent(projectileRenderer.Clone());
					//	gameObject.AddComponent(new Collider(projectileRenderer, (Projectile)gameObject.GetComponent(Tag.PROJECTILE)));
					//                gameObject.AddComponent(new Movement(false, 1000));
					//	break;
			}

			return gameObject;
		}
	}
}
