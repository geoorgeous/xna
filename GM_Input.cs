using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace XNATemplate
{
    /// <summary>
    /// A simple input management class that preforms some basic mouse and keyboard checks.
    /// </summary>
    public static class GM_Input
    {
        #region Private Fields

        // Keep the old and new states for comparisons between the two.
        // This way we can see if the states have recently changed.

        private static MouseState oldMouseState;
        private static MouseState newMouseState;

        private static KeyboardState oldKeyboardState;
        private static KeyboardState newKeyboardState;

        #endregion



        #region Update

        /// <summary>
        /// Updates the input states after capturing the current ones so we 
        /// have the last states and the most recent for comparisons.
        /// </summary>
        public static void Update()
        {
            oldMouseState = newMouseState;
            newMouseState = Mouse.GetState();

            oldKeyboardState = newKeyboardState;
            newKeyboardState = Keyboard.GetState();
        }

        #endregion



        #region Mouse Methods

        /// <summary>
        /// Returns the X and Y position of the mouse cursor.
        /// </summary>
        /// <param name="mostRecentState"> Specifies which state to check: the most recent or the state saved from the last Update() call. </param>
        /// <returns> A 2D Point representing the cursor position in screen coordinates. </returns>
        public static Point Mouse_GetCursorPos(bool mostRecentState)
        {
            return (mostRecentState) ? new Point(newMouseState.X, newMouseState.Y) : new Point(oldMouseState.X, oldMouseState.Y);
        }


        /// <summary>
        /// Returns the X and Y position of the mouse cursor as a 1x1 rectangle.
        /// </summary>
        /// <param name="mostRecentState"> Specifies which state to check: the most recent or the state saved from the last Update() call. </param>
        /// <returns> A 1x1 Rectangle representing the cursor bounds. </returns>
        public static Rectangle Mouse_GetCursorBounds(bool mostRecentState)
        {
            return (mostRecentState) ? new Rectangle(newMouseState.X, newMouseState.Y, 1, 1) : new Rectangle(oldMouseState.X, oldMouseState.Y, 1, 1);
        }

        /// <summary>
        /// Checks to see if the specified button is up.
        /// </summary>
        /// <param name="button"> Specifies which button to check is up. </param>
        /// <param name="mostRecentState"> Specifies which state to check: the most recent or the state saved from the last Update() call. </param>
        /// <returns> Returns true if it's found that the button is up. </returns>
        public static bool Mouse_IsButtonUp(GM_MouseButton button, bool mostRecentState)
        {
            switch (button)
            {
                case GM_MouseButton.Left:
                    return (mostRecentState) ? newMouseState.LeftButton == ButtonState.Released : oldMouseState.LeftButton == ButtonState.Released;

                case GM_MouseButton.Middle:
                    return (mostRecentState) ? newMouseState.MiddleButton == ButtonState.Released : oldMouseState.MiddleButton == ButtonState.Released;

                case GM_MouseButton.Right:
                    return (mostRecentState) ? newMouseState.RightButton == ButtonState.Released : oldMouseState.RightButton == ButtonState.Released;

                default:
                    return false;
            }
        }


        /// <summary>
        /// Checks to see if the specified button is down.
        /// </summary>
        /// <param name="button"> Specifies which button to check is down. </param>
        /// <param name="mostRecentState"> Specifies which state to check: the most recent or the state saved from the last Update() call. </param>
        /// <returns> Returns true if it's found that the button is down. </returns>
        public static bool Mouse_IsButtonDown(GM_MouseButton button, bool mostRecentState)
        {
            // Simply negate the Mouse_IsButtonUp() method as if the button is'nt up, it can only be down!
            return !Mouse_IsButtonUp(button, mostRecentState);
        }

        /// <summary>
        /// Checks to see if the button was up on the last frame and down on the most recent frame. 
        /// This would indicate the button was pressed during the most recent Update() call.
        /// </summary>
        /// <param name="button"> The button to check has just been pressed. </param>
        /// <returns> Returns true if the button is found to have just been pressed. </returns>
        public static bool Mouse_IsButtonJustPressed(GM_MouseButton button)
        {
            return Mouse_IsButtonUp(button, false) && Mouse_IsButtonDown(button, true);
        }


        /// <summary>
        /// Checks to see if the button was down on the last frame and up on the most recent frame. 
        /// This would indicate the button was released during the most recent Update() call.
        /// </summary>
        /// <param name="button"> The button to check has just been released. </param>
        /// <returns> Returns true if the button is found to have just been released. </returns>
        public static bool Mouse_IsButtonJustReleased(GM_MouseButton button)
        {
            return Mouse_IsButtonDown(button, false) && Mouse_IsButtonUp(button, true);
        }

        #endregion



        #region Keyboard Methods

        /// <summary>
        /// Checks to see if a specific keyboard key is up.
        /// </summary>
        /// <param name="key"> The keyboard key to check is up. </param>
        /// <param name="mostRecentState"> Specifies which state to check: the most recent or the state saved from the last Update() call. </param>
        /// <returns> Returns true if the key is found to be up. </returns>
        public static bool KeyBrd_IsKeyUp(Keys key, bool mostRecentState)
        {
            return (mostRecentState) ? newKeyboardState.IsKeyUp(key) : oldKeyboardState.IsKeyUp(key);
        }

        /// <summary>
        /// Checks to see if a specific keyboard key is down.
        /// </summary>
        /// <param name="key"> The keyboard key to check is down. </param>
        /// <param name="mostRecentState"> Specifies which state to check: the most recent or the state saved from the last Update() call. </param>
        /// <returns> Returns true if the key is found to be down. </returns>
        public static bool KeyBrd_IsKeyDown(Keys key, bool mostRecentState)
        {
            return (mostRecentState) ? newKeyboardState.IsKeyDown(key) : oldKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Checks to see if the key was up on the last frame and down on the most recent frame. 
        /// This would indicate that the key was pressed down during the most recent Update() call.
        /// </summary>
        /// <param name="key"> The keyboard key to check has just been pressed. </param>
        /// <returns> Returns true if the key is found to have just been pressed. </returns>
        public static bool KeyBrd_IsKeyJustPressed(Keys key)
        {
            return KeyBrd_IsKeyUp(key, false) && KeyBrd_IsKeyDown(key, true);
        }
        
        /// <summary>
        /// Checks to see if the key was down on the last frame and up on the most recent frame. 
        /// This would indicate that the key was released during the most recent Update() call.
        /// </summary>
        /// <param name="key"> The keyboard key to check has just been released. </param>
        /// <returns> Returns true if the key is found to have just been released. </returns>
        public static bool KeyBrd_IsKeyJustReleased(Keys key)
        {
            return KeyBrd_IsKeyDown(key, false) && KeyBrd_IsKeyUp(key, true);
        }

        #endregion
    }
    
    /// <summary>
    /// Represents several buttons on the mouse.
    /// </summary>
    public enum GM_MouseButton
    {
        Left,
        Middle,
        Right
    }
}
