using DalilAcademy.Core.DTOs.Courses;
using DalilAcademy.Core.DTOs.Students;

namespace DalilAcademy.Core.Service;

public interface IStudentService
{
    Task<List<StudentResponseDto>> GetAllAsync();

    Task<StudentResponseDto> GetByIdAsync(int id);

    Task<StudentResponseDto> CreateAsync(CreateStudentDto dto);

    Task UpdateAsync(int id, UpdateStudentDto dto);

    Task DeleteAsync(int id);

    // Lecture 5 bonus: GET /api/students/{id}/courses
    Task<List<CourseResponseDto>> GetCoursesAsync(int id);
}
