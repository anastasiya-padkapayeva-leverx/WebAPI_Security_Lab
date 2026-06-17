using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecurityLab.Api.Common;
using SecurityLab.Api.Contracts;
using SecurityLab.Api.Dtos;

namespace SecurityLab.Api.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService) => _adminService = adminService;

    [HttpGet("users")]
    public async Task<ActionResult<IReadOnlyList<UserDetailResponse>>> GetUsers(CancellationToken ct)
    {
        return Ok(await _adminService.GetUsersAsync(ct));
    }

    [HttpGet("users/search")]
    public async Task<ActionResult<IReadOnlyList<UserDetailResponse>>> SearchUsers([FromQuery] string email, CancellationToken ct)
    {
        return Ok(await _adminService.SearchUsersAsync(email, ct));
    }

    [HttpDelete("users/{id:int}")]
    public async Task<IActionResult> DeleteUser(int id, CancellationToken ct)
    {
        try
        {
            await _adminService.DeleteUserAsync(id, ct);
            return Ok();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
