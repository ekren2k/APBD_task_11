namespace APBD_task_11.Services.Token;

public interface ITokenService
{
    string GenerateToken(string username, string role);
}