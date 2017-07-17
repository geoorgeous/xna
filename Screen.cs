using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNATemplate
{
    /// <summary>
    /// A static class that used to help manage the game window's size and also the main viewport's size. 
    /// This makes changing resolutions and screen sizes easier and allows you to set many different screen sizes while keeping the game's logic the exact same.
    /// This class stores the game's virtual dimensions and a matrix to transform any virtual coordinates into the main viewport's space.
    /// </summary>
    public static class Screen
    {
        #region private Fields

        // Main viewport that the game is drawn on
        private static Viewport viewport;
        // View matrix that transforms virtual coordinates into viewport coordinates
        private static Matrix viewMatrix = Matrix.Identity;

        // Screen dimensions of the inner window
        private static int windowWidth;
        private static int windowHeight;

        // The view dimensions are the actual dimensions of the viewport in pixel units
        private static int viewWidth;
        private static int viewHeight;

        // The virtual demensions are the dimensions that thingts in the game are drawn to
        private static int virtualViewWidth;
        private static int virtualViewHeight;

        #endregion



        #region Member Access Modifiers

        /// <summary> The width of the game window. </summary>
        public static int Width { get { return windowWidth; } }
        /// <summary> The height of the game window. </summary>
        public static int Height { get { return windowHeight; } }

        /// <summary> The width of the game. This is used in the games logic or drawing calculations. </summary>
        public static int GameWidth { get { return virtualViewWidth; } }
        /// <summary> The height of the game. This is used in the games logic or drawing calculations. </summary>
        public static int GameHeight { get { return virtualViewHeight; } }

        /// <summary> Matrix used for transforming the game's virtual coordinates into the Screen's viewport's coordinates. </summary>
        public static Matrix ViewMatrix { get { return viewMatrix; } }

        #endregion



        #region Initialization

        /// <summary>
        /// Call this to initialize the Screen and set the width and height of the window, viewport, and virtual game dimensions.
        /// </summary>
        /// <param name="deviceManager"> The graphics device manager which will have its buffer dimensions set to the dimensions specified. </param>
        /// <param name="device"> The graphics device to apply the new viewport to. </param>
        /// <param name="width"> The width to apply to the window, viewport, and game. </param>
        /// <param name="height"> The height to apply to the window, viewport, and game. </param>
        public static void InitializeScreen(GraphicsDeviceManager deviceManager, GraphicsDevice device, int width, int height)
        {
            // Set the window size
            SetSize(deviceManager, width, height);

            // Set the viewport and virtual viewport sizes
            viewWidth = virtualViewWidth = width;
            viewHeight = virtualViewHeight = height;

            // Create the viewport
            viewport = new Viewport(0, 0, width, height);

            // Apply the new viewport to the graphics device
            ApplyViewport(device);
        }

        #endregion



        #region Helper Methods

        /// <summary>
        /// Sets the dimensions of the game's window.
        /// </summary>
        /// <param name="deviceManager"> The device manager to apply changes to. </param>
        /// <param name="width"> The new width of the window. </param>
        /// <param name="height"> The new height of the window. </param>
        public static void SetSize(GraphicsDeviceManager deviceManager, int width, int height)
        {
            // Keep a local store of the screen dimensions
            windowWidth = width;
            windowHeight = height;

            // Set the graphics device buffer's width and height - this is what actually changes the window size
            deviceManager.PreferredBackBufferWidth = width;
            deviceManager.PreferredBackBufferHeight = height;

            deviceManager.ApplyChanges();
        }

        /// <summary>
        /// Sets the size of the game's *virtual* viewport. This is not the displayed size but the size that is used in all of the game's logic and drawing.
        /// </summary>
        /// <param name="width"> The width of the virtual viewport. </param>
        /// <param name="height"> The height of the virtual viewport. </param>
        public static void SetGameSize(int width, int height)
        {
            virtualViewWidth = width;
            virtualViewHeight = height;

            // Get the window aspect ratio and the game's aspect ratio.
            float screenAspectRatio = (float)windowWidth / (float)windowHeight;
            float gameAspectRatio = (float)width / (float)height;

            // If the game's aspect ratio is greater than the screen's then
            // we fit the viewport's width to the screens width and then scale the height
            // accordingly. Otherwise we do it the other way around.
            // We also need to center the viewport after we've aquired it's size,
            // Either in the X or the Y, depending on which dimension is fitted to the screen.
            if (gameAspectRatio > screenAspectRatio)
            {
                viewWidth = windowWidth;
                float widthScale = (float)viewWidth / (float)width;
                viewHeight = (int)(height * widthScale);
                viewport.Y = (windowHeight - viewHeight) / 2; 
            }
            else
            {
                viewHeight = windowHeight;
                float heightScale = (float)viewHeight / (float)height;
                viewWidth = (int)(width * heightScale);
                viewport.X = (windowWidth - viewWidth) / 2;
            }

            viewport.Width = viewWidth;
            viewport.Height = viewHeight;

            // We need to know the scale that's been applied to the width 
            // and height that were provided so we can define a view matrix 
            // for the spritebatch.
            float viewScale = (float)viewWidth / (float)virtualViewWidth;

            viewMatrix = Matrix.CreateScale(viewScale, viewScale, 1.0f);
        }

        /// <summary>
        /// This is used to apply the Screen classes viewport to a draphics device so that it is made *active*.
        /// </summary>
        /// <param name="device"> The graphics device the viewport should be given to. </param>
        public static void ApplyViewport(GraphicsDevice device)
        {
            device.Viewport = viewport;
        }

        #endregion
    }
}
