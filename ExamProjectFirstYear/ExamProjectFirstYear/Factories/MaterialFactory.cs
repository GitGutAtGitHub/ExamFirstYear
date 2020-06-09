using ExamProjectFirstYear.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.Factories
{
	class MaterialFactory : IFactory
	{
		private Material spiderFilament;
		private SpriteRenderer spiderFilamentRenderer;

		private Material mothWing;
		private SpriteRenderer mothWingRenderer;

		private Material matchHead;
		private SpriteRenderer matchHeadRenderer;

		private static MaterialFactory instance;

		public static MaterialFactory Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new MaterialFactory();
				}

				return instance;
			}
		}

		private MaterialFactory()
		{
			CreateMaterialPrototype(ref spiderFilament, ref spiderFilamentRenderer, "OopBossProjectileSprite2", 1);
			CreateMaterialPrototype(ref matchHead, ref matchHeadRenderer, "OopPlayerSprite2", 2);
			CreateMaterialPrototype(ref mothWing, ref mothWingRenderer, "OopBossProjectileSprite2", 3);
		}

		private void CreateMaterialPrototype(ref Material material, ref SpriteRenderer renderer, string spriteName, int materialID)
		{
			material = new Material(materialID);
			renderer = new SpriteRenderer(spriteName);
			renderer.Origin = new Vector2(renderer.Sprite.Width / 2, renderer.Sprite.Height / 2);
		}

		public GameObject Create(Tag type)
		{
			GameObject gameObject = new GameObject();
			Collider collider;
			gameObject.Tag = type;

			switch (type)
			{
				case Tag.SPIDERFILAMENT:
					gameObject.AddComponent(spiderFilament.Clone());
					gameObject.AddComponent(spiderFilamentRenderer.Clone());
					collider = new Collider(spiderFilamentRenderer, (Material)gameObject.GetComponent(Tag.MATERIAL)) { CheckCollisionEvents = true};
                    gameObject.AddComponent(collider);
					gameObject.AddComponent(new Movement(true, 0));
					break;
				case Tag.MATCHHEAD:
					gameObject.AddComponent(matchHead.Clone());
					gameObject.AddComponent(matchHeadRenderer.Clone());
					collider = new Collider(matchHeadRenderer, (Material)gameObject.GetComponent(Tag.MATERIAL)) { CheckCollisionEvents = true};
                    gameObject.AddComponent(collider);
                    gameObject.AddComponent(new Movement(true, 0));
					break;
				case Tag.MOTHWING:
					gameObject.AddComponent(mothWing.Clone());
					gameObject.AddComponent(mothWingRenderer.Clone());
					collider = new Collider(mothWingRenderer, (Material)gameObject.GetComponent(Tag.MATERIAL)) { CheckCollisionEvents = true };
					gameObject.AddComponent(collider);
					gameObject.AddComponent(new Movement(true, 0));
					break;
			}
			return gameObject;
		}
	}
}
