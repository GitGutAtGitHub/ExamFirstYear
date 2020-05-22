using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear
{
	public class SpriteRenderer : Component
	{
		#region PROPERTIES
		public Texture2D Sprite { get; set; }
		public Vector2 Origin { get; set; }

		#endregion

		/// <summary>
		/// Constructor for SpriteRenderer
		/// </summary>
		public SpriteRenderer()
		{

		}

		/// <summary>
		/// SpriteRenderers Start runs the SetSprite method and Sets the Origin.
		/// </summary>
		public override void Start()
		{
			SetSprite();
			Origin = new Vector2(Sprite.Width / 2, Sprite.Height / 2);
		}

		/// <summary>
		/// Sets the sprite based on the spritename belonging to the GameObject the SpriteRenderer is a component of.
		/// </summary>
		public void SetSprite()
		{
			Sprite = GameWorld.Instance.Content.Load<Texture2D>(GameObject.SpriteName);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(Sprite, GameObject.Transform.Position, null, Color.White, 0, Origin, 1, SpriteEffects.None, 0);
		}

		public override Tag ToEnum()
		{
			return Tag.SPRITERENDERER;
		}
	}
}
