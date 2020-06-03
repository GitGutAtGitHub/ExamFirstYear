using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.Components
{
    /// <summary>
    /// Component for game platforms.
    /// </summary>
    class Platform : Component
    {
        #region Methods

        public override Tag ToEnum()
        {
            return Tag.PLATFORM;
        }

        public override void Awake()
        {
            GameObject.Tag = Tag.PLATFORM;
        }

		public override void Start()
		{
			GameObject.SpriteName = "Platform";
		}

        #endregion
    }
}
