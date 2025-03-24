namespace University.Tests.Utilities
{
    internal static class NormalizeLineEndings
    {
        public static string Normalize(string input)
        {
            return input.Replace("\r\n", "\n").Replace("\r", "\n");
        }
    }
}
