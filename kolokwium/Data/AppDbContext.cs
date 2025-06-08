using kolokwium.Models;
using Microsoft.EntityFrameworkCore;

namespace kolokwium.Data;

public class AppDbContext: DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupAssigment> GroupAssigments { get; set; }
    
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
            Age = 25,
            EntranceExamScore = 73.3f
        };

        var group = new Group
        {
            Id = 1,
            Name = "Group"
        };

        var groupAssigments = new GroupAssigment
        {
            StudentId = 1,
            GroupId = 1
        };
        
        modelBuilder.Entity<Student>().HasData(student);
        modelBuilder.Entity<Group>().HasData(group);
        modelBuilder.Entity<GroupAssigment>().HasData(groupAssigments);
    }
}