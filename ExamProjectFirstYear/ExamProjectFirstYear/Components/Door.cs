using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.Components
{
	/// <summary>
	/// Door component class.
	/// </summary>
	class Door : Component
	{
		#region Fields

		private bool isLocked = true;

		#endregion


		#region Constructors

		/// <summary>
		/// Constructor for Door.
		/// </summary>
		public Door()
		{

		}

		#endregion


		#region Override methods

		public override void Awake()
		{
			base.Awake();
		}

		public override void Start()
		{
			base.Start();
		}

		public override Tag ToEnum()
		{
			return Tag.DOOR;
		}

		#endregion


		#region Other methods

		/// <summary>
		/// Method for setting a door to unlocked after opening it.
		/// </summary>
		public void OpenDoor()
		{
			isLocked = false;
		}

        #endregion
    }
}
