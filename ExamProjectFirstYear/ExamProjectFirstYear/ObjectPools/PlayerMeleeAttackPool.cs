using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.ObjectPools
{
	class PlayerMeleeAttackPool : ObjectPool
	{
		private static PlayerMeleeAttackPool instance;

		public static PlayerMeleeAttackPool Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new PlayerMeleeAttackPool();
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
			return ProjectileFactory.Instance.Create(Tag.PLAYERMELEEATTACK);
		}
	}
}
