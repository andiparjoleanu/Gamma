using System;
using System.Linq;
using Gamma.Models;

namespace Gamma
{
    class Program
    {
        static void Main(string[] args)
        {

        }
    }
}

/*
public delegate void MyDelegate();

static void Main(string[] args)
{
    Console.WriteLine("Hello World!");


    Console.WriteLine("Welcome! Please choose a number to continue:");

    while (true)
    {
        ShowMenu();

        var optionString = Console.ReadLine();
        var succeded = Int32.TryParse(optionString, out var option);
        var d = succeded
            ? SelectOption(option)
            : ShowError;

        d.Invoke();
    }
}

static MyDelegate SelectOption(int option) => option switch
{
    0 => Exit,
    1 => AddASchool,
    2 => ShowAllMembers,
    3 => AddAMember,
    4 => AddACourse,
    5 => SubscribeToACourse,
    _ => ShowError
};

private struct Interval
{
    public int Start { get; set; }
    public int Final { get; set; }
}

private static Interval GetASCIIInterval(int option) => option switch
{
    0 => new Interval { Start = 48, Final = 57 },
    1 => new Interval { Start = 65, Final = 90 },
    _ => new Interval { Start = 97, Final = 122 },
};

private static string GeneratePassword()
{
    string password = "";
    Random rnd = new Random(DateTime.Now.Millisecond);

    for (int i = 0; i < 8; i++)
    {
        Interval interval = GetASCIIInterval(rnd.Next(0, 2));
        password += (char)rnd.Next(interval.Start, interval.Final);
    }

    return password;
}

static void ShowMenu()
{
    Console.WriteLine("0. Exit.");
    Console.WriteLine("1. Add a school");
    Console.WriteLine("2. Show all members.");
    Console.WriteLine("3. Add a member.");
    Console.WriteLine("4. Add a course");
    Console.WriteLine("5. Subscribe to a course");
    Console.WriteLine();
}

static void AddASchool()
{
    using var db = new Context.GammaContext();

    Console.WriteLine("Name: ");
    string name = Console.ReadLine();

    Console.WriteLine("Type: ");
    string type = Console.ReadLine();

    Console.WriteLine("Address: ");
    string address = Console.ReadLine();

    Console.WriteLine("Phone: ");
    string mail = Console.ReadLine();

    db.Schools.Add(new School
    {
        Name = name,
        Type = type,
        Address = address,
        Mail = mail
    });

    db.SaveChanges();

    Console.WriteLine();
}

static void ShowAllMembers()
{
    using var db = new Context.GammaContext();

    db.Teachers.Join(db.Members, teacher => teacher.MemberID, member => member.ID,
        (teacher, member) => new { Name = member.FirstName + " " + member.LastName, teacher.Department })
        .ToList().ForEach(ob => Console.WriteLine(ob.Name + ", Departmentul de " + ob.Department));

    db.Students.Join(db.Members, student => student.MemberID, member => member.ID,
        (student, member) => new { Name = member.FirstName + " " + member.LastName, student.Grade, student.FieldOfStudy })
        .ToList().ForEach(ob => Console.WriteLine(ob.Name + ", Clasa a " + ob.Grade + "-a,  " + ob.FieldOfStudy));

    Console.WriteLine();
}

static void AddACourse()
{
    using var db = new Context.GammaContext();

    Console.WriteLine("Profesori");

    db.Teachers.Join(db.Members, teacher => teacher.MemberID, member => member.ID,
               (teacher, member) => new { member.ID, member.SchoolID, member.FirstName, member.LastName, teacher.Department })
               .Join(db.Schools, ob => ob.SchoolID, school => school.ID, (ob, school) => new { ob.ID, ob.FirstName, ob.LastName, School = school.Name, ob.Department })
               .ToList().ForEach(t => Console.WriteLine(t.ID + " - " + t.FirstName + " " + t.LastName + ", " + t.School + ", " + t.Department));

    Int32.TryParse(Console.ReadLine(), out int id);

    Console.WriteLine("Nume: ");
    string name = Console.ReadLine();

    Console.WriteLine("Descriere: ");
    string description = Console.ReadLine();

    db.Courses.Add(new Course
    {
        Name = name,
        Description = description,
        TeacherID = id
    });

    db.SaveChanges();

    Console.WriteLine();
}

static void AddAMember()
{
    using var db = new Context.GammaContext();

    Console.WriteLine("First Name: ");
    string firstName = Console.ReadLine();

    Console.WriteLine("Last Name: ");
    string lastName = Console.ReadLine();

    Console.WriteLine("Username: ");
    string username = Console.ReadLine();

    string password = GeneratePassword();

    Console.WriteLine("Email: ");
    string mail = Console.ReadLine();

    Console.WriteLine("Phone: ");
    string phone = Console.ReadLine();

    Console.WriteLine("Institut: ");
    db.Schools.ToList().ForEach(school => Console.WriteLine(school.ID + " - " + school.Name));
    Int32.TryParse(Console.ReadLine(), out int schoolID);

    Member newMember = new Member
    {
        FirstName = firstName,
        LastName = lastName,
        Username = username,
        Password = password,
        Mail = mail,
        Phone = phone,
        SchoolID = schoolID
    };

    db.Members.Add(newMember);

    db.SaveChanges();

    Console.WriteLine("Grad (profesor/student):");
    string memberType = Console.ReadLine();

    if(memberType.Contains("profesor"))
    {
        Console.WriteLine("Department: ");
        string department = Console.ReadLine();

        db.Teachers.Add(new Teacher
        {
            Department = department,
            MemberID = newMember.ID                    
        });
    }
    else
    {
        Console.WriteLine("Grade: ");
        Int32.TryParse(Console.ReadLine(), out int grade);

        Console.WriteLine("Field of study: ");
        string fieldOfStudy = Console.ReadLine();

        db.Students.Add(new Student
        {
            Grade = grade,
            FieldOfStudy = fieldOfStudy,
            MemberID = newMember.ID
        });
    }

    db.SaveChanges();

    Console.WriteLine();

}

static void SubscribeToACourse()
{
    using var db = new Context.GammaContext();

    Console.WriteLine("Studenti: ");
    var list = db.Students.Join(db.Members, student => student.MemberID, member => member.ID,
        (student, member) => new { member.ID, member.SchoolID, member.FirstName, member.LastName, student.Grade, student.FieldOfStudy })
        .Join(db.Schools, ob => ob.SchoolID, school => school.ID,
        (ob, school) => new { School = school.Name, ob })
        .ToList();

    list.ForEach(s => Console.WriteLine(s.ob.ID + " - " + s.ob.FirstName + " " + s.ob.LastName + ", clasa a " + s.ob.Grade 
                                                 + "-a, specializarea " + s.ob.FieldOfStudy + ", " + s.School));

    Int32.TryParse(Console.ReadLine(), out int studentID);

    var student = list.Find(x => x.ob.ID == studentID);

    Console.WriteLine("Cursuri: ");
    db.Courses.Join(db.Teachers, course => course.TeacherID, teacher => teacher.MemberID,
        (course, teacher) => new { teacher.MemberID, CourseID = course.ID, CourseName = course.Name, teacher.Department })
        .Join(db.Members, ob => ob.MemberID, member => member.ID,
        (ob, member) => new { member.FirstName, member.LastName, member.SchoolID, TeacherCourse = ob })
        .Join(db.Schools, rez => rez.SchoolID, school => school.ID,
        (rez, school) => new { MemberTeacherCourse = rez, SchoolName = school.Name })
        .Where(x => x.SchoolName == student.School)
        .ToList().ForEach(c => Console.WriteLine(c.MemberTeacherCourse.TeacherCourse.CourseID + " " + c.MemberTeacherCourse.TeacherCourse.CourseName 
                                                 + " - " + c.MemberTeacherCourse.FirstName + " " + c.MemberTeacherCourse.LastName));

    Int32.TryParse(Console.ReadLine(), out int courseID);

    db.StudentCourses.Add(new StudentCourse
    {
        StudentID = studentID,
        CourseID = courseID
    });

    db.SaveChanges();

    Console.WriteLine();
}

static void Exit() => Environment.Exit(0);

static void ShowError() => Console.WriteLine("This option is not yet supported, please choose one from the list.");
}
 */
