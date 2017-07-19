using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNATemplate
{
    /// <summary>
    /// A GM_ClickableSprite child class that takes a spritesheet instead of a single texture and 
    /// displays a different sprite depending on the sprites clicksate, creating a graphically more appealing button.
    /// </summary>
    public class GM_Button : GM_ClickableSprite
    {
        #region Private Fields
        
        private Rectangle[] sourceRects; // [0] = Up, [1] = Hovered, [2] = Down

        private GM_ClickState oldState = GM_ClickState.Up;

        #endregion



        #region Constructors

        /// <summary>
        /// Constructs a button with default bounds.
        /// </summary>
        /// <param name="spriteSheet"> The spritesheet to source the button's sprites from. </param>
        /// <param name="verticalSpriteSheet"> This specifies the positioning of the spritesheet's sprites. </param>
        public GM_Button(Texture2D spriteSheet, bool verticalSpriteSheet)
            : base(spriteSheet)
        {
            Initialize(spriteSheet, verticalSpriteSheet);
        }

        /// <summary>
        /// Constructs a button at a specified position.
        /// </summary>
        /// <param name="spriteSheet"> The spritesheet to source the button's sprites from. </param>
        /// <param name="verticalSpriteSheet"> This specifies the positioning of the spritesheet's sprites. </param>
        /// <param name="position"> The position of the button's origin (default is the top left corner of the sprite). </param>
        public GM_Button(Texture2D spriteSheet, bool verticalSpriteSheet, Point position)
            : base(spriteSheet, position)
        {
            Initialize(spriteSheet, verticalSpriteSheet);
        }

        /// <summary>
        /// Constructs a button with a specified position and size.
        /// </summary>
        /// <param name="spriteSheet"> The spritesheet to source the button's sprites from. </param>
        /// <param name="verticalSpriteSheet"> This specifies the positioning of the spritesheet's sprites. </param>
        /// <param name="destinationRect"> The button's bounds defining a position and size. </param>
        public GM_Button(Texture2D spriteSheet, bool verticalSpriteSheet, Rectangle destinationRect)
            : base(spriteSheet, destinationRect)
        {
            Initialize(spriteSheet, verticalSpriteSheet);
        }

        /// <summary>
        /// Constructs a button at a specified position and applies a scale to its texture.
        /// </summary>
        /// <param name="spriteSheet"> The spritesheet to source the button's sprites from. </param>
        /// <param name="verticalSpriteSheet"> This specifies the positioning of the spritesheet's sprites. </param>
        /// <param name="position"> The position of the button's origin (default is the top left corner of the sprite). </param>
        /// <param name="scale"> The scale to preform on the button's texture when drawing it. </param>
        public GM_Button(Texture2D spriteSheet, bool verticalSpriteSheet, Point position, Vector2 scale)
            : base(spriteSheet, position, scale)
        {
            Initialize(spriteSheet, verticalSpriteSheet);
        }

        /// <summary>
        /// Constructs a button with a specified position, size, and collection of source rectangles.
        /// </summary>
        /// <param name="spriteSheet"> The spritesheet to source the button's sprites from. </param>
        /// <param name="verticalSpriteSheet"> This specifies the positioning of the spritesheet's sprites. </param>
        /// <param name="destinationRect"> The button's bounds defining a position and size. </param>
        public GM_Button(Texture2D spriteSheet, bool verticalSpriteSheet, Rectangle destinationRect, Rectangle[] sourceRects)
            : base(spriteSheet, destinationRect)
        {
            this.sourceRects = sourceRects;

            SourceRect = sourceRects[0];
        }

        /// <summary>
        /// Initializes the sprite and calculates the spritesheet's source 
        /// rectangle depending on whether the sheet's sprites are vertically or horizontally aligned.
        /// </summary>
        /// <param name="spriteSheet"> The spritesheet to source the button's sprites from. </param>
        /// <param name="verticalSpriteSheet"> This specifies the positioning of the spritesheet's sprites. </param>
        private void Initialize(Texture2D spriteSheet, bool verticalSpriteSheet)
        {
            if (verticalSpriteSheet)
            {
                int width = spriteSheet.Width;
                int height = spriteSheet.Height / 3;

                sourceRects = new Rectangle[3];
                sourceRects[0] = new Rectangle(0, 0, width, height);
                sourceRects[1] = new Rectangle(0, height, width, height);
                sourceRects[2] = new Rectangle(0, height * 2, width, height);
            }
            else
            {
                int width = spriteSheet.Width / 3;
                int height = spriteSheet.Height;

                sourceRects = new Rectangle[3];
                sourceRects[0] = new Rectangle(0, 0, width, height);
                sourceRects[1] = new Rectangle(width, 0, width, height);
                sourceRects[2] = new Rectangle(width * 2, 0, width, height);
            }

            SourceRect = sourceRects[0];
        }

        #endregion



        #region Update Methods

        /// <summary>
        /// Updates the button's click state and the displayed sprite.
        /// </summary>
        /// <param name="mousePosition"> The position that is checked against the region </param>
        public override void Update(Point mousePosition)
        {
            base.Update(mousePosition);

            // Only make changes if there has been an actual change in click state.
            if (State != oldState)
            {
                Colour = Color.White;

                switch (State)
                {
                    case GM_ClickState.Up:
                        SourceRect = sourceRects[0];
                        break;

                    case GM_ClickState.Hovered:
                        SourceRect = sourceRects[1];
                        break;

                    // The button could have been constructed with less than 3 source 
                    // rectangles so only display a third if there is one.
                    case GM_ClickState.Down:
                        if (sourceRects.Length > 2)
                            SourceRect = sourceRects[2];
                        break;

                    // If it's disabled then we colour the button a dark gray to indicate this.
                    case GM_ClickState.Disabled:
                        SourceRect = sourceRects[0];
                        Colour = Color.DarkSlateGray;
                        break;
                }
            }

            // Store the current state for the next Update() call.
            oldState = State;
        }

        #endregion
    }
}
