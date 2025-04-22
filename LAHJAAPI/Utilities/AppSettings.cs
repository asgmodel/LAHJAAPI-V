namespace LAHJAAPI.Utilities
{
    public class AppSettings
    {
        public string AesSecret { get; set; }
        public string Client { get; set; }
        public string Api { get; set; }
        public string Auth { get; set; }

        public string[] PermittedExtensions { get; set; }
        public long FileSizeLimit { get; set; } = 2097152;
        public Jwt Jwt { get; set; }
    }

    public class Jwt
    {
        public string Secret { get; set; } = "secret";
        public static string SessionToken => "JWTToken";
        public string WebSecret { get; set; }
        public string validIssuer { get; set; }
        public string ValidAudience { get; set; }
    }
}
