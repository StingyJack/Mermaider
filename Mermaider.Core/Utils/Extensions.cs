using System.Collections.Generic;

namespace Mermaider.UI
{
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
    }
}
