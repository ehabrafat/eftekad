using Eftekad.Data;

namespace Eftekad.Features.Auth;

public static class Role
{
    public static string SuperAdmin { get; set; } = "super_admin";
    
    public static string Admin { get; set; } = "admin";
    public static string User { get; set; } = "user";
}