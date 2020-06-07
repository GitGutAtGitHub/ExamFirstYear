using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.ObjectPools
{
    class ProjectilePool : ObjectPool
    {
		private static ProjectilePool instance;

		public static ProjectilePool Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new ProjectilePool();
				}

				return instance;
			}
		}

		protected override void CleanUp(GameObject gameObject)
		{
			// Tilføj cleanup kode her
		}

		protected override GameObject Create(Tag sender)
		{
			return ProjectileFactory.Instance.Create(Tag.PROJECTILE);
		}
	}
}
