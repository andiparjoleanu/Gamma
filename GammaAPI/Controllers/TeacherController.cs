using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gamma;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GammaAPI.ViewModels;
using Newtonsoft.Json;
using Gamma.Models;

namespace GammaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public TeacherController(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        [HttpGet("getCourses/{teacherid}")]
        public async Task<List<CourseVM>> GetCourses(string teacherid)
        {
            var courses = await _repositoryWrapper.CourseRepository.GetAllAsync();
            var teacherCourses = courses.Where(c => c.TeacherId == teacherid);

            List<CourseVM> courseVMs = new List<CourseVM>();

            if (teacherCourses != null)
            {
                foreach (var course in teacherCourses)
                {
                    courseVMs.Add(new CourseVM
                    {
                        CourseId = course.Id,
                        Name = course.Name,
                        Description = course.Description,
                        Grade = course.Grade,
                        FieldOfStudy = course.FieldOfStudy
                    });
                }
            }

            return courseVMs;
        }

        [HttpGet("getCourse/{courseid}")]
        public async Task<CourseVM> GetCourse(string courseid)
        {
            var courses = await _repositoryWrapper.CourseRepository.GetAllAsync();
            var course = courses.FirstOrDefault(c => c.Id == courseid);

            return new CourseVM
            {
                CourseId = course.Id,
                Name = course.Name,
                Description = course.Description,
                FieldOfStudy = course.FieldOfStudy,
                Grade = course.Grade
            };
        }

        [HttpPut("editCourse")]
        public async Task<ResultVM> EditCourse(object routeValue)
        {
            var courseVM = JsonConvert.DeserializeObject<CourseVM>(routeValue.ToString());

            if (courseVM != null)
            {
                var courses = await _repositoryWrapper.CourseRepository.GetAllAsync();
                var course = courses.FirstOrDefault(c => c.Id == courseVM.CourseId);

                course.Description = courseVM.Description;
                course.Grade = courseVM.Grade;

                _repositoryWrapper.CourseRepository.Update(course);

                return new ResultVM
                {
                    Status = Status.Success,
                    Message = "Actualizare reușită"
                };
            }

            return new ResultVM
            {
                Status = Status.Error,
                Message = "Cererea nu a putut fi procesată"
            };
        }

        [HttpPost("createCourse")]
        public async Task<ResultVM> CreateCourse(object routeValue)
        {
            var courseVM = JsonConvert.DeserializeObject<CourseVM>(routeValue.ToString());

            if (courseVM != null)
            {
                var teachers = await _repositoryWrapper.TeacherRepository.GetAllAsync();
                var teacher = teachers.FirstOrDefault(t => t.MemberId == courseVM.TeacherId);

                Course course = new Course
                {
                    Id = Guid.NewGuid().ToString(),
                    Description = courseVM.Description,
                    Name = courseVM.Name,
                    TeacherId = courseVM.TeacherId,
                    Grade = courseVM.Grade,
                    FieldOfStudy = teacher.Department
                };

                await _repositoryWrapper.CourseRepository.CreateAsync(course);

                return new ResultVM
                {
                    Status = Status.Success,
                    Message = "Operație reușită"
                };
            }

            return new ResultVM
            {
                Status = Status.Error,
                Message = "Cererea nu a putut fi procesată"
            };
        }
    }
}