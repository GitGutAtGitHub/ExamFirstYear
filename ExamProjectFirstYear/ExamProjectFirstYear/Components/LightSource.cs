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
		#region FIELDS AND PROPERTIES

		private Texture2D lightMaskTexture;
		private Vector2 lightOrigin;

		public float LightRadiusScale { get; set; }
		public bool LightOn { get; set; }

		#endregion

		#region METHODS

		/// <summary>
		/// LightSources constructor.
		/// </summary>
		/// <param name="lightRadiusScale"></param>
		/// <param name="lightOn"></param>
		public LightSource(float lightRadiusScale, bool lightOn)
		{
			this.LightRadiusScale = lightRadiusScale;
			this.LightOn = lightOn;
		}

		/// <summary>
		/// Sets the lightMaskTexture and lightOrigin point.
		/// </summary>
		public override void Awake()
		{
			lightMaskTexture = GameWorld.Instance.Content.Load<Texture2D>("Lightmask");
			lightOrigin = new Vector2(lightMaskTexture.Width / 2, lightMaskTexture.Height / 2);
		}

		/// <summary>
		/// Adds this lightsource to the list of lightsources nin GameWorld.
		/// </summary>
		public override void Start()
		{
			GameWorld.Instance.LightSources.Add(this);
		}

		public override void Update(GameTime gameTime)
		{

		}

		private void ManageLightRadius()
		{

		}

		/// <summary>
		/// Draws the lightsource component.
		/// </summary>
		/// <param name="spriteBatch"></param>
		public override void Draw(SpriteBatch spriteBatch)
		{
			if (LightOn == true)
			{
				spriteBatch.Draw(lightMaskTexture, GameObject.Transform.Position, null, Color.White, 0, lightOrigin, LightRadiusScale, SpriteEffects.None, 1f);

			}
		}

		public override Tag ToEnum()
		{
			return Tag.LIGHTSOURCE;
		}

		#endregion
	}
}
