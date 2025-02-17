using Core.DTOs.AccountDtos;
using Core.Entities;
using Core.Enums;
using Core.Services.Contracts;
using Core.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IJwtServices _jwtServices;

        public AccountsController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            IJwtServices jwtServices)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtServices = jwtServices;
        }

        /// <summary>
        /// Register an account
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("register")]
        [HttpPost]
        [Authorize("NoAuthenticated")]
        public async Task<IActionResult> RegisterAsync(RegisterRequestDto request)
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                PersonName = request.PersonName,
                Email = request.Email,
                Address = request.Address,
                Gender = request.Gender,
                PostalCode = request.PostalCode
            };

            IdentityResult result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                if (request.AppRole == AppRoles.User)
                {
                    if (await _roleManager.FindByNameAsync(AppRoles.User.ToString()) != null)
                    {
                        await _userManager.AddToRoleAsync(user, AppRoles.User.ToString());
                    }
                    else
                    {
                        ApplicationRole userRole = new() { Name = AppRoles.User.ToString() };
                        await _roleManager.CreateAsync(userRole);
                        await _userManager.AddToRoleAsync(user, AppRoles.User.ToString());
                    }
                }
                else
                {
                    if (await _roleManager.FindByNameAsync(AppRoles.Admin.ToString()) != null)
                    {
                        await _userManager.AddToRoleAsync(user, AppRoles.Admin.ToString());
                    }
                    else
                    {
                        ApplicationRole userRole = new() { Name = AppRoles.Admin.ToString() };
                        await _roleManager.CreateAsync(userRole);
                        await _userManager.AddToRoleAsync(user, AppRoles.User.ToString());
                    }
                }
            }
            else
            {
                string errors = string.Join(" ,\n", result.Errors.Select(e => e.Description));
                return Problem(errors, statusCode: 400);
            }

            // var token = await _jwtServices.CreateJwtToken(user);

            return Ok();
        }

        [Route("login")]
        [HttpPost]
        [Authorize("NoAuthenticated")]
        public async Task<IActionResult> LoginAsync(LoginRequestDto request)
        {
            var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, false);

            if (result.Succeeded)
            {
                ApplicationUser? user = await _userManager.FindByNameAsync(request.Email);

                if (user == null)
                {
                    return Problem("No user found with these credentials.", statusCode: 400);
                }

                return Ok(await _jwtServices.CreateJwtToken(user));
            }
            else
            {
                return Problem("Login failed, Try again with the correct login credentials", statusCode: 400);
            }
        }
    }
}
