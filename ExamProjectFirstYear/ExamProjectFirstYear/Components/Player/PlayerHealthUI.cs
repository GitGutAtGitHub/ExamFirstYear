using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.Components.Player
{
    /// <summary>
    /// The class that handles the UI for the Players health.
    /// </summary>
    public class PlayerHealthUI : Component
    {
        #region Fields

        private SpriteRenderer playerHealthUIRenderer;

        private float playerPositionX;
        private float playerPositionY;

        #endregion


        #region Constructors

        /// <summary>
        /// Constructor for PlayerHealthUI.
        /// </summary>
        public PlayerHealthUI()
        {

        }

        #endregion


        #region Override methods

        public override void Awake()
        {
            GameObject.Tag = Tag.PLAYERHEALTHUI;
            GameObject.SpriteName = "OopHeartSprite2";

            playerHealthUIRenderer = (SpriteRenderer)GameObject.GetComponent(Tag.SPRITERENDERER);
        }

        public override void Update(GameTime gameTime)
        {
            HandlePosition();
        }

        public override Tag ToEnum()
        {
            return Tag.PLAYERHEALTHUI;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            float positionX = playerPositionX + 750;

            for (int i = 0; i < GameWorld.Instance.player.Health; i++)
            {
                spriteBatch.Draw(playerHealthUIRenderer.Sprite, new Vector2(positionX, playerPositionY - 500), null, 
                                 Color.White, 0, playerHealthUIRenderer.Origin, 1, SpriteEffects.None, playerHealthUIRenderer.SpriteLayer);

                positionX -= 180;
            }
        }

        #endregion


        #region Other methods

        /// <summary>
        /// Handles the health UI position according to the players position.
        /// </summary>
        private void HandlePosition()
        {
            playerPositionX = GameWorld.Instance.player.GameObject.Transform.Position.X;
            playerPositionY = GameWorld.Instance.player.GameObject.Transform.Position.Y;
        }

        #endregion
    }
}
