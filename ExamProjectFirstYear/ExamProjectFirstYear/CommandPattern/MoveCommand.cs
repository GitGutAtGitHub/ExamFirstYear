using ExamProjectFirstYear.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.CommandPattern
{
	/// <summary>
	/// MoveCommand for the player.
	/// </summary>
	class MoveCommand : ICommand
	{
		#region Fields

		private Vector2 velocity;

		#endregion


		#region Constructors

		/// <summary>
		/// Constructor for MoveCommand.
		/// </summary>
		/// <param name="velocity"></param>
		public MoveCommand(Vector2 velocity)
		{
			this.velocity = velocity;
		}

		#endregion


		#region Methods

		/// <summary>
		/// Executes the command.
		/// </summary>
		/// <param name="player"></param>
		public void Execute(Player player)
		{
			Movement playerMovement = (Movement)player.GameObject.GetComponent(Tag.MOVEMENT);
			player.Velocity = velocity;
			playerMovement.Move(velocity);

			SpriteRenderer playerRenderer = (SpriteRenderer)player.GameObject.GetComponent(Tag.SPRITERENDERER);
			playerRenderer.FlipSprite(velocity);
			playerRenderer.AnimationOn = true;

			AnimationHandler playerAnimation = (AnimationHandler)player.GameObject.GetComponent(Tag.ANIMATIONHANDLER);
			playerAnimation.MovingRight = true;
			playerAnimation.MovingLeft = true;
			//playerRenderer.SpriteEffect = SpriteEffects.FlipHorizontally;

			//((SoundComponent)player.GameObject.GetComponent(Tag.SOUNDCOMPONENT)).ChangeRepeat("footstepsLouder", true);
			//SoundEngine.Instance.AddSoundEffect(SoundEngine.Instance.Footsteps);

		}

		public CommandTag GetCommandTag()
		{
			return CommandTag.KEYDOWN;
		}

		#endregion
	}
}
