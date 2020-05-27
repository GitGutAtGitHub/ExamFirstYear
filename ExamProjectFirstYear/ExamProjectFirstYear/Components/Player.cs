using ExamProjectFirstYear.Components;
using ExamProjectFirstYear.ObjectPools;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear
{
	/// <summary>
	/// The Player Character class.
	/// </summary>
	public class Player : Component, IGameListener
	{
		#region Fields


		#endregion


		#region Properties

		public int Health { get; set; }
		public int JournalID { get; set; }
		public Movement Movement { get; private set; }
		public Vector2 Direction { get; set; } = new Vector2(1, 0);
		public bool canAttack { get; set; } = true;
		public bool canShoot { get; set; } = true;


		#endregion


		#region Constructors

		/// <summary>
		/// Constructor for the Player Character component.
		/// </summary>
		public Player()
		{
			//Lav en ny instans af JournalDB her.
		}

		#endregion


		#region Methods

		public override Tag ToEnum()
		{
			return Tag.PLAYER;
		}

		public override void Awake()
		{
			GameObject.Tag = Tag.PLAYER;
			GameObject.SpriteName = "OopPlayerSprite2";
		}

		public override void Start()
		{
			Movement = (Movement)GameObject.GetComponent(Tag.MOVEMENT);
			Movement.Momentum = GameWorld.Instance.ScreenSize.Y / 28 /*35*/;
			Movement.Speed = 500;
		}

		public void Notify(GameEvent gameEvent, Component component)
		{
			if (gameEvent.Title == "Colliding" && component.GameObject.Tag == Tag.PLATFORM)
			{

			}
		}

		public void ReleaseAttack(int attackNumber)
		{
			if (attackNumber == 1)
			{
				canAttack = true;
			}
			if (attackNumber == 2)
			{
				canShoot = true;
			}
		}


		/// <summary>
		/// Players method for attacking.
		/// </summary>
		/// <param name="attackNumber"></param>
		public void Attack(int attackNumber)
		{
			switch (attackNumber)
			{
				case 1:
					MeleeAttak();
					break;

				case 2:
					RangedAttack();
					break;
			}
		}

		/// <summary>
		/// Melee attack for Player.
		/// </summary>
		private void MeleeAttak()
		{
			if (canAttack)
			{
				GameObject tmpMeleeObject = PlayerMeleeAttackPool.Instance.GetObject();
				SpriteRenderer tmpMeleeRenderer = (SpriteRenderer)tmpMeleeObject.GetComponent(Tag.SPRITERENDERER);
				Collider tmpMeleeCollider = (Collider)tmpMeleeObject.GetComponent(Tag.COLLIDER);
				tmpMeleeObject.Transform.Position = this.GameObject.Transform.Position + (new Vector2(Direction.X * tmpMeleeRenderer.Sprite.Width, Direction.Y));
				GameWorld.Instance.GameObjects.Add(tmpMeleeObject);
				GameWorld.Instance.Colliders.Add(tmpMeleeCollider);
				canAttack = false;
			}
		}

		/// <summary>
		/// Ranged attack for Player.
		/// </summary>
		private void RangedAttack()
		{
			//MANGLER KODE DER FORHINDRER AT MAN KAN LAVE MERE END ÉT ANGREB AF GANGEN 
			//MANGLER OGSÅ KODE DER SØRGER FOR AT SPILLEREN MISTER LYS/MANA.
			if (canShoot)
			{
				GameObject tmpProjectileObject = PlayerProjectilePool.Instance.GetObject();
				Collider tmpProjectileCollider = (Collider)tmpProjectileObject.GetComponent(Tag.COLLIDER);
				tmpProjectileObject.Transform.Position = this.GameObject.Transform.Position;
				Movement tmpMovement = (Movement)tmpProjectileObject.GetComponent(Tag.MOVEMENT);
				tmpMovement.Velocity = Direction;
				//tmpMovement.Speed = 1000f;
				GameWorld.Instance.Colliders.Add(tmpProjectileCollider);
				GameWorld.Instance.GameObjects.Add(tmpProjectileObject);
				canShoot = false;
			}
		}



		#endregion
	}
}
