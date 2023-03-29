namespace MvcMarkdownPost.Helpers
{
    public static class StringHelper
    {
        public static string TruncateLongString(this string str, int maxLength)
        {
            return str.Length <= maxLength ? str : str.Remove(maxLength);
        }
    }
}
