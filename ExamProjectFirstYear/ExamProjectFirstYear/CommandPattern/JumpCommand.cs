using ExamProjectFirstYear.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.CommandPattern
{
	/// <summary>
	/// Jump command for the player.
	/// </summary>
	class JumpCommand : ICommand
	{
		#region Constructors

		/// <summary>
		/// Constructor for JumpCommand.
		/// </summary>
		public JumpCommand()
		{

		}

		#endregion

		// Check on material er sat sammen med player pos

		#region Methods

		public void Execute(Player player)
		{
			player.SoundComponent.StartPlayingSoundInstance("Jump_04");
			player.Jump.PlayerJump(player.Movement);
			player.SpriteRenderer.SetSprite("smol");
		}
        #endregion
    }
}
