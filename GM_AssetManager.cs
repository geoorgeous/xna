using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace XNATemplate
{
    /// <summary>
    /// A very simple asset manager class to make asset organisation a little simpler.
    /// </summary>
    public static class GM_AssetManager
    {
        #region Fields

        // Store three dictionaries of assets
        private static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        private static Dictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont>();
        private static Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();

        #endregion



        #region Texture Methods

        /// <summary>
        /// Adds a new texture to the Asset Managers collection only if there is not already an existing texture of the same name.
        /// </summary>
        /// <param name="name"> The name to give the new texture. </param>
        /// <param name="texture"> The texture object to add. </param>
        public static void NewTexture(string name, Texture2D texture)
        {
            if (!textures.ContainsKey(name))
            {
                textures.Add(name, texture);
            }
        }

        /// <summary>
        /// Gets a texture by name. If no texture is found with the specified name then the texture at [0] will be given.
        /// </summary>
        /// <param name="name"> The name of the texture to look for. </param>
        /// <returns> Returns a texture pulled from Asset Manager's collection. </returns>
        public static Texture2D GetTexture(string name)
        {
            if (textures.ContainsKey(name))
            {
                return textures[name];
            }

            return textures.Values.ElementAt(0);
        }

        #endregion



        #region Font Methods

        /// <summary>
        /// Adds a new font to the Asset Managers collection only if there is not already an existing font of the same name.
        /// </summary>
        /// <param name="name"> The name to give the new font. </param>
        /// <param name="font"> The font object to add. </param>
        public static void NewFont(string name, SpriteFont font)
        {
            if (!fonts.ContainsKey(name))
            {
                fonts.Add(name, font);
            }
        }

        /// <summary>
        /// Gets a font by name. If no font is found with the specified name then the font at [0] will be given.
        /// </summary>
        /// <param name="name"> The name of the font to look for. </param>
        /// <returns> Returns a font pulled from Asset Manager's collection. </returns>
        public static SpriteFont GetFont(string name)
        {
            if (fonts.ContainsKey(name))
            {
                return fonts[name];
            }

            return fonts.Values.ElementAt(0);
        }

        #endregion



        #region Sound Methods

        /// <summary>
        /// Adds a new sound to the Asset Managers collection only if there is not already an existing sound of the same name.
        /// </summary>
        /// <param name="name"> The name to give the new sound. </param>
        /// <param name="sound"> The sound object to add. </param>
        public static void NewSound(string name, SoundEffect sound)
        {
            if (!sounds.ContainsKey(name))
            {
                sounds.Add(name, sound);
            }
        }

        /// <summary>
        /// Gets a sound by name. If no sound is found with the specified name then the sound at [0] will be given.
        /// </summary>
        /// <param name="name"> The name of the sound to look for. </param>
        /// <returns> Returns a sound pulled from Asset Manager's collection. </returns>
        public static SoundEffect GetSound(string name)
        {
            if (sounds.ContainsKey(name))
            {
                return sounds[name];
            }

            return sounds.Values.ElementAt(0);
        }

        #endregion
        
    }
}
