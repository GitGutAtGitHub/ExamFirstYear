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

		public SpriteRenderer(string spriteName)
		{
			SetSprite(spriteName);
		}

		/// <summary>
		/// SpriteRenderers Start runs the SetSprite method and Sets the Origin.
		/// </summary>
		public override void Start()
		{
			SetSprite(GameObject.SpriteName);

			//har ændret deault origin til at være i hjørnet igen, ellers er det svært at lave level design med bitmap
			//det kan bare ændres specifikt til player osv
			//Origin = new Vector2(Sprite.Width / 2, Sprite.Height / 2);
		}

		/// <summary>
		/// Sets the sprite based on the spritename belonging to the GameObject the SpriteRenderer is a component of.
		/// </summary>
		public void SetSprite(string spriteName)
		{
			Sprite = GameWorld.Instance.Content.Load<Texture2D>(spriteName);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(Sprite, GameObject.Transform.Position, null, Color.White, 0, Origin, 1, SpriteEffects.None, 0);
		}

		public SpriteRenderer Clone()
		{
			return (SpriteRenderer)this.MemberwiseClone();
		}

		public override Tag ToEnum()
		{
			return Tag.SPRITERENDERER;
		}
	}
}
