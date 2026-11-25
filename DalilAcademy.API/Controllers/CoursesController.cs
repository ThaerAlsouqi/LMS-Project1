using DalilAcademy.Core.DTOs.Courses;
using DalilAcademy.Core.Service;
using Microsoft.AspNetCore.Mvc;

namespace DalilAcademy.API.Controllers;

// Lecture 6: no try/catch, no null checks — the controller only describes the happy path
[ApiController]
[Route("api/[controller]")]
public class CoursesController(ICourseService courseService)
    : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<CourseResponseDto>>> GetAll()
        => Ok(await courseService.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CourseResponseDto>> GetById(int id)
        => Ok(await courseService.GetByIdAsync(id));

    [HttpPost]
    public async Task<ActionResult<CourseResponseDto>> Create(CreateCourseDto dto)
    {
        var created = await courseService.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = created.Id },
            created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateCourseDto dto)
    {
        await courseService.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await courseService.DeleteAsync(id);
        return NoContent();
    }
}
