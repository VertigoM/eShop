using eShop.API.Entities.Models;
using eShop.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly SecurityService _auth;

        public SecurityController(SecurityService auth)
        {
            _auth = auth;
        }

        [Authorize(Policy = "ModeratorPolicy")]
        [Route("roles/readall")]
        [HttpGet]
        public async Task<IActionResult> GetRolesAsync()
        {
            ResponseStatus response;
            try
            {
                var roles = await _auth.GetRolesAsync();
                return Ok(roles);
            }
            catch(Exception ex)
            {
                response = SetResponse(400, ex.Message, null, null);
                return BadRequest(response);
            }
        }

        //[Authorize(Policy = "ModeratorPoilicy")]
        [Route("users/readall")]
        [HttpGet]
        public async Task<IActionResult> GetUserAsync()
        {
            ResponseStatus response;
            try
            {
                var users = await _auth.GetUsersAsync();
                return Ok(users);
            } catch(Exception ex)
            {
                response = SetResponse(400, ex.Message, null, null);
                return BadRequest(response);
            }
        }

        /*
         * Post role
         */

        [Route("post/register/user")]
        [HttpPost]
        public async Task<IActionResult> RegisterUserAsync(RegisterUser user)
        {
            ResponseStatus response;
            try
            {
                var registrationResult = await _auth.RegisterUserAsync(user);
                if(!registrationResult)
                {
                    response = SetResponse(500, "User registration failed", null, null);
                    return StatusCode(500, response);
                }

                var activationResult = await _auth.AssignRoleToUserAsync(new UserRole
                {
                    Username = user.Email,
                    RoleName = "User"
                });

                if(!activationResult)
                {
                    response = SetResponse(500, "Activation Failed", null, null);
                    return StatusCode(500, response);
                }

                response = SetResponse(201, $"User {user.Email} was created successfully", null, null);
                return Ok(response);
            } catch(Exception ex)
            {
                response = SetResponse(400, ex.Message, null, null);
                return BadRequest(response);
            }
        }

        [Route("post/auth/user")]
        [HttpPost]
        public async Task<IActionResult> AuthUserAsync(LoginUser user)
        {
            ResponseStatus response;
            try
            {
                /*
                 * Login failed
                 */
                var result = await _auth.AuthUserAsync(user);
                if (result.LoginStatus == LoginStatus.LoginFailed)
                {
                    response = SetResponse(401, "Username or Password not found", null, null);
                    return Unauthorized(response);
                }
                
                /*
                 * Failed role check
                 */
                if (result.LoginStatus == LoginStatus.NoRoleToUser)
                {
                    response = SetResponse(401, "Missing role", null, null);
                    return Unauthorized(response);
                }

                /*
                 * Login successful
                 */
                if (result.LoginStatus == LoginStatus.LoginSuccessful)
                {
                    response = SetResponse(200, "Login successful", result.Token, result.Role);
                    response.Username = user.Username;
                    return Ok(response);
                }
                /*
                 * Internal server error
                 */
                else
                {
                    response = SetResponse(500, "Internal Server Error", null, null);
                    return StatusCode(500, response);
                }
            }
            catch(Exception ex)
            {
                response = SetResponse(400, ex.Message, null, null);
                return BadRequest(response);
            }
        }

        private ResponseStatus SetResponse(int code, string message, string? token, string? role)
        {
            ResponseStatus response = new ResponseStatus()
            {
                StatusCode = code,
                Message = message,
                Token = token ?? "",
                Role = role ?? "",
            };

            return response;
        }
    }
}
