using UnityEngine;

namespace Jam.Core
{

    public static partial class Utils
    {
        public static class Color
        {
            public static UnityEngine.Color FromHex(string hex)
            {
                // 移除#字符，如果有的话
                hex = hex.TrimStart('#');

                // 解析16进制字符串，确保它是6个字符长
                if (hex.Length < 6)
                {
                    Debug.LogError("Hex color must be 6 characters long.");
                    return UnityEngine.Color.black;
                }

                // 解析每个颜色分量
                byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                byte a = 255;
                if (hex.Length == 8)
                {
                    a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
                }

                // 转换到0-1范围
                return new UnityEngine.Color(r / 255f, g / 255f, b / 255f, a / 255f);
            }
        }
    }

}