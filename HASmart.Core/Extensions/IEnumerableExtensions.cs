using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HASmart.Core.Extensions {
    public static class IEnumerableExtensions {
        public static string ToCsv<T>(this IEnumerable<T> collection, int startingIndex = 0, string separator = ", ", string nullString = "") {
            StringBuilder sb = new StringBuilder();
            foreach (T element in collection.Skip(startingIndex)) {
                sb.Append(element);
                sb.Append(separator);
            }

            if (sb.Length == 0) {
                return nullString;
            }

            sb.Remove(sb.Length - separator.Length, separator.Length);
            return sb.ToString();
        }
    }
}
