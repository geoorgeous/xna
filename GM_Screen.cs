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
    public static class GM_Screen
    {
        #region private Fields

        // Default dimensions
        private const int defaultWidth = 800;
        private const int defaultHeight = 500;

        // Graphics Device Manager to apply changes to.
        private static GraphicsDeviceManager graphics;

        // Main viewport that the game is drawn on.
        private static Viewport viewport = new Viewport(0, 0, defaultWidth, defaultHeight);
        // View matrix that transforms virtual coordinates into viewport coordinates.
        private static Matrix viewMatrix = Matrix.Identity;

        // Screen dimensions of the inner window.
        private static int windowWidth = defaultWidth;
        private static int windowHeight = defaultHeight;

        // The view dimensions are the actual dimensions of the viewport in pixel units.
        private static int viewWidth = defaultWidth;
        private static int viewHeight = defaultHeight;

        // The virtual demensions are the dimensions that thingts in the game are drawn to.
        private static int virtualViewWidth = defaultWidth;
        private static int virtualViewHeight = defaultHeight;

        #endregion



        #region Member Access Modifiers

        /// <summary> Matrix used for transforming the game's virtual coordinates into the Screen's viewport's coordinates. </summary>
        public static Matrix ViewMatrix { get { return viewMatrix; } }

        /// <summary> The width of the game window. </summary>
        public static int Width { get { return windowWidth; } }
        /// <summary> The height of the game window. </summary>
        public static int Height { get { return windowHeight; } }

        /// <summary> The width of the game. This is used in the games logic or drawing calculations. </summary>
        public static int GameWidth { get { return virtualViewWidth; } }
        /// <summary> The height of the game. This is used in the games logic or drawing calculations. </summary>
        public static int GameHeight { get { return virtualViewHeight; } }

        /// <summary> Get's and sets whether or not the screen is in full screen mode. </summary>
        public static bool IsFullScreen { get { return graphics.IsFullScreen; } set { if (graphics.IsFullScreen != value) ToggleFullScreen(); } }

        #endregion



        #region Initialization

        /// <summary>
        /// Call this to initialize the Screen, viewport, and game to the default size (800, 500).
        /// </summary>
        /// <param name="graphics"> The graphics device manager which will have its buffer dimensions set to the defaults. </param>
        public static void InitializeScreen(GraphicsDeviceManager graphics)
        {
            GM_Screen.graphics = graphics;

            // Set the graphics buffer sizes and apply changes
            graphics.PreferredBackBufferWidth = defaultWidth;
            graphics.PreferredBackBufferHeight = defaultHeight;
            graphics.ApplyChanges();

            // Apply the new viewport to the graphics device
            ApplyViewport();
        }

        /// <summary>
        /// Call this to initialize the Screen and set the width and height of the window, viewport, and virtual game dimensions.
        /// </summary>
        /// <param name="graphics"> The graphics device manager which will have its buffer dimensions set to the dimensions specified. </param>
        /// <param name="width"> The width to apply to the window, viewport, and game. </param>
        /// <param name="height"> The height to apply to the window, viewport, and game. </param>
        public static void InitializeScreen(GraphicsDeviceManager graphics, int width, int height)
        {
            GM_Screen.graphics = graphics;

            // Set the viewport and virtual viewport, as well as the window and graphics buffer sizes
            viewWidth = virtualViewWidth = windowWidth = graphics.PreferredBackBufferWidth = width;
            viewHeight = virtualViewHeight = windowHeight = graphics.PreferredBackBufferHeight = height;
            graphics.ApplyChanges();

            // Create the viewport
            viewport = new Viewport(0, 0, viewWidth, viewHeight);

            // Apply the new viewport to the graphics device
            ApplyViewport();
        }

        /// <summary>
        /// Call this to initialize the Screen and set the dimensions of the screen and game individually.
        /// </summary>
        /// <param name="graphics"> The graphics device manager which will have its buffer dimensions set to the dimensions specified. </param>
        /// <param name="screenWidth"> The width to apply to the window. </param>
        /// <param name="screenHeight"> The height to apply to the window. </param>
        /// <param name="gameWidth"> The width to apply to the window. </param>
        /// <param name="gameHeight"> The height to apply to game's virtual viewport. </param>
        public static void InitializeScreen(GraphicsDeviceManager graphics, int screenWidth, int screenHeight, int gameWidth, int gameHeight)
        {
            GM_Screen.graphics = graphics;

            // Set the virtual dimenions before setting the size as the SetSize() function calls RefreshViewport() which needs to pass these.
            virtualViewWidth = gameWidth;
            virtualViewHeight = gameHeight;

            SetSize(screenWidth, screenHeight);

            // Apply the new viewport to the graphics device
            ApplyViewport();
        }

        #endregion



        #region Helper Methods

        /// <summary>
        /// Sets the dimensions of the game's window.
        /// </summary>
        /// <param name="deviceManager"> The device manager to apply changes to. </param>
        /// <param name="width"> The new width of the window. </param>
        /// <param name="height"> The new height of the window. </param>
        public static void SetSize(int width, int height)
        {
            // Keep a local store of the screen dimensions
            windowWidth = width;
            windowHeight = height;

            // Set the graphics device buffer's width and height - this is what actually changes the window size
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;

            // We need to make sure to call graphics.ApplyCHanges() here otherwise the backbuffer 
            // won't actually change size and we may gett errors regarding a mismatched viewport 
            // trying to fit inside a small render target.
            graphics.ApplyChanges();

            // Refresh the viewport so it fits the new size and is poitioned correctly.
            RefreshViewport();
        }

        /// <summary>
        /// Sets the size of the game's *virtual* viewport. This is not the displayed size but the size that is used in all of the game's logic and drawing.
        /// </summary>
        /// <param name="width"> The width of the virtual viewport. </param>
        /// <param name="height"> The height of the virtual viewport. </param>
        public static void SetGameSize(int width, int height)
        {
            // Reset the viewport
            viewport = new Viewport();

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

            ApplyViewport();
        }

        private static void RefreshViewport()
        {
            // All we're doing here is calling SetGameSize and passing the current virtual viewport dimensions.
            // This will just force the viewport to fit the screen.
            SetGameSize(virtualViewWidth, virtualViewHeight);
        }

        /// <summary>
        /// Re-applys the viewport to the graphics device.
        /// This is usually called whenthe viewport has been changed or the GraphicsDevice has a different viewport.
        /// </summary>
        public static void ApplyViewport()
        {
            graphics.GraphicsDevice.Viewport = viewport;
        }

        /// <summary>
        /// Toggles fullscren mode 
        /// </summary>
        public static void ToggleFullScreen()
        {
            // Toggle the device manager's full screen flag.
            graphics.IsFullScreen = !graphics.IsFullScreen;

            if (graphics.IsFullScreen)
            {
                // If we're going into full screen mode then we first capture the current
                // window dimensions so we can remember them for when we come out of full screen mode.
                int originalWindowWidth = windowWidth;
                int originalWindowHeight = windowHeight;

                // Here we just set the screen size to the graphics adpaters size (the monitors size) so that when we go in to full screen mode the resolution isn't stretched.
                SetSize(graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width, graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height);
                // We set the game size to what it already is here so that the viewport is forced to match the new screen size again.
                SetGameSize(virtualViewWidth, virtualViewHeight);

                // Now reset the kept window dimensions to the old ones so when we come out of full screen mode we know what to shrink to.
                windowWidth = originalWindowWidth;
                windowHeight = originalWindowHeight;
            }
            else
            {
                // If we're coming out of full screen mode then we don't do anything special, just reset the screen size and game size.
                SetSize(windowWidth, windowHeight);
                SetGameSize(virtualViewWidth, virtualViewHeight);
            }

            // Apply the viewport.
            ApplyViewport();
        }

        /// <summary>
        /// Transforms a given 2D point in local window coordionates to the Screen's virtual viewport's coordinates.
        /// This point can then be used in the game's logic for example transforming a cursors position to its position in the game's viewport.
        /// </summary>
        /// <param name="p"> The point - in screen coordinates - to be transformed to virtual viewport coordinates. </param>
        /// <returns> Returns the transformed point. </returns>
        public static Point GetTransformedPoint(Point p)
        {
            // Get the scale between the game's *virtual* viewport and the Screen actual viewport sizes.
            float viewScale = (float)virtualViewWidth / (float)viewWidth;

            // First we need to translate the point to originate at the viewport's origin.
            p.X -= viewport.X;
            p.Y -= viewport.Y;

            // Finally we scale the point so that we can find its location in virtual space.
            p.X = (int)(p.X * viewScale);
            p.Y = (int)(p.Y * viewScale);

            return p;
        }

        #endregion
    }
}
