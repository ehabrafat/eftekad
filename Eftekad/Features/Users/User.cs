using System.ComponentModel.DataAnnotations.Schema;
using Eftekad.Data;

namespace Eftekad.Features.Users;

public class User : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Role { get; set; } = Auth.Role.User;

    public string Church { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string ProfilePic { get; set; } = string.Empty;
}