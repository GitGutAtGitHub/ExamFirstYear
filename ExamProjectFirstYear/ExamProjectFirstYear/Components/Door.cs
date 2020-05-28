using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.Components
{
	class Door : Component
	{
		private bool isLocked = true;

		public Door()
		{

		}

		public override void Awake()
		{
			base.Awake();
		}

		public override void Start()
		{
			base.Start();
		}


		public void OpenDoor()
		{
			isLocked = false;
		}

		public override Tag ToEnum()
		{
			return Tag.DOOR;
		}
	}
}
