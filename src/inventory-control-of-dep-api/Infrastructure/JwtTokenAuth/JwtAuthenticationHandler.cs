using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

using inventory_control_of_dep_api.Infrastructure.Services.JwtTokenServices;

namespace inventory_control_of_dep_api.Infrastructure.JwtTokenAuth
{
    public class JwtAuthenticationHandler : AuthenticationHandler<JwtAuthenticationOptions>
    {
        private readonly IJwtTokenService _jwtTokenService;

        public JwtAuthenticationHandler(
            IOptionsMonitor<JwtAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IJwtTokenService jwtTokenService)
            : base(options, logger, encoder, clock)
        {
            _jwtTokenService = jwtTokenService ?? throw new ArgumentNullException(nameof(jwtTokenService));
        }

        /// <inheritdoc />
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("Unauthorized");
            }

            var token = Request.Headers["Authorization"].ToString()["Bearer ".Length..];

            var result = _jwtTokenService.ValidateToken(token);

            if (!result)
            {
                return AuthenticateResult.Fail(new Exception("Forbiden"));
            }

            try
            {
                var ticket = await Task.Run(() =>
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                    var identity = new ClaimsIdentity(jwtToken.Claims, Scheme.Name);
                    var principal = new ClaimsPrincipal(identity);
                    return new AuthenticationTicket(principal, Scheme.Name);
                });

                return AuthenticateResult.Success(ticket);
            }
            catch (HttpRequestException exp)
            {
                return AuthenticateResult.Fail(exp);
            }
        }
    }
}
