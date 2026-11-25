using DalilAcademy.Core.Entities;
using DalilAcademy.Core.Exceptions;
using DalilAcademy.Core.Repository;
using DalilAcademy.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace DalilAcademy.Infra.Repository;

public class EfCourseRepository(AppDbContext db)
    : ICourseRepository
{
    // Include(Instructor) so CourseResponseDto.InstructorName is filled (Lecture 6)
    public async Task<List<Course>> GetAllAsync()
    {
        return await db.Courses
            .Include(c => c.Instructor)
            .ToListAsync();
    }

    public async Task<Course?> GetByIdAsync(int id)
    {
        return await db.Courses
            .Include(c => c.Instructor)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Course> CreateAsync(Course course)
    {
        try
        {
            db.Courses.Add(course);
            await db.SaveChangesAsync();

            // Load the instructor so the response can show InstructorName
            await db.Entry(course).Reference(c => c.Instructor).LoadAsync();

            return course;
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is PostgresException
            {
                SqlState: PostgresErrorCodes.UniqueViolation
            })
        {
            throw new ConflictException(
                "A course with this title already exists.");
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is PostgresException
            {
                SqlState: PostgresErrorCodes.ForeignKeyViolation
            })
        {
            throw new NotFoundException(
                $"Instructor {course.InstructorId} was not found.");
        }
    }

    public async Task UpdateAsync(Course course)
    {
        try
        {
            db.Courses.Update(course);
            await db.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is PostgresException
            {
                SqlState: PostgresErrorCodes.UniqueViolation
            })
        {
            throw new ConflictException(
                "A course with this title already exists.");
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var course = await db.Courses.FindAsync(id);

        if (course is null)
            return false;

        db.Courses.Remove(course);
        await db.SaveChangesAsync();

        return true;
    }
}
