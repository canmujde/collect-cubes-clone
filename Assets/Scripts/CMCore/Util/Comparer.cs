using System.Linq;
using UnityEngine;

namespace CMCore.Util
{
    public static class Comparer
    {
        public static bool IsAllTrue(this bool[] booleans) => booleans.All(t => t);


        public static bool SearchInside(this string[] source, string[] searchInside) =>
            searchInside.All(source.Contains);

        public static bool SearchInside(this float[] source, float[] searchInside) => searchInside.All(source.Contains);
        public static bool SearchInside(this int[] source, int[] searchInside) => searchInside.All(source.Contains);

        public static bool SearchInside(this Transform[] source, Transform[] searchInside) =>
            searchInside.All(source.Contains);

        public static bool SearchInside(this Vector3[] source, Vector3[] searchInside) =>
            searchInside.All(source.Contains);


        public static bool Compare(this bool[] a, bool[] b) =>
            (a != null && b != null) && (a.Length > 0 && b.Length > 0) && a.Length == b.Length &&
            a.OrderBy(c => c).SequenceEqual(b.OrderBy(c => c));

        public static bool Compare(this string[] a, string[] b) =>
            (a != null && b != null) && (a.Length > 0 && b.Length > 0) &&
            a.Length == b.Length &&
            a.OrderBy(c => c).SequenceEqual(b.OrderBy(c => c));

        public static bool Compare(this float[] a, float[] b) =>
            (a != null && b != null) && (a.Length > 0 && b.Length > 0) &&
            a.Length == b.Length && a.OrderBy(c => c)
                .SequenceEqual(b.OrderBy(c => c));

        public static bool Compare(this int[] a, int[] b) =>
            (a != null && b != null) && (a.Length > 0 && b.Length > 0) && a.Length == b.Length &&
            a.OrderBy(c => c).SequenceEqual(b.OrderBy(c => c));

        public static bool Compare(this double[] a, double[] b) =>
            (a != null && b != null) && (a.Length > 0 && b.Length > 0) &&
            a.Length == b.Length &&
            a.OrderBy(c => c).SequenceEqual(b.OrderBy(c => c));

        public static bool Compare(this char[] a, char[] b) =>
            (a != null && b != null) && (a.Length > 0 && b.Length > 0) && a.Length == b.Length &&
            a.OrderBy(c => c).SequenceEqual(b.OrderBy(c => c));

        public static bool Compare(this Vector3[] a, Vector3[] b) =>
            (a != null && b != null) && (a.Length > 0 && b.Length > 0) &&
            a.Length == b.Length &&
            a.OrderBy(c => c).SequenceEqual(b.OrderBy(c => c));

        public static bool Compare(this Transform[] a, Transform[] b) =>
            (a != null && b != null) && (a.Length > 0 && b.Length > 0) &&
            a.Length == b.Length &&
            a.OrderBy(c => c)
                .SequenceEqual(b.OrderBy(c => c));
    }
}