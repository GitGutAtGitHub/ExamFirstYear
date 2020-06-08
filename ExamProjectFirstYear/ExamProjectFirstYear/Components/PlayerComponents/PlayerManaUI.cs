using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear.Components
{
    /// <summary>
    /// The class that handles the UI for the Players mana.
    /// </summary>
    public class PlayerManaUI : Component
    {
        #region Fields

        private SpriteRenderer playerManaUIRenderer;

        private float playerPositionX;
        private float playerPositionY;

        #endregion


        #region Constructors

        /// <summary>
        /// Constructor for PlayerHealthUI.
        /// </summary>
        public PlayerManaUI()
        {

        }

        #endregion


        #region Override methods

        public override void Awake()
        {
            GameObject.Tag = Tag.PLAYERMANAUI;
            GameObject.SpriteName = "OopHeartSprite2";

            playerManaUIRenderer = (SpriteRenderer)GameObject.GetComponent(Tag.SPRITERENDERER);
        }

        public override void Update(GameTime gameTime)
        {
            HandlePosition();
        }

        public override Tag ToEnum()
        {
            return Tag.PLAYERMANAUI;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            float positionX = playerPositionX + 750;

            for (int i = 0; i < GameWorld.Instance.Player.Mana; i++)
            {
                spriteBatch.Draw(playerManaUIRenderer.Sprite, new Vector2(positionX, playerPositionY - 300), null,
                                 Color.White, 0, playerManaUIRenderer.Origin, 1 * GameWorld.Instance.Scale, SpriteEffects.None, playerManaUIRenderer.SpriteLayer);

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
            playerPositionX = GameWorld.Instance.Player.GameObject.Transform.Position.X;
            playerPositionY = GameWorld.Instance.Player.GameObject.Transform.Position.Y;
        }

        #endregion
    }
}
