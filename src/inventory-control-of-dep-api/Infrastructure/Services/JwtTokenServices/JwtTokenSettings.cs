namespace inventory_control_of_dep_api.Infrastructure.Services.JwtTokenServices
{
    public class JwtTokenSettings
    {
        public string JwtIssuer { get; set; }

        public string JwtAudience { get; set; }

        public string JwtSecretKey { get; set; }
    }
}
