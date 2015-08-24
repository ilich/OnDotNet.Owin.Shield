namespace OnDotNet.Owin.Shield.Frameguard
{
    public enum XFrameOptions
    {
        Deny,
        Sameorigin,
        AllowFrom
    }

    class XFrameOptionsConstants
    {
        public static readonly string Header = "X-Frame-Options";

        public static readonly string Deny = "DENY";

        public static readonly string Sameorigin = "SAMEORIGIN";

        public static readonly string AllowFrom = "ALLOW-FROM {0}";
    }
}
