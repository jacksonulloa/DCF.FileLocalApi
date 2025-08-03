namespace DCF.FileStream.Entities.Generic
{
    public class AppSettings
    {
        public GlobalConfig GlobalConfig { get; set; } = new();
        public Jwt Jwt { get; set; } = new();
        public FileConfig FileConfig { get; set; } = new();
        public List<CredentialConfig> CredentialsConfig { get; set; } = [];
    }
    public class FileConfig
    {
        public string RootPathLocal { get; set; } = string.Empty;
    }
    public class GlobalConfig
    {
        public string SystemName { get; set; } = string.Empty;
        public string PathLog { get; set; } = string.Empty;
        public string ShowOpenApi { get; set; } = string.Empty;
        public string UseHttps { get; set; } = string.Empty;
        //public string LocalProfile { get; set; } = string.Empty;
        //public string LocalPass { get; set; } = string.Empty;
    }
    public class Jwt
    {
        public string SecretKey { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string MinutesFactor { get; set; } = string.Empty;
        public string HoursFactor { get; set; } = string.Empty;
        public string DaysFactor { get; set; } = string.Empty;

    }
    public class CredentialConfig
    {
        public string Role { get; set; } = string.Empty;
        public string Identifier { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
    }
}
