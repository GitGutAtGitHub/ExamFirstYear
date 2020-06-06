using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear
{
	class EnemyProjectilePool : ObjectPool
	{
		private static EnemyProjectilePool instance;

		public static EnemyProjectilePool Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new EnemyProjectilePool();
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
			return ProjectileFactory.Instance.Create(Tag.ENEMYPROJECTILE, sender);
		}
	}
}
