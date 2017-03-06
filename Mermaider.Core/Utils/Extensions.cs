namespace Mermaider.Core.Utils
{
    using System.Collections.Generic;

    public static class Extensions
    {
        /// <summary>
        ///     Shorthand string.Join(separator, list)
        /// </summary>
        /// <param name="list"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string ToSingle(this IEnumerable<string> list, string separator = "\n")
        {
            return string.Join(separator, list);
        }

        /// <summary>
        ///     True if only one of the two are empty
        /// </summary>
        /// <param name="firstValue"></param>
        /// <param name="secondValue"></param>
        /// <returns></returns>
        public static bool OnlyOneEmpty(this string firstValue, string secondValue)
        {
            var firstIsEmpty= string.IsNullOrWhiteSpace(firstValue);
            var secondIsEmpty = string.IsNullOrWhiteSpace(secondValue);

            if (firstIsEmpty && secondIsEmpty)
            {
                return false;
            }

            return firstIsEmpty != false || secondIsEmpty != false;
        }

    }
}
