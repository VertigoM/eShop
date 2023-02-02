using eShop.API.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace eShop.API.Services
{
    /// <summary>
    /// The following class contains methods for:
    /// 1. Create a new role
    /// 2. Create a new user and assyng Role to User
    /// 3. Manage the User Login and generate Token
    /// </summary>
    public class SecurityService
    {
        private IConfiguration              _Configuration;
        private SignInManager<IdentityUser> _SignInManager;
        private UserManager<IdentityUser>   _UserManager;
        private RoleManager<IdentityRole>   _RoleManager;

        public SecurityService(
            IConfiguration configuration,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _Configuration = configuration;
            _SignInManager = signInManager;
            _UserManager = userManager;
            _RoleManager = roleManager;
        }

        public async Task<bool> CreateRoleAsync(IdentityRole role)
        {
            bool isRoleCreated = false;
            var result = await _RoleManager.CreateAsync(role);
            if(result.Succeeded)
            {
                isRoleCreated = true;
            }

            return isRoleCreated;
        }

        public async Task<List<ApplicationRole>> GetRolesAsync()
        {
            List<ApplicationRole> roles = new List<ApplicationRole>();
            roles = (from r in await _RoleManager.Roles.ToListAsync()
                     select new ApplicationRole()
                     {
                         Name = r.Name,
                         NormalizedName = r.NormalizedName,
                     }).ToList();
            return roles;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            List<User> users = new List<User>();
            users = (from u in await _UserManager.Users.ToListAsync()
                     select new User()
                     {
                         Email = u.Email,
                         Username = u.UserName,
                     }).ToList();
            return users;
        }

        public async Task<bool> RegisterUserAsync(RegisterUser register)
        {
            bool isCreated = false;
            var registerUser = new IdentityUser()
            {
                UserName = register.Email,
                Email = register.Email
            };

            var result = await _UserManager.CreateAsync(registerUser, register.Password);
            if (result.Succeeded)
            {
                isCreated = true;
            }
            return isCreated;
        }

        public async Task<bool> AssignRoleToUserAsync(UserRole user)
        {
            bool isRoleAssigned = false;
            var role = _RoleManager.FindByNameAsync(user.RoleName).Result;
            var registeredUser = await _UserManager.FindByNameAsync(user.Username);

            if(role != null)
            {
                var result = await _UserManager.AddToRoleAsync(registeredUser, role.Name);
                if (result.Succeeded)
                {
                    isRoleAssigned = true;
                }
            }
            return isRoleAssigned;
        }

        /*
         * Authenticate user based on username
         */
        public async Task<AuthStatus> AuthUserAsync(LoginUser inputModel)
        {
            string jwtToken = "";
            LoginStatus loginStatus;
            string roleName = "";

            var result = _SignInManager.PasswordSignInAsync(inputModel.Username,
                inputModel.Password, false, lockoutOnFailure: true).Result;

            /*
             * Check result status
             */
            if (result.Succeeded)
            {
                /*
                 * Read the secret key and the expiration from the configuration
                 */
                var secretKey = Convert.FromBase64String(
                    _Configuration["JWTCoreSettings:SecretKey"]);

                var expiryTimeSpan = Convert.ToInt32(
                    _Configuration["JWTCoreSettings:ExpiryTimeInMinutes"]);

                var user = await _UserManager.FindByEmailAsync(inputModel.Username);
                var role = await _UserManager.GetRolesAsync(user);

                /*
                 * If user is not associated with role log off
                 */
                if (role.Count == 0)
                {
                    await _SignInManager.SignOutAsync();
                    loginStatus = LoginStatus.NoRoleToUser;
                }
                else
                {
                    roleName = role[0];

                    /*
                     * Set expiry, subjects, etc.
                     */
                    var securityTokenDescriptor = new SecurityTokenDescriptor()
                    {
                        Issuer = null,
                        Audience = null,
                        Subject = new ClaimsIdentity(
                            new List<Claim>
                            {
                                new Claim("userid", user.Id.ToString()),
                                new Claim("role", role[0])
                            }),
                        Expires = DateTime.UtcNow.AddMinutes(expiryTimeSpan),
                        IssuedAt = DateTime.UtcNow,
                        NotBefore = DateTime.UtcNow,
                        SigningCredentials = new SigningCredentials(
                            new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
                    };

                    /*
                     * Generate token
                     */
                    var jwtHandler = new JwtSecurityTokenHandler();
                    var jwToken = jwtHandler.CreateJwtSecurityToken(securityTokenDescriptor);
                    jwtToken = jwtHandler.WriteToken(jwToken);
                    loginStatus = LoginStatus.LoginSuccessful;
                }
            }
            else
            {
                loginStatus = LoginStatus.LoginFailed;
            }

            var authResponse = new AuthStatus()
            {
                LoginStatus = loginStatus,
                Token = jwtToken,
                Role = roleName
            };

            return authResponse;
        }

        /*
         * Accept token as parameter and receive token from it
         */
        public async Task<string> GetUserFromTokenAsync(string token)
        {
            string userName = string.Empty;
            var jwtHandler = new JwtSecurityTokenHandler();

            /*
             * Read token values
             */
            var jwtSecurityToken = jwtHandler.ReadJwtToken(token);

            /*
             * Read claims
             */
            var claims = jwtSecurityToken.Claims;

            /*
             * Read first claim
             */
            var userIdClaim = claims.First();

            /*
             * Read the user id
             */
            var userId = userIdClaim.Value;

            /*
             * Get the username from the userid
             */
            var identityUser = await _UserManager.FindByIdAsync(userId);
            userName = identityUser.UserName;
            return userName;
        }

        public string GetRoleFromToken(string token)
        {
            string roleName = string.Empty;
            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = jwtHandler.ReadJwtToken(token);
            var claims = jwtSecurityToken.Claims;
            var roleClaim = claims.Take(2);
            var roleRecord = roleClaim.Last();
            roleName = roleRecord.Value;
            return roleName;
        }
    }
}
