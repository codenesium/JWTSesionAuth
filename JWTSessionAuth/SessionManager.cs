using Codenesium.DataConversionExtensions;
using System;
using Codenesium.Encryption;

namespace Codenesium.JWTSessionAuth
{
    public class SessionManager
    {
        private IEncryptionManager _encryptionManager;
        private string _hmacKey;
        private int _sessionExpirationTimeInMinutes;
        private string _siteName;

        public SessionManager(IEncryptionManager encryptionManager, string hmacKey, int sessionExpirationTimeInMinutes, string siteName)
        {
            this._encryptionManager = encryptionManager;
            this._hmacKey = hmacKey;
            this._sessionExpirationTimeInMinutes = sessionExpirationTimeInMinutes;
            this._siteName = siteName;
        }

        public string UnpackageToken(string token)
        {
            if (this._encryptionManager.JWTVerify(token, this._hmacKey))
            {
                return this._encryptionManager.JWTDecode(token, this._hmacKey);
            }
            else
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Verify this user has a valid session
        /// </summary>
        /// <param name="session"></param>
        /// <param name="userId"></param>
        public Session Authenticate(string token)
        {
            SessionPayload requestSessionPayload = new SessionPayload();
            string unpackagedToken = this.UnpackageToken(token);
            if (unpackagedToken == String.Empty)
            {
                return null;
            }
            else
            {
                requestSessionPayload.LoadJson(unpackagedToken);

                if (IsTokenActive(requestSessionPayload.CreatedDate.ToDateTime(), this._sessionExpirationTimeInMinutes))
                {
                    SessionPayload responseSessionPayload = new SessionPayload(DateTime.UtcNow.ToCompleteDateString(), requestSessionPayload.CreatedDate.ToDateTime().AddMinutes(this._sessionExpirationTimeInMinutes).ToCompleteDateString(), requestSessionPayload.Email, requestSessionPayload.UserId.ToString(), this._siteName);
                    return new Session(requestSessionPayload.CreatedDate.ToDateTime(), requestSessionPayload.Email, requestSessionPayload.UserId.ToInt(), token, this.PackagePayload(responseSessionPayload.ToJson()));
                }
                else
                {
                    return null;
                }
            }
        }

        public string PackagePayload(string payload)
        {
            return this._encryptionManager.JWTEncode(payload, this._hmacKey);
        }

        public static string GenerateSessionPayload(string createdDate, string expirationDate, string email, int userId, string siteName)
        {
            SessionPayload payload = new SessionPayload(createdDate, expirationDate, email, userId.ToString(), siteName);
            return payload.ToJson();
        }

        public static bool IsTokenActive(DateTime date, int expirationTimeInMinutes)
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