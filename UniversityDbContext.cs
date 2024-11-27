using Microsoft.EntityFrameworkCore;
using UMS1._0;
using UMS1._0.Models;

public class UniversityDbContext : DbContext
{
    public UniversityDbContext(DbContextOptions<UniversityDbContext> options) : base(options)
    {
    }
    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Unit> Units { get; set; }
    public DbSet<Attendance> Attendance { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {       
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
		
        var connectionString = configuration["ConnectionStrings:DefaultConnection"];
        if (connectionString is not null)
        {
            optionsBuilder.UseMySQL(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var hashingService = new HashingService();
        // Many-to-Many relationship between Students and Courses
        modelBuilder.Entity<Student>()
            .HasMany(s => s.Courses)
            .WithMany(c => c.Students)
            .UsingEntity<Dictionary<string, object>>(
                "StudentsCourses",
                j => j.HasOne<Course>().WithMany().HasForeignKey("CourseID"),
                j => j.HasOne<Student>().WithMany().HasForeignKey("StudentID")
            );

        // One-to-Many relationship between Course and Units
        modelBuilder.Entity<Unit>()
            .HasOne(u => u.Course)
            .WithMany(c => c.Units)
            .HasForeignKey(u => u.CourseID);

        // Many-to-One relationships for Attendance
        modelBuilder.Entity<Attendance>()
            .HasOne(a => a.Student)
            .WithMany(s => s.Attendance)
            .HasForeignKey(a => a.StudentID);

        modelBuilder.Entity<Attendance>()
            .HasOne(a => a.Course)
            .WithMany()
            .HasForeignKey(a => a.CourseID);

        // One-to-Many relationship between Teacher and Courses
        modelBuilder.Entity<Course>()
            .HasOne(c => c.Teacher)
            .WithMany(t => t.Courses)
            .HasForeignKey(c => c.TeacherID);
    }

}
