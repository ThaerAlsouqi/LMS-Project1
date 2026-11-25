using DalilAcademy.Core.Entities;
using DalilAcademy.Core.Exceptions;
using DalilAcademy.Core.Repository;
using DalilAcademy.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace DalilAcademy.Infra.Repository;

public class StudentRepository(AppDbContext db)
    : IStudentRepository
{
    public async Task<List<Student>> GetAllAsync()
    {
        return await db.Students.ToListAsync();
    }

    public async Task<Student?> GetByIdAsync(int id)
    {
        return await db.Students.FindAsync(id);
    }

    // Lecture 5 bonus: Include + ThenInclude
    public async Task<Student?> GetWithCoursesAsync(int id)
    {
        return await db.Students
            .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course)
                    .ThenInclude(c => c!.Instructor)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Student> CreateAsync(Student student)
    {
        try
        {
            db.Students.Add(student);
            await db.SaveChangesAsync();

            return student;
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is PostgresException
            {
                SqlState: PostgresErrorCodes.UniqueViolation
            })
        {
            throw new ConflictException(
                "A student with this email already exists.");
        }
    }

    public async Task UpdateAsync(Student student)
    {
        try
        {
            db.Students.Update(student);
            await db.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is PostgresException
            {
                SqlState: PostgresErrorCodes.UniqueViolation
            })
        {
            throw new ConflictException(
                "A student with this email already exists.");
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var student = await db.Students.FindAsync(id);

        if (student is null)
            return false;

        db.Students.Remove(student);
        await db.SaveChangesAsync();

        return true;
    }
}
