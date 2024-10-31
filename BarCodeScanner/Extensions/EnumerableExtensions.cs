using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarCodeScanner.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool Any(this IEnumerable enumerable)
        {
            return enumerable.GetEnumerator().MoveNext();
        }
    }
}
