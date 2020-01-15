using Gamma.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gamma
{
    public interface IRepositoryWrapper
    {
        ICourseRepository CourseRepository { get; }
        IMarkRepository MarkRepository { get; }
        ISchoolRepository SchoolRepository { get; }
        IStudentCourseRepository StudentCourseRepository { get; }
        IStudentRepository StudentRepository { get; }
        ITeacherRepository TeacherRepository { get; }
    }
}
