using UnityEngine;

namespace Jam.Core
{

    public static class ColorEx
    {
        public static Color FromByteValues(byte r, byte g, byte b, byte a = 255)
        {
            float inv255 = 1.0f / 255.0f;
            return new Color(r * inv255, g * inv255, b * inv255, a * inv255);
        }

        /// The "hexString" used in "ColorUtility.TryParseHtmlString" need start with '#', like #FFFFFF
        public static Color FromHex(string hexString)
        {
            Color color = Color.white;
            ColorUtility.TryParseHtmlString(hexString, out color);
            return color;
        }

        /// The "hexString" used in "ColorUtility.TryParseHtmlString" need start with '#', like #FFFFFF
        public static Color FromHex(string hexString, float alpha)
        {
            Color color = Color.white;
            ColorUtility.TryParseHtmlString(hexString, out color);
            color.a = alpha;
            return color;
        }

        public static Color[] GetFilledColorArray(int arrayLength, Color fillValue)
        {
            Color[] colorArray = new Color[arrayLength];
            for (int colorIndex = 0; colorIndex < arrayLength; ++colorIndex)
            {
                colorArray[colorIndex] = fillValue;
            }

            return colorArray;
        }

        public static Color KeepAllButAlpha(this Color color, float newAlpha)
        {
            return new Color(color.r, color.g, color.b, newAlpha);
        }

        public static void SetAlpha(this ref Color color, float newAlpha)
        {
            color.a = newAlpha;
        }

        public static string GetHexFromRGBA(Color color)
        {
            // The return of "ColorUtility.ToHtmlStringRGBA" doesn't contain '#'
            return $"#{ColorUtility.ToHtmlStringRGBA(color)}";
        }
    }

}