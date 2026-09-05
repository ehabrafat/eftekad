using System.ComponentModel.DataAnnotations.Schema;
using Eftekad.Data;
using Eftekad.Features.Members;

namespace Eftekad.Features.Users;

public class User : BaseEntity
{
    public string? Name { get; set; } 

    public int? MemberId { get; set; }
    
    public Member? Member { get; set; }
    public string Username { get; set; } 
    public string Password { get; set; } 
    public string Role { get; set; } = Auth.Role.User;
    public string? Church { get; set; }
    public string? Email { get; set; }
    public string? ProfilePic { get; set; } 
}