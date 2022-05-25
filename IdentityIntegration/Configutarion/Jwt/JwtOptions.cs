namespace SecurityIntegration.Configutarion.Jwt;

public class JwtOptions
{
    public string Secret { get; set; }
    public int ExpireInMinutes { get; set; }
}
