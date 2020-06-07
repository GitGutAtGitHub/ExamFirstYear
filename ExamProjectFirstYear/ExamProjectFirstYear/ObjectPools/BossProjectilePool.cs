using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear
{
	class BossProjectilePool : ObjectPool
	{
		private static BossProjectilePool instance;

		public static BossProjectilePool Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new BossProjectilePool();
				}

				return instance;
			}
		}

		protected override GameObject Create(Tag sender)
		{
			return ProjectileFactory.Instance.Create(Tag.BOSSPROJECTILE);
		}

		protected override void CleanUp(GameObject gameObject)
		{
			// Ikke sikker på hvad vi skal bruge den til endnu, måske til at sørge for at der er at maks antal objekter i poolen?
		}
	}
}
