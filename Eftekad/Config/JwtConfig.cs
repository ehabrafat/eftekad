namespace Eftekad.Config;

public class JwtConfig
{
    public string Secret { get; set; } = string.Empty;
    public int ExpiresIn { get; set; }
}