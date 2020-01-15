using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gamma;
using Gamma.Models;
using GammaAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GammaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministrationController : ControllerBase
    {
        private readonly UserManager<Member> _userManager;
        private readonly SignInManager<Member> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public AdministrationController(UserManager<Member> userManager,
                                SignInManager<Member> signInManager,
                                RoleManager<IdentityRole> roleManager,
                                IRepositoryWrapper repositoryWrapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _repositoryWrapper = repositoryWrapper;
        }

        [HttpPost("createRole")]
        public async Task<ResultVM> CreateRole(object userModel)
        {
            if (ModelState.IsValid)
            {
                var model = JsonConvert.DeserializeObject<RoleVM>(userModel.ToString());

                IdentityRole role = new IdentityRole
                {
                    Name = model.RoleName
                };

                IdentityResult identityResult = await _roleManager.CreateAsync(role);

                if (identityResult.Succeeded)
                {
                    return new ResultVM
                    {
                        Status = Status.Success,
                        Message = "Rolul a fost creat"
                    };
                }

                return new ResultVM
                {
                    Status = Status.Error,
                    Message = "Rolul nu a putut fi creat"
                };

            }

            var errors = ModelState.Keys.Select(e => "<li>" + e + "</li>");
            return new ResultVM
            {
                Status = Status.Error,
                Message = "Informații incorecte: " + string.Join("", errors)
            };

        }

        [HttpGet("getRolesList")]
        public List<IdentityRole> GetRolesList()
        {
            return _roleManager.Roles.ToList();
        }

        [HttpGet("getAllRoles")]
        public async Task<List<EditRoleVM>> GetAllRoles()
        {
            var users = _userManager.Users.ToList();
            var roles = _roleManager.Roles.ToList();

            List<EditRoleVM> result = new List<EditRoleVM>();

            if (roles.Any())
            {
                foreach (var role in roles)
                {
                    EditRoleVM editRole = new EditRoleVM
                    {
                        RoleId = role.Id,
                        RoleName = role.Name,
                        Users = new List<string>()
                    };

                    if (users.Any())
                    {
                        foreach (var user in users)
                        {
                            if (await _userManager.IsInRoleAsync(user, role.Name))
                            {
                                editRole.Users.Add(user.FirstName + " " + user.LastName);
                            }
                        }
                    }

                    result.Add(editRole);
                }
            }

            return result;
        }

        [HttpGet("getUsersWithoutRole/{roleid}")]
        public async Task<List<AddUserRoleVM>> GetUsersWithoutRole(string roleid)
        {
            List<AddUserRoleVM> deleteAccountVMs = new List<AddUserRoleVM>();

            var users = _userManager.Users.ToList();
            var roles = _roleManager.Roles.ToList();

            var selectedRole = await _roleManager.FindByIdAsync(roleid);

            foreach (var user in users)
            {
                bool hasRole = false;

                foreach (var role in roles)
                {
                    if (await _userManager.IsInRoleAsync(user, role.Name))
                    {
                        hasRole = true;
                        break;
                    }
                }

                if (!hasRole && user.DesiredRole == selectedRole.Name)
                {
                    var account = new AddUserRoleVM
                    {
                        FullName = user.FirstName + " " + user.LastName,
                        Name = user.UserName,
                        IsSelected = false
                    };

                    deleteAccountVMs.Add(account);
                }
            }

            return deleteAccountVMs;
        }

        [HttpPost("saveUserRoles")]
        public async Task<ResultVM> SaveUserRoles(object routeValues)
        {
            if (ModelState.IsValid)
            {
                var model = JsonConvert.DeserializeObject<UpdateUserRolesVM>(routeValues.ToString());

                var role = await _roleManager.FindByIdAsync(model.RoleId);

                IdentityResult result = new IdentityResult();

                foreach (var account in model.UserRoles)
                {
                    if (account.IsSelected)
                    {
                        var user = await _userManager.FindByNameAsync(account.Name);

                        result = await _userManager.AddToRoleAsync(user, role.Name);

                        switch(role.Name)
                        {
                            case "Profesor":
                                {
                                    Teacher teacher = new Teacher
                                    {
                                        MemberId = user.Id,
                                    };
                                    await _repositoryWrapper.TeacherRepository.CreateAsync(teacher);
                                }; break;
                            case "Student":
                                {
                                    Student student = new Student
                                    {
                                        MemberId = user.Id,
                                    };
                                    await _repositoryWrapper.StudentRepository.CreateAsync(student);
                                }; break;
                        }
                    }
                }

                if (result.Succeeded)
                {
                    return new ResultVM
                    {
                        Status = Status.Success,
                        Message = "Actualizare reușită"
                    };
                }

                return new ResultVM
                {
                    Status = Status.Error,
                    Message = "Eroare la actualizare"
                };
            }

            return new ResultVM
            {
                Status = Status.Error,
                Message = "Date Invalide"
            };
        }

        [HttpGet("removeRole/{roleid}")]
        public async Task<ResultVM> RemoveRole(string roleid)
        {
            var role = await _roleManager.FindByIdAsync(roleid);
            var users = _userManager.Users.ToList();
            
            foreach(var user in users)
            {
                if(await _userManager.IsInRoleAsync(user, role.Name))
                {
                    await _userManager.DeleteAsync(user);
                }
            }

            await _roleManager.DeleteAsync(role);

            return new ResultVM
            {
                Status = Status.Success,
                Message = "Operație reușită"
            };
        }
    }
}