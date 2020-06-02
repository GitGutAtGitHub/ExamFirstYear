using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.Components
{
	class LightSource : Component
	{
		//private float radius;
		private int intensity;
		

		private Texture2D lightMaskTexture;
		private Effect lightEffect;
		private RenderTarget2D lightsTarget;
		private RenderTarget2D mainTarget;
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

		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}

		public override Tag ToEnum()
		{
			return Tag.LIGHTSOURCE;
		}
	}
}
