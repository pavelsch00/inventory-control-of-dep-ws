using Microsoft.AspNetCore.Identity;

using AutoMapper;

using inventory_control_of_dep_api.Infrastructure.Services.AuthorizationService;
using inventory_control_of_dep_api.Models.Authorization;
using inventory_control_of_dep_dal.Domain;

namespace inventory_control_of_dep_api.Infrastructure.Services.AuthorizationServices
{
    internal class AuthorizationService : IAuthorizationService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;

        public AuthorizationService(UserManager<User> userManager, 
            SignInManager<User> signInManager, IMapper mapper)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<User> RegisterUser(RegistrationRequest request, IEnumerable<string> roles)
        {
            var user = new User
            {
                Email = request.Email,
                UserName = request.Email,
                Name = request.Firstname,
                LastName = request.LastName,
                Surname = request.Surname,
                PositionId = request.PositionId,
                DepartmentId = request.DepartmentId,
                FacultyId = request.FacultyId,
                IsActive = true
            };

            await _userManager.CreateAsync(user, request.Password);
            await _userManager.AddToRolesAsync(user, roles);
            await _signInManager.SignInAsync(user, false);

            return user;
        }

        public async Task<SignInResult> LoginUser(LoginRequst request)
        {
            return await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, false);
        }

        public async Task LogoutUser()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
