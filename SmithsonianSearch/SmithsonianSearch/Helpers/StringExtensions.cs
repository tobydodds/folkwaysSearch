namespace SmithsonianSearch.Helpers
{
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// The string extensions.
    /// </summary>
    public static class StringExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        /// Strip HTML tags.
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string RemoveHtmlTags(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            string pattern = @"<(.|\n)*?>";
            return Regex.Replace(text, pattern, string.Empty);
        }

        /// <summary>
        /// Replace <, > chars with specified string (empty string by default)
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ReplaceTagBrackets(this string text, string replaceString = null) 
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            replaceString = replaceString ?? string.Empty;

            return text.Replace("<", replaceString).Replace(">", replaceString);
        }

        /// <summary>
        /// Truncate string to maxLength not breaking words and adding ... to the end.
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <param name="maxLength">
        /// The max length.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string Truncate(this string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text) || text.Trim().Length <= maxLength)
            {
                return text;
            }

            var sb = new StringBuilder(maxLength);

            string[] words = text.Split(new[] { ' ', '\t' });
            sb.Append(words[0]);

            for (int i = 1;
                 i < words.Length && sb.Length < maxLength && (words[i].Length + 1) <= (maxLength - sb.Length);
                 i++)
            {
                sb.Append(" " + words[i]);
            }

            sb.Append("...");

            return sb.ToString();
        }

        public static string RemoveAlertScript(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            string pattern = @"alert *\([^)]*\) *";
            return Regex.Replace(text, pattern, string.Empty, RegexOptions.IgnoreCase);
        }

        #endregion
    }
}