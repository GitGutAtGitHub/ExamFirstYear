using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.Components
{
	public class LightSource : Component
	{
		//private float radius;
		private int intensity;

		private Texture2D lightMaskTexture;
		private Effect lightEffect;
		//private RenderTarget2D mainTarget;
		private float lightRadiusScale;
		private bool lightOn;

		public LightSource(float lightRadiusScale, bool lightOn)
		{
			this.lightRadiusScale = lightRadiusScale;
			this.lightOn = lightOn;

		}

		public override void Awake()
		{
			lightMaskTexture = GameWorld.Instance.Content.Load<Texture2D>("Lightmask");
			lightEffect = GameWorld.Instance.Content.Load<Effect>("LightEffect");
		}

		public override void Start()
		{
			GameWorld.Instance.LightSources.Add(this);
		}

		public override void Update(GameTime gameTime)
		{
			
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			if (lightOn == true)
			{
				spriteBatch.Draw(lightMaskTexture, GameObject.Transform.Position, null, Color.White, 0, new Vector2(lightMaskTexture.Width/2, lightMaskTexture.Height/2), lightRadiusScale, SpriteEffects.None, 1f);
				
			}
		}

		public override Tag ToEnum()
		{
			return Tag.LIGHTSOURCE;
		}
	}
}
