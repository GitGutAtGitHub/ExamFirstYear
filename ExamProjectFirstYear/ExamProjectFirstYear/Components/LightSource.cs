using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.Components
{
	/// <summary>
	/// Class for managing the lightsources.
	/// </summary>
	class LightSource : Component
	{
		#region Fields

		private float radius;
		private int intensity;
		private bool lightOn;

		#endregion


		#region Constructors

		/// <summary>
		/// Constructor for LightSource
		/// </summary>
		public LightSource()
		{

		}

		#endregion


		#region Override methods

		public override void Awake()
		{
			base.Awake();
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}

		public override Tag ToEnum()
		{
			return Tag.LIGHTSOURCE;
		}

        #endregion
    }
}
