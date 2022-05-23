using inventory_control_of_dep_dal.Domain;

namespace inventory_control_of_dep_api.Infrastructure.Services.JwtTokenServices
{
    public interface IJwtTokenService
    {
        public string GetToken(User user, IList<string> roles);

        public bool ValidateToken(string token);
    }
}
