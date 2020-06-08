using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.Components
{
	class AnimationHandler : Component
	{
		private SpriteRenderer spriteRenderer;
		private int currentIndex = 0;
		private float elapsedTime;
		private float fps;

		public string[] SpritesNames { get; set; }
		public bool MovingRight { get; set; }
		public bool MovingLeft { get; set; }

		public AnimationHandler(SpriteRenderer spriteRenderer, float fps, string[] spritesNames)
		{
			this.spriteRenderer = spriteRenderer;
			this.fps = fps;
			SpritesNames = spritesNames;
		}

		public override void Start()
		{
			SetAnimationSpritesArray(SpritesNames);
		}

		public override void Update(GameTime gameTime)
		{
			if (MovingRight == false && MovingLeft == false)
			{
				spriteRenderer.AnimationOn = false;
			}
			if (spriteRenderer.AnimationOn == true)
			{
				RunThroughAnimationSpritesArray();
			}
		}

		public void SetAnimationSpritesArray(string[] spritesNames)
		{
			spriteRenderer.Sprites = new Texture2D[spritesNames.Length];
			for (int i = 0; i < spritesNames.Length; i++)
			{
				spriteRenderer.Sprites[i] = GameWorld.Instance.Content.Load<Texture2D>(spritesNames[i]);
			}
		}

		public void RunThroughAnimationSpritesArray()
		{
			elapsedTime += (float)GameWorld.Instance.ElapsedGameTime.TotalSeconds;
			spriteRenderer.CurrentIndex = (int)(elapsedTime * fps);

			if (spriteRenderer.CurrentIndex >= SpritesNames.Length)
			{
				elapsedTime = 0;
				spriteRenderer.CurrentIndex = 0;
			}
		}

		

		public override Tag ToEnum()
		{
			return Tag.ANIMATIONHANDLER;
		}

	}
}
