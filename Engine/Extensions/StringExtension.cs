namespace Engine.Extensions
{
    using System.Linq;
    using System.Text.RegularExpressions;

    public static class StringExtension
    {
        public static byte[][] StringToByteMatrix(this string str, int size)
        {
            return new Regex("\\D+").Replace(str, string.Empty).ToCharArray()
                .Select(x => x == ' ' ? (byte) 0 : byte.Parse(x.ToString())).ToArray().Split(size).Reverse().ToArray();
        }
    }
}