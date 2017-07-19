using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNATemplate
{
    /// <summary>
    /// A Sprite with a clickable region component.
    /// </summary>
    public class GM_ClickableSprite : GM_Sprite
    {
        #region Private Fields

        // Rather than deriving from GM_ClickableRegion, we just keep a local variable of one.
        private GM_ClickableRegion clickRegion;

        #endregion



        #region Member Access Modifiers

        // Expose the clickRegions properties as if they were the sprite's own.

        /// <summary> The current Click State of the clickable sprite. </summary>
        public GM_ClickState State { get { return clickRegion.State; } }
        /// <summary> Returns true if the sprite has registered a valid click. </summary>
        public bool IsClicked { get { return clickRegion.IsClicked; } }
        /// <summary> Gets and Sets if the sprite can be clicked or not. </summary>
        public bool IsDisabled { get { return clickRegion.IsDisabled; } set { clickRegion.IsDisabled = value; } }

        #endregion



        #region Constructors

        /// <summary>
        /// Constructs a clickable sprite without a specific bounds.
        /// </summary>
        /// <param name="texture"> The texture to be given to the sprite. </param>
        public GM_ClickableSprite(Texture2D texture)
            : base(texture)
        {
            clickRegion = new GM_ClickableRegion(DestinationRect);
        }

        /// <summary>
        /// Constructs a clickable sprite at a specified position.
        /// </summary>
        /// <param name="texture"> The texture to be given to the sprite. </param>
        /// <param name="position"> The position of the sprite's origin (default is the top left corner of the sprite). </param>
        public GM_ClickableSprite(Texture2D texture, Point position)
            : base(texture, position)
        {
            clickRegion = new GM_ClickableRegion(DestinationRect);
        }

        /// <summary>
        /// Constructs a clickable sprite with a specified bounds.
        /// </summary>
        /// <param name="texture"> The texture to be given to the sprite. </param>
        /// <param name="destinationRect"> The sprite's bounds. </param>
        public GM_ClickableSprite(Texture2D texture, Rectangle destinationRect)
            : base(texture, destinationRect)
        {
            clickRegion = new GM_ClickableRegion(DestinationRect);
        }

        /// <summary>
        /// Constructs a clickable sprite at a specified position.
        /// </summary>
        /// <param name="texture"> The texture to be given to the sprite. </param>
        /// <param name="position"> The position of the sprite's origin (default is the top left corner of the sprite). </param>
        /// <param name="scale"> The scale to preform on the sprite's texture when drawing it. </param>
        public GM_ClickableSprite(Texture2D texture, Point position, Vector2 scale)
            : base(texture, position, scale)
        {
            clickRegion = new GM_ClickableRegion(DestinationRect);
        }

        #endregion



        #region Update Methods

        /// <summary>
        /// Updates the sprite's clickable region component.
        /// </summary>
        /// <param name="mousePosition"></param>
        public virtual void Update(Point mousePosition)
        {
            clickRegion.Update(mousePosition);
        }

        #endregion
    }
}
