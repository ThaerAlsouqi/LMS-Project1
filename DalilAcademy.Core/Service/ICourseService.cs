using DalilAcademy.Core.DTOs.Courses;

namespace DalilAcademy.Core.Service;

// Lecture 6: the service speaks DTOs and throws exceptions instead of returning null/bool
public interface ICourseService
{
    Task<List<CourseResponseDto>> GetAllAsync();

    Task<CourseResponseDto> GetByIdAsync(int id);

    Task<CourseResponseDto> CreateAsync(CreateCourseDto dto);

    Task UpdateAsync(int id, UpdateCourseDto dto);

    Task DeleteAsync(int id);
}
