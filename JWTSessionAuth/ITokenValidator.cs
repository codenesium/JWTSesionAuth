using System;

namespace Codenesium.JWTSessionAuth
{
    public interface ITokenValidator
    {
        bool IsTokenActive(DateTime date, int expirationTimeInMinutes);
    }
}