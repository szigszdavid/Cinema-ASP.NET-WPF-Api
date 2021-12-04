using Cinema.Persistence;
using Cinema.Persistence.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.WebApi.Controllers
{
    [Route("api/[controller]/[action]")] //Ez ki lett egészítve
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _sigInManager;

        //Kell egy végpont a be és kijelentkezéshez is

        public AccountController(SignInManager<ApplicationUser> signInManager)
        {
            _sigInManager = signInManager;
        }

        //api/Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto login) //A bejelntkezés endpointja
        {
            var result = await _sigInManager.PasswordSignInAsync(login.UserName, login.Password, false, false);

            if(result.Succeeded)
            {
                return Ok();
            }

            return Unauthorized("Login failed");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _sigInManager.SignOutAsync();

            return Ok();
        }
    }
}
