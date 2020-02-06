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

namespace GammaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly UserManager<Member> _userManager;

        public StudentController(IRepositoryWrapper repositoryWrapper,
                                 UserManager<Member> userManager)
        {
            _repositoryWrapper = repositoryWrapper;
            _userManager = userManager;
        }

        [HttpGet("getCourses/{studentid}")]
        public async Task<List<CourseVM>> GetCourses(string studentid)
        {
            var courses = await _repositoryWrapper.CourseRepository.GetAllAsync();
            var studentsCourses = await _repositoryWrapper.StudentCourseRepository.GetAllAsync();
            studentsCourses = studentsCourses.Where(sc => sc.StudentId == studentid).ToList();
            
            List<CourseVM> coursesOfStudent = new List<CourseVM>();

            if(studentsCourses != null)
            {
                coursesOfStudent = courses.Join(studentsCourses,
                                                course => course.Id,
                                                studentCourse => studentCourse.CourseId,
                                                (course, studentCourse) =>
                                                 new CourseVM
                                                 {
                                                     CourseId = course.Id,
                                                     Description = course.Description,
                                                     FieldOfStudy = course.FieldOfStudy,
                                                     Lesson = course.Lesson,
                                                     Grade = course.Grade,
                                                     Name = course.Name,
                                                     TeacherId = course.TeacherId
                                                 }).ToList();
            }
            
            return coursesOfStudent;
        }

        [HttpGet("getStudentInfo/{studentid}")]
        public async Task<StudentVM> GetStudentInfo(string studentid)
        {
            var students = await _repositoryWrapper.StudentRepository.GetAllAsync();
            var student = students.FirstOrDefault(s => s.MemberId == studentid);
            var member = await _userManager.FindByIdAsync(studentid);

            StudentVM model;

            if(student != null)
            {
                model = new StudentVM
                {
                    FieldOfStudy = student.FieldOfStudy,
                    Grade = student.Grade,
                    SchoolId = member.SchoolId,
                    FirstName = member.FirstName,
                    LastName = member.LastName,
                    MemberId = member.Id
                };
            }
            else
            {
                model = new StudentVM();
            }

            return model;
        }

        [HttpGet("getMarks/{studentid}/{courseid}")]
        public async Task<List<MarkVM>> GetMarks(string studentid, string courseid)
        {
            List<MarkVM> markVMs = new List<MarkVM>();

            var marks = await _repositoryWrapper.MarkRepository.GetAllAsync();
            var studentMarks = marks.Where(m => m.CourseId == courseid && m.StudentId == studentid);

            if (studentMarks != null)
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
    }
}