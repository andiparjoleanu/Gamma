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
using Microsoft.AspNetCore.Identity;

namespace GammaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly UserManager<Member> _userManager;

        public TeacherController(IRepositoryWrapper repositoryWrapper,
                                 UserManager<Member> userManager)
        {
            _repositoryWrapper = repositoryWrapper;
            _userManager = userManager;
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

                if (course.Grade != courseVM.Grade)
                {
                    var studentCourses = await _repositoryWrapper.StudentCourseRepository.GetAllAsync();

                    foreach (var studentCourse in studentCourses)
                    {
                        if (studentCourse.CourseId == courseVM.CourseId)
                        {
                            _repositoryWrapper.StudentCourseRepository.Delete(studentCourse);
                        }
                    }

                }

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

        [HttpGet("getTeacherInfo/{teacherid}")]
        public async Task<TeacherVM> GetTeacherInfo(string teacherid)
        {
            var teachers = await _repositoryWrapper.TeacherRepository.GetAllAsync();
            var teacher = teachers.FirstOrDefault(t => t.MemberId == teacherid);

            return new TeacherVM
            {
                MemberId = teacherid,
                Department = teacher.Department
            };
        }

        [HttpGet("getStudents/{courseid}")]
        public async Task<List<StudentVM>> GetStudents(string courseid)
        {
            var students = await _repositoryWrapper.StudentRepository.GetAllAsync();
            var studentMembers = _userManager.Users.ToList().Join(students, u => u.Id, s => s.MemberId,
                                (u, s) => new { u.FirstName, u.LastName, u.Id, u.SchoolId, s.FieldOfStudy, s.Grade });

            var studentsCourses = await _repositoryWrapper.StudentCourseRepository.GetAllAsync();

            var result = studentMembers.Join(studentsCourses, s => s.Id, sc => sc.StudentId,
                            (student, studentCourse) => new { student, studentCourse.CourseId })
                            .Where(ob => ob.CourseId == courseid);


            List<StudentVM> studentVMs = new List<StudentVM>();

            foreach (var ob in result)
            {
                var student = ob.student;

                StudentVM studentVM = new StudentVM
                {
                    FieldOfStudy = student.FieldOfStudy,
                    Grade = student.Grade,
                    MemberId = student.Id,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    SchoolId = student.SchoolId
                };

                studentVMs.Add(studentVM);
            }

            return studentVMs;
        }

        [HttpGet("getStudentsToJoinIn/{courseid}/{schoolid}")]
        public async Task<List<StudentVM>> GetStudentsToJoinIn(string courseid, string schoolid)
        {
            var courses = await _repositoryWrapper.CourseRepository.GetAllAsync();
            var course = courses.FirstOrDefault(c => c.Id == courseid);

            List<StudentVM> studentVMs = new List<StudentVM>();

            if (course != null)
            {
                var students = await _repositoryWrapper.StudentRepository.GetAllAsync();
                var studentCourses = await _repositoryWrapper.StudentCourseRepository.GetAllAsync();

                foreach (var student in students)
                {
                    var member = await _userManager.FindByIdAsync(student.MemberId);
                    var studentCourse = studentCourses.FirstOrDefault(s => s.StudentId == member.Id);
                    

                    if (student.FieldOfStudy == course.FieldOfStudy &&
                       student.Grade == course.Grade &&
                       member.SchoolId == schoolid &&
                       studentCourse == null)
                    {
                        studentVMs.Add(new StudentVM
                        {
                            FirstName = member.FirstName,
                            LastName = member.LastName,
                            FieldOfStudy = student.FieldOfStudy,
                            Grade = student.Grade,
                            MemberId = member.Id,
                            IsSelected = false
                        });
                    }
                }
            }

            return studentVMs;
        }

        [HttpPost("subscribeStudents")]
        public async Task<ResultVM> SubscribeStudents(object routeValues)
        {
            var model = JsonConvert.DeserializeObject<SubscribeStudentsVM>(routeValues.ToString());

            if(model != null)
            {
                var students = model.Students;

                foreach (var student in students)
                {
                    if (student.IsSelected)
                    {
                        StudentCourse studentCourse = new StudentCourse
                        {
                            StudentId = student.MemberId,
                            CourseId = model.CourseId
                        };

                        await _repositoryWrapper.StudentCourseRepository.CreateAsync(studentCourse);
                    }
                }

                return new ResultVM
                {
                    Status = Status.Success,
                    Message = "Actualizare reușită"
                };
            }

            return new ResultVM
            {
                Message = "Cererea nu a putut fi procesată",
                Status = Status.Error
            };
            
        }

        [HttpGet("removeCourse/{courseid}")]
        public async Task<ResultVM> RemoveCourse(string courseid)
        {
            var courses = await _repositoryWrapper.CourseRepository.GetAllAsync();
            var course = courses.FirstOrDefault(c => c.Id == courseid);

            _repositoryWrapper.CourseRepository.Delete(course);

            return new ResultVM
            {
                Status = Status.Success,
                Message = "Operație reușită"
            };
        }

        [HttpGet("getMarks/{studentid}/{courseid}")]
        public async Task<List<MarkVM>> GetMarks(string studentid, string courseid)
        {
            List<MarkVM> markVMs = new List<MarkVM>();

            var marks = await _repositoryWrapper.MarkRepository.GetAllAsync();
            var studentMarks = marks.Where(m => m.CourseId == courseid && m.StudentId == studentid);

            if(studentMarks != null)
            {
                foreach (var mark in studentMarks)
                {
                    markVMs.Add(new MarkVM
                    {
                        StudentId = studentid,
                        CourseId = courseid,
                        Date = mark.Date,
                        Note = mark.Note,
                        Value = mark.Value
                    });
                }
            }

            return markVMs;
        }

        [HttpPost("addMark")]
        public async Task<ResultVM> AddMark(object routeValue)
        {
            var model = JsonConvert.DeserializeObject<MarkVM>(routeValue.ToString());

            if (model != null)
            {
                await _repositoryWrapper.MarkRepository.CreateAsync(new Mark
                {
                    Id = Guid.NewGuid().ToString(), 
                    CourseId = model.CourseId,
                    StudentId = model.StudentId,
                    Date = model.Date,
                    Note = model.Note,
                    Value = model.Value
                });

                return new ResultVM
                {
                    Status = Status.Success,
                    Message = "Operație reușită"
                };
            }

            return new ResultVM
            {
                Status = Status.Error,
                Message = "Cererea nu a putut fi executată"
            };
        }
    }
}