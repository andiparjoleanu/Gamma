using Gamma.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gamma.Context
{
    public class GammaContext : IdentityDbContext<Member>
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Mark> Marks { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

        public GammaContext(DbContextOptions options) : base(options) { }

        public GammaContext() { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<School>(entity =>
            {
                entity.HasKey(x => x.Id);
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.HasOne(x => x.School)
                      .WithMany(x => x.Members)
                      .HasForeignKey(x => x.SchoolId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(x => x.MemberId);

                entity.HasOne(x => x.Member)
                      .WithOne(x => x.Student)
                      .HasForeignKey("Student", "MemberId")
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Teacher>(entity => 
            {
                entity.HasKey(x => x.MemberId);

                entity.HasOne(x => x.Member)
                      .WithOne(x => x.Teacher)
                      .HasForeignKey("Teacher", "MemberId")
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.HasOne(x => x.Teacher)
                      .WithMany(x => x.Courses)
                      .HasForeignKey(x => x.TeacherId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<StudentCourse>(entity =>
            {
                entity.HasKey(x => new { x.StudentId, x.CourseId });

                entity.HasOne(x => x.Course)
                      .WithMany(x => x.StudentCourses)
                      .HasForeignKey(x => x.CourseId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.Student)
                      .WithMany(x => x.StudentCourses)
                      .HasForeignKey(x => x.StudentId)
                      .OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<Mark>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.HasOne(x => x.StudentCourse)
                      .WithMany(x => x.Marks)
                      .HasForeignKey(x => new { x.StudentId, x.CourseId })
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
