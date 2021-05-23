using System;

namespace MultiChat.Shared.Helpers
{
    public static class ColorHelper
    {
        private static Random _random = new();

        public static ColorEnum RandomColor()
        {
            int colorsCount = Enum.GetNames(typeof(ColorEnum)).Length;
            int index = _random.Next(0, colorsCount);
            return (ColorEnum)index;
        }

        public static string GetColor(ColorEnum color)
        {
            return "#"+color.ToString().Substring(1);
        }
    }
}
