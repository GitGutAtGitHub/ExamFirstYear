using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.ObjectPools
{
    class MeleeAttackPool : ObjectPool
    {
		private static MeleeAttackPool instance;

		public static MeleeAttackPool Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new MeleeAttackPool();
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
			return ProjectileFactory.Instance.Create(Tag.MELEEATTACK);
		}
	}
}

