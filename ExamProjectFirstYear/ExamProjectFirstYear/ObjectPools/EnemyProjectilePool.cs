using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear
{
	class EnemyProjectilePool : ObjectPool
	{
		protected override void CleanUp(GameObject gameObject)
		{
			// Tilføj cleanup kode her
		}

		protected override GameObject Create()
		{
			return ProjectileFactory.Instance.Create(Tag.ENEMYPROJECTILE);
		}
	}
}
