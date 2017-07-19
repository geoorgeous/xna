using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace XNATemplate
{
    /// <summary>
    /// A class that keeps track of the click state in a certain region of the screen.
    /// </summary>
    public class GM_ClickableRegion
    {
        #region Private Fields

        // The 2D region to monitor.
        private Rectangle region;

        // Mouse button to register with.
        private GM_MouseButton button = GM_MouseButton.Left;
        // The state of the region. Normal state is Up.
        private GM_ClickState state = GM_ClickState.Up;
        // Click flag.
        private bool clicked;

        #endregion



        #region Member Access Modiefiers

        /// <summary> The current Click State of the clickable region. </summary>
        public GM_ClickState State { get { return state; } }
        /// <summary> Returns true if the region has registered a valid click. </summary>
        public bool IsClicked { get { return clicked; } }
        /// <summary> Gets and Sets if the clickable region can be clicked or not. </summary>
        public bool IsDisabled { get { return state == GM_ClickState.Disabled; } set { state = (value) ? GM_ClickState.Disabled : GM_ClickState.Up; } }

        #endregion



        #region Constructors

        /// <summary>
        /// Constructs a clickable region which registers click with the default mouse button (left).
        /// </summary>
        /// <param name="region"> The square 2D bounds to monitor for mouse clicks. </param>
        public GM_ClickableRegion(Rectangle region)
        {
            this.region = region;
        }


        /// <summary>
        /// Constructs a clickable region which registers click with a specified mouse button.
        /// </summary>
        /// <param name="region"> The square 2D bounds to monitor for mouse clicks. </param>
        /// <param name="button"> The mouse button to check for clicks with. </param>
        public GM_ClickableRegion(Rectangle region, GM_MouseButton button)
        {
            this.region = region;
            this.button = button;
        }

        #endregion



        #region Update Logic

        /// <summary>
        /// Updates the state of the clickable region. This will execute the logic which will change the 
        /// clickable region's state depending on the state and position of the mouse.
        /// </summary>
        public void Update(Point mousePosition)
        {
            // Reset the clicked flag to false every frame. 
            // This means that the region can only be clicked for a single frame.
            clicked = false;

            // Don't do anything if it's disabled
            if (state != GM_ClickState.Disabled)
            {
                // Create a bounding box the size of one pixel at the mouse position. This will give us a rectangle to check for intersection with.
                Rectangle mouseBounds = new Rectangle(mousePosition.X, mousePosition.Y, 1, 1);

                if (mouseBounds.Intersects(region))
                {
                    // If the mouse intersects with the region AND the button is not clicked then we always set the state to HOVERED.
                    // ... Otherwise if the mouse is down (not up) then we check to see if the current state == hovered.
                    // If that's true then we can safely set the state to DOWN.

                    // If the mouse intersects the region AND the button is up AND the current state is down then we know that this is a valid click.

                    if (GM_Input.Mouse_IsButtonUp(button, true))
                    {
                        if (state == GM_ClickState.Down)
                        {
                            clicked = true;
                        }

                        state = GM_ClickState.Hovered;
                    }
                    else if (state == GM_ClickState.Hovered)
                    {
                        state = GM_ClickState.Down;
                    }
                }
                else
                {
                    // We don't do much if the mouse does not intersect the region -
                    // just set the state to Up is the button is up.

                    if (GM_Input.Mouse_IsButtonUp(button, true))
                    {
                        state = GM_ClickState.Up;
                    }

                    // Because we don't do much here this allows for nice button behaviour:
                    // If you hold down the clickable region and leave the region the region will still be held down until you release the mouse button,
                    // however a valid click will only register if you release the button again inside the region.
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// Represents several possible states a clickable region could have.
    /// </summary>
    public enum GM_ClickState
    {
        Disabled,
        Up,
        Hovered,
        Down
    }
}
