using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear
{
	class PlayerProjectilePool : ObjectPool
	{
		private static PlayerProjectilePool instance;

		public static PlayerProjectilePool Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new PlayerProjectilePool();
				}

				return instance;
			}
		}

		protected override void CleanUp(GameObject gameObject)
		{
			// Tilføj cleanup kode her
		}

		protected override GameObject Create()
		{
			return ProjectileFactory.Instance.Create(Tag.PLAYERPROJECTILE);
		}
	}
}
