using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codenesium.JWTSessionAuth
{
    public class TokenValidator : ITokenValidator
    {
        public bool IsTokenActive(DateTime date, int expirationTimeInMinutes)
        {
            /*
             * A session date is when it's created.
             * It expires in the future from that date.
             * We add the sesion expiration interval to the date and if it's
             * greater than now we know the session isn't expired.
            */
            if (date.AddMinutes(expirationTimeInMinutes) > DateTime.UtcNow)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}