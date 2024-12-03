using Microsoft.AspNetCore.Http;

public static class SessionHelper
{
    public static int GetUserIdFromSession(IHttpContextAccessor httpContextAccessor)
    {
        // IHttpContextAccessor'dan HttpContext alınır
        var userIdString = httpContextAccessor.HttpContext?.Session.GetString("UserId");

        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
        {
            throw new UnauthorizedAccessException("User not authenticated or invalid UserId.");
        }

        return userId;
    }
}