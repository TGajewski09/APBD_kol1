using kolokwium.Models;
using Microsoft.EntityFrameworkCore;

namespace kolokwium.Data;

public class AppDbContext: DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var student = new Student
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "j.doe@wp.pl"
        };

        var course = new Course
        {
            Id = 1,
            Title = "Course1",
            Credits = "Credits",
            Teacher = "Jimmy Jammy"
        };

        var enrollment = new Enrollment
        {
            StudentId = 1,
            CourseId = 1,
            EnrollmentDate = DateTime.Now
        };
        
        modelBuilder.Entity<Student>().HasData(student);
        modelBuilder.Entity<Course>().HasData(course);
        modelBuilder.Entity<Enrollment>().HasData(enrollment);
    }
}