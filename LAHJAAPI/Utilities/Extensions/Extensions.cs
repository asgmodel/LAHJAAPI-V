namespace TEP.Helper.Extensions
{
    public static class UserClaims
    {

        public static string Fillter(this string s) { return s.Split(",")[0]; }

        public static void ForEach(this IEnumerable<object> t, Action<object> acc)
        {
            foreach (object obj in t)
                acc(obj);
        }

        public static async Task ForEachAsync<T>(this List<T> list, Func<T, Task> func)
        {
            foreach (var value in list)
            {
                await func(value);
            }
        }

        public static int ToInt32(this string s) { return Convert.ToInt32(s); }
        public static long ToInt64(this string? s)
        {
            return Int64.TryParse(s, out var val) ? val : 0;
        }
    }
}
