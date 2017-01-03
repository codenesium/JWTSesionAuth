using System;

namespace Codenesium.JWTSessionAuth
{
    public interface ISessionManager
    {
        Session Authenticate(string token);

        string GenerateSessionPayload(string createdDate, string expirationDate, string email, int userId, string siteName);

        string PackagePayload(string payload);

        string UnpackageToken(string token);
    }
}