using TeSystemBackend.Application.DTOs.Teams;

namespace TeSystemBackend.Application.DTOs.Users;

public class UserDto
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public List<TeamDto> Teams { get; set; } = new List<TeamDto>();
}



