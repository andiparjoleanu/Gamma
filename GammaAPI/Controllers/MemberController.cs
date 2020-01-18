using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gamma.Models;
using Gamma.ViewModels;
using GammaAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GammaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly UserManager<Member> _userManager;
        private readonly SignInManager<Member> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public MemberController(UserManager<Member> userManager,
                                SignInManager<Member> signInManager,
                                RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet("logout")]
        public async Task<ResultVM> Logout()
        {
            await _signInManager.SignOutAsync();
            return new ResultVM
            {
                Status = Status.Success,
                Message = "Deconectare reușită"
            };
        }

        [HttpGet("currentClient")]
        public async Task<Member> GetCurrentClient()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            return user;
        }

        [HttpGet("currentClientInfo")]
        public async Task<RoleVM> GetCurrentClientInfo()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var roles = _roleManager.Roles.ToList();

            RoleVM userRole = new RoleVM
            {
                RoleName = "none"
            };

            foreach (var role in roles)
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRole.RoleName = role.Name;
                    break;
                }
            }

            return userRole;
        }

        [HttpPost("login")]
        public async Task<ResultVM> TryLogin(object routeValue)
        {
            if (ModelState.IsValid)
            {
                var model = JsonConvert.DeserializeObject<LoginVM>(routeValue.ToString());

                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    await _signInManager.SignInAsync(user, false);

                    return new ResultVM
                    {
                        Status = Status.Success,
                        Message = "Autentificare reușită"
                    };
                }

                return new ResultVM
                {
                    Status = Status.Error,
                    Message = "Utilizatorul nu a fost găsit"
                };
            }

            return new ResultVM
            {
                Status = Status.Error,
                Message = "Informații incorecte"
            };

        }

        [HttpPost("register")]
        public async Task<ResultVM> TryRegister(object userModel)
        {
            if (ModelState.IsValid)
            {
                var model = JsonConvert.DeserializeObject<RegisterVM>(userModel.ToString());

                IdentityResult result = null;
                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user != null)
                {
                    return new ResultVM
                    {
                        Status = Status.Error,
                        Message = "Utilizatorul există deja",
                    };
                }

                user = new Member
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DesiredRole = model.SelectedRole
                };

                result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var roles = _roleManager.Roles.ToList();
                    if(roles.Count == 0)
                    {
                        IdentityRole role = new IdentityRole
                        {
                            Name = "Administrator"
                        };

                        IdentityResult identityResult = await _roleManager.CreateAsync(role);

                        if(identityResult.Succeeded)
                        {
                            await _userManager.AddToRoleAsync(user, role.Name);
                        }

                    }

                    await _signInManager.SignInAsync(user, true);

                    return new ResultVM
                    {
                        Status = Status.Success,
                        Message = "Utilizator creat"
                    };
                }
                else
                {
                    var resultErrors = result.Errors.Select(e => "<li>" + e.Description + "</li>");
                    return new ResultVM
                    {
                        Status = Status.Error,
                        Message = "Informații incorecte: " + string.Join("", resultErrors)
                    };
                }
            }

            var errors = ModelState.Keys.Select(e => "<li>" + e + "</li>");
            return new ResultVM
            {
                Status = Status.Error,
                Message = "Informații incorecte: " + string.Join("", errors)
            };
        }
       
    }
}