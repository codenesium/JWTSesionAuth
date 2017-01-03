using Codenesium.DataConversionExtensions;
using System;
using Codenesium.Encryption;
using FluentAssertions;

namespace Codenesium.JWTSessionAuth
{
    public class SessionManager : ISessionManager
    {
        private IEncryptionManager _encryptionManager;
        private string _hmacKey;
        private int _sessionExpirationTimeInMinutes;
        private string _siteName;
        private ITokenValidator _tokenValidator;

        public SessionManager(
            IEncryptionManager encryptionManager,
            ITokenValidator tokenValidator,
            string hmacKey,
            int sessionExpirationTimeInMinutes,
            string siteName
            )
        {
            encryptionManager.Should().NotBeNull();
            tokenValidator.Should().NotBeNull();
            hmacKey.Should().NotBeNullOrEmpty();
            sessionExpirationTimeInMinutes.Should().BeGreaterThan(0);
            siteName.Should().NotBeNullOrEmpty();

            this._encryptionManager = encryptionManager;
            this._hmacKey = hmacKey;
            this._sessionExpirationTimeInMinutes = sessionExpirationTimeInMinutes;
            this._siteName = siteName;
            this._tokenValidator = tokenValidator;
        }

        public string UnpackageToken(string token)
        {
            token.Should().NotBeNullOrEmpty();

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
            token.Should().NotBeNullOrEmpty();

            SessionPayload requestSessionPayload = new SessionPayload();
            string unpackagedToken = this.UnpackageToken(token);
            if (unpackagedToken == String.Empty)
            {
                return null;
            }
            else
            {
                requestSessionPayload.LoadJson(unpackagedToken);

                if (this._tokenValidator.IsTokenActive(requestSessionPayload.CreatedDate.ToDateTime(), this._sessionExpirationTimeInMinutes))
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
            payload.Should().NotBeNullOrEmpty();

            return this._encryptionManager.JWTEncode(payload, this._hmacKey);
        }

        public string GenerateSessionPayload(
            string createdDate,
            string expirationDate,
            string email,
            int userId,
            string siteName)
        {
            createdDate.Should().NotBeNullOrEmpty();
            expirationDate.Should().NotBeNullOrEmpty();
            email.Should().NotBeNullOrEmpty();
            siteName.Should().NotBeNullOrEmpty();
            userId.Should().BeGreaterThan(0);

            SessionPayload payload = new SessionPayload(createdDate, expirationDate, email, userId.ToString(), siteName);
            return payload.ToJson();
        }
    }
}