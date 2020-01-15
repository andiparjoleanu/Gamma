using Gamma.Context;
using Gamma.IRepositories;
using Gamma.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gamma
{
    public class RepositoryWrapper : IRepositoryWrapper 
    {
        private GammaContext _context;
        private ICourseRepository _courseRepository;
        private IMarkRepository _markRepository;
        private ISchoolRepository _schoolRepository;
        private IStudentCourseRepository _studentCourseRepository;
        private IStudentRepository _studentRepository;
        private ITeacherRepository _teacherRepository;

        public ICourseRepository CourseRepository
        {
            get
            {
                if(_courseRepository == null)
                {
                    _courseRepository = new CourseRepository(_context);
                }

                return _courseRepository;
            }
        }

        public IMarkRepository MarkRepository
        {
            get
            {
                if (_markRepository == null)
                {
                    _markRepository = new MarkRepository(_context);
                }

                return _markRepository;
            }
        }

        public ISchoolRepository SchoolRepository
        {
            get
            {
                if (_schoolRepository == null)
                {
                    _schoolRepository = new SchoolRepository(_context);
                }

                return _schoolRepository;
            }
        }

        public IStudentCourseRepository StudentCourseRepository
        {
            get
            {
                if (_studentCourseRepository == null)
                {
                    _studentCourseRepository = new StudentCourseRepository(_context);
                }

                return _studentCourseRepository;
            }
        }

        public IStudentRepository StudentRepository
        {
            get
            {
                if (_studentRepository == null)
                {
                    _studentRepository = new StudentRepository(_context);
                }

                return _studentRepository;
            }
        }

        public ITeacherRepository TeacherRepository
        {
            get
            {
                if (_teacherRepository == null)
                {
                    _teacherRepository = new TeacherRepository(_context);
                }

                return _teacherRepository;
            }
        }

        public RepositoryWrapper(GammaContext context)
        {
            _context = context;
        }
    }
}
