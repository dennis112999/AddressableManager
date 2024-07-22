using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace ResourceManager
{
    public enum SpriteCategory
    {
        /// <summary>
        /// 
        /// </summary>
        UI,

        /// <summary>
        /// 
        /// </summary>
        Test,
    }

    internal static class SpriteUtil
    {
        private static readonly StringBuilder s_addressBuilder = new StringBuilder();

        public static void LoadSprite(this Image image, SpriteCategory category, string key)
        {
            string address = GetAddress(category, key);
            image.LoadSprite(address);
        }

        public static void LoadSprite(this SpriteRenderer spriteRenderer, SpriteCategory category, string key)
        {
            string address = GetAddress(category, key);
            spriteRenderer.LoadSprite(address);
        }

        public static string GetAddress(SpriteCategory category, string key)
        {
            string texture;
            switch (category)
            {
                case SpriteCategory.UI:
                    texture = $"Texture/UI/1.png";
                    break;
                case SpriteCategory.Test:
                    texture = $"Icons/bsSkill/{key}.png";
                    break;
                default:
                    throw new InvalidOperationException();
            }

            return GetAddress(texture, key);
        }

        public static string GetAddress(string textureAddress, string spriteName = null)
        {
            Debug.Assert(!string.IsNullOrEmpty(textureAddress));

            s_addressBuilder.Append(textureAddress);

            if (!string.IsNullOrEmpty(spriteName))
            {
                s_addressBuilder.Append('[');
                s_addressBuilder.Append(spriteName);
                s_addressBuilder.Append(']');
            }

            string address = s_addressBuilder.ToString();
            s_addressBuilder.Clear();

            return address;
        }
    }
}
