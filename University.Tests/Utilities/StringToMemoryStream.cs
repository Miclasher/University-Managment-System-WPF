using System.Text;

namespace University.Tests.Utilities
{
    internal static class StringToMemoryStream
    {
        internal static MemoryStream Convert(string str)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(str ?? ""));
        }
    }
}