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
    public class PrincipalController : ControllerBase
    {
        private readonly UserManager<Member> _userManager;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public PrincipalController(IRepositoryWrapper repositoryWrapper,
                                   UserManager<Member> userManager)
        {
            _repositoryWrapper = repositoryWrapper;
            _userManager = userManager;
        }

        [HttpGet("getAllSchools")]
        public async Task<List<SchoolVM>> GetAllSchools()
        {
            List<School> schools = await _repositoryWrapper.SchoolRepository.GetAllAsync();

            List<SchoolVM> schoolVMs = new List<SchoolVM>();
            foreach (var school in schools)
            {
                schoolVMs.Add(new SchoolVM
                {
                    Address = school.Address,
                    Id = school.Id,
                    Mail = school.Mail,
                    Name = school.Name,
                    Phone = school.Phone,
                    Type = school.Type
                });
            }

            return schoolVMs;
        }

        [HttpPost("addSchool")]
        public async Task<ResultVM> AddSchool(object newSchool)
        {
            if (ModelState.IsValid)
            {
                var model = JsonConvert.DeserializeObject<SchoolVM>(newSchool.ToString());

                School school = new School
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = model.Name,
                    Address = model.Address,
                    Mail = model.Mail,
                    Phone = model.Phone,
                    Type = model.Type
                };

                await _repositoryWrapper.SchoolRepository.CreateAsync(school);

                return new ResultVM
                {
                    Status = Status.Success,
                    Message = "Școala a fost adăugată cu succes"
                };

            }

            return new ResultVM
            {
                Status = Status.Error,
                Message = "Date incorecte"
            };
        }

        [HttpGet("getAllTeachers")]
        public async Task<List<TeacherVM>> GetAllTeachers()
        {
            List<TeacherVM> teacherVMs = new List<TeacherVM>();
            var teachers = await _repositoryWrapper.TeacherRepository.GetAllAsync();
            foreach (var teacher in teachers)
            {
                var member = await _userManager.FindByIdAsync(teacher.MemberId);
                teacherVMs.Add(new TeacherVM
                {
                    MemberId = member.Id,
                    FirstName = member.FirstName,
                    LastName = member.LastName,
                    Department = teacher.Department,
                    SchoolId = member.SchoolId
                });
            }

            return teacherVMs;
        }

        [HttpGet("getAllStudents")]
        public async Task<List<StudentVM>> GetAllStudents()
        {
            List<StudentVM> studentVMs = new List<StudentVM>();
            var students = await _repositoryWrapper.StudentRepository.GetAllAsync();
            foreach (var student in students)
            {
                var member = await _userManager.FindByIdAsync(student.MemberId);
                studentVMs.Add(new StudentVM
                {
                    MemberId = member.Id,
                    FirstName = member.FirstName,
                    LastName = member.LastName,
                    Grade = student.Grade,
                    FieldOfStudy = student.FieldOfStudy,
                    SchoolId = member.SchoolId
                });
            }

            return studentVMs;
        }

        [HttpPost("addTeachers")]
        public async Task<ResultVM> AddTeacher(object teachers)
        {
            if (ModelState.IsValid)
            {
                var teacherVMs = JsonConvert.DeserializeObject<List<TeacherVM>>(teachers.ToString());

                foreach (var teacher in teacherVMs)
                {
                    var member = await _userManager.FindByIdAsync(teacher.MemberId);
                    member.SchoolId = teacher.SchoolId;
                    await _userManager.UpdateAsync(member);
                }

                return new ResultVM
                {
                    Status = Status.Success,
                    Message = "Actualizare reușită"
                };
            }

            return new ResultVM
            {
                Status = Status.Error,
                Message = "Date incorecte"
            };
        }

        [HttpPost("addStudents")]
        public async Task<ResultVM> AddStudents(object students)
        {
            if (ModelState.IsValid)
            {
                var studentVMs = JsonConvert.DeserializeObject<List<StudentVM>>(students.ToString());

                foreach (var student in studentVMs)
                {
                    var member = await _userManager.FindByIdAsync(student.MemberId);
                    member.SchoolId = student.SchoolId;
                    await _userManager.UpdateAsync(member);
                }

                return new ResultVM
                {
                    Status = Status.Success,
                    Message = "Actualizare reușită"
                };
            }

            return new ResultVM
            {
                Status = Status.Error,
                Message = "Date incorecte"
            };
        }

        [HttpPut("updateTeacher")]
        public async Task<ResultVM> UpdateTeacher(object routeValue)
        {
            var teacherVM = JsonConvert.DeserializeObject<TeacherVM>(routeValue.ToString());

            if(teacherVM != null)
            {
                var teachers = await _repositoryWrapper.TeacherRepository.GetAllAsync();
                var teacher = teachers.FirstOrDefault(t => t.MemberId == teacherVM.MemberId);

                teacher.Department = teacherVM.Department;

                _repositoryWrapper.TeacherRepository.Update(teacher);

                return new ResultVM
                {
                    Message = "Actualizare reușită",
                    Status = Status.Success
                };
            }

            return new ResultVM {
                Message = "Eroare la deserializare",
                Status = Status.Error
            };
            
        }

        [HttpPut("updateStudent")]
        public async Task<ResultVM> UpdateStudent(object routeValue)
        {
            var studentVM = JsonConvert.DeserializeObject<StudentVM>(routeValue.ToString());

            if (studentVM != null)
            {
                var students = await _repositoryWrapper.StudentRepository.GetAllAsync();
                var student = students.FirstOrDefault(t => t.MemberId == studentVM.MemberId);

                student.Grade = studentVM.Grade;
                student.FieldOfStudy = studentVM.FieldOfStudy;

                _repositoryWrapper.StudentRepository.Update(student);

                return new ResultVM
                {
                    Message = "Actualizare reușită",
                    Status = Status.Success
                };
            }

            return new ResultVM
            {
                Message = "Eroare la deserializare",
                Status = Status.Error
            };

        }

        [HttpGet("deleteTeacher/{teacherid}")]
        public async Task<ResultVM> RemoveTeacher(string teacherid)
        {
            var teacher = await _userManager.FindByIdAsync(teacherid);
            
            
            if(teacher == null)
            {
                return new ResultVM
                {
                    Message = "Utilizatorul nu a fost găsit",
                    Status = Status.Error
                };
            }

            teacher.SchoolId = null;
            await _userManager.UpdateAsync(teacher);

            return new ResultVM
            {
                Message = "Operație reușită",
                Status = Status.Success
            };
        }

        [HttpGet("deleteStudent/{studentid}")]
        public async Task<ResultVM> RemoveStudent(string studentid)
        {
            var student = await _userManager.FindByIdAsync(studentid);


            if (student == null)
            {
                return new ResultVM
                {
                    Message = "Utilizatorul nu a fost găsit",
                    Status = Status.Error
                };
            }

            student.SchoolId = null;
            await _userManager.UpdateAsync(student);

            return new ResultVM
            {
                Message = "Operație reușită",
                Status = Status.Success
            };
        }
    }
}