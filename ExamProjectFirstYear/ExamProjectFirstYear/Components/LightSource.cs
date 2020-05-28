using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.Components
{
	class LightSource : Component
	{
		private float radius;
		private int intensity;
		private bool lightOn;

		public LightSource()
		{

		}

		public override void Awake()
		{
			base.Awake();
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
