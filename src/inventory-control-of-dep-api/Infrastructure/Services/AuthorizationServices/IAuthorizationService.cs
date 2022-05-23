using inventory_control_of_dep_api.Models.Authorization;
using inventory_control_of_dep_dal.Domain;
using Microsoft.AspNetCore.Identity;

namespace inventory_control_of_dep_api.Infrastructure.Services.AuthorizationService
{
    public interface IAuthorizationService
    {
        public Task<User> RegisterUser(RegistrationRequest request, IEnumerable<string> roles);

        public Task<SignInResult> LoginUser(LoginRequst request);

        public Task LogoutUser();
    }
}
