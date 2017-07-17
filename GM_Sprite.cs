using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNATemplate
{
    /// <summary>
    /// A sprite which can be drawn to the viewport with various transforms and attributes.
    /// </summary>
    public class GM_Sprite
    {
        #region Private Fields
        
        private Texture2D texture;
        
        private Rectangle destinationRect;
        private Rectangle sourceRect;
        
        private Color colour = Color.White;
        private float rotation = 0.0f;
        private Vector2 origin = Vector2.Zero;

        #endregion



        #region Member Access Modifiers

        /// <summary> The texture that is drawn. </summary>
        public Texture2D Texture { get { return texture; } set { texture = value; } }

        /// <summary> The destination bounds of the sprite. </summary>
        public Rectangle DestinationRect { get { return destinationRect; } set { destinationRect = value; } }
        /// <summary> The source rectangle that represents the region of the texture that should be drawn. </summary>
        public Rectangle SourceRect { get { return sourceRect; } set { sourceRect = value; } }

        /// <summary> The colour that is blended with the texture colour data. </summary>
        public Color Colour { get { return colour; } set { colour = value; } }
        /// <summary> Rotation of the sprite in degrees. </summary>
        public float Rotation { get { return GetRotationRadToDeg(); } set { SetRotationDegToRad(value); } }
        /// <summary> The origin of the sprite's transformations. Default is (0, 0) which represents the top left corner of the source rectangle. </summary>
        public Vector2 Origin { get { return origin; } set { origin = value; } }

        // Extra public properties

        /// <summary> The X and Y position of the sprite in screen coordinates. </summary>
        public Point Position { get { return new Point(X, Y); } set { X = value.X;  Y = value.Y; } }
        /// <summary> The X value of the sprite's position. </summary>
        public int X { get { return destinationRect.X; } set { destinationRect.X = value; } }
        /// <summary> The Y value of the sprite's position. </summary>
        public int Y { get { return destinationRect.Y; } set { destinationRect.Y = value; } }
        /// <summary> The width of the sprite in pixel units. </summary>
        public int Width { get { return destinationRect.Width; } set { destinationRect.Width = value; } }
        /// <summary> The height of the sprite in pixel units. </summary>
        public int Height { get { return destinationRect.Height; } set { destinationRect.Height = value; } }

        /// <summary> The sprite's texture scale. Defaults to (1, 1) which draws at the texture's dimensions. </summary>
        public Vector2 Scale { get { return new Vector2((float)Width / (float)texture.Width, (float)Height / (float)texture.Height); } set { SetScale(value); } }

        #endregion



        #region Constructors

        /// <summary>
        /// Creates a sprite with a specified texture positioned at (0, 0) with all default attributes.
        /// </summary>
        /// <param name="texture"> The texture the sprite will use to draw. </param>
        public GM_Sprite(Texture2D texture)
        {
            this.texture = texture;

            destinationRect = texture.Bounds;
            sourceRect = texture.Bounds;
        }

        /// <summary>
        /// Creates a sprite with a specified texture and position with the rest of the attributes set to default values.
        /// </summary>
        /// <param name="texture"> The texture the sprite will use to draw. </param>
        /// <param name="position"> The position the sprite will draw at in screen coordinates. </param>
        public GM_Sprite(Texture2D texture, Point position)
        {
            this.texture = texture;

            destinationRect = new Rectangle(position.X, position.Y, texture.Width, texture.Height);
            sourceRect = texture.Bounds;
        }

        /// <summary>
        /// Creates a sprite with a specified texture, position, and dimensions.
        /// </summary>
        /// <param name="texture"> The texture the sprite will use to draw. </param>
        /// <param name="destinationRect"> The rectangular bounds the sprite will be drawn to. </param>
        public GM_Sprite(Texture2D texture, Rectangle destinationRect)
        {
            this.texture = texture;

            this.destinationRect = destinationRect;
            sourceRect = texture.Bounds;
        }

        /// <summary>
        /// Creates a sprite with a specified texture and position and scales its dimensions relative to the texture's dimensions.
        /// </summary>
        /// <param name="texture"> The texture the sprite will use to draw. </param>
        /// <param name="position"> The position the sprite will draw at in screen coordinates. </param>
        /// <param name="scale"> The scale that will be used to draw the sprite's texture. </param>
        public GM_Sprite(Texture2D texture, Point position, Vector2 scale)
        {
            this.texture = texture;

            Position = position;
            Scale = scale;
            sourceRect = texture.Bounds;
        }

        #endregion



        #region Helper Methods

        // Private

        private float GetRotationRadToDeg()
        {
            return rotation * 57.2958f;
        }

        private void SetRotationDegToRad(float degrees)
        {
            rotation = degrees / 57.2958f;
        }

        private void SetScale(Vector2 scale)
        {
            destinationRect.Width = (int)(texture.Width * scale.X);
            destinationRect.Height = (int)(texture.Height * scale.Y);
        }

        // Public

        /// <summary>
        /// Center's the sprite's origin so that it will be transformed around its center. For example any rotations will be centered.
        /// </summary>
        public void CenterOrigin()
        {
            origin = new Vector2(sourceRect.Width / 2, sourceRect.Height / 2);
        }

        #endregion



        #region Draw Methods

        /// <summary>
        /// Draws the sprite to the specified spritebatch.
        /// </summary>
        /// <param name="spriteBatch"> The spritebatch to be drawn to. </param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, destinationRect, sourceRect, colour, rotation, origin, SpriteEffects.None, 0);
        }

        #endregion
    }
}
