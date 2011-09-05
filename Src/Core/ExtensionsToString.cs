using System.Text;

namespace Core
{
    public static class ExtensionsToString
    {
        public static string AsUtf8String(this byte[] args)
        {
            return Encoding.UTF8.GetString(args);
        }
    }
}
