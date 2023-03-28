using DataBundle.Models;

public interface IJWTBearerConfiguration
{
    string GenerateToken(User user);
}