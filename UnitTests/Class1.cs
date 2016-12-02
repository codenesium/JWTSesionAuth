using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;
using Codenesium.JWTSessionAuth;
using Codenesium.Encryption;
using Codenesium.DataConversionExtensions;

namespace UnitTests
{
    public class Class1
    {
        [Test]
        public void GenerateToken()
        {
            SessionManager manager = new SessionManager(
                new EncryptionManager(new Codenesium.Encryption.BCryptor(), new Codenesium.Encryption.JWTHelper(), new Codenesium.Encryption.HashSHA256(), new Codenesium.Encryption.SaltBCrypt()),
                "secretkey", 60 * 60 * 24 * 7, "fermatafish");
            string result = manager.PackagePayload(SessionManager.GenerateSessionPayload(DateTime.UtcNow.ToCompleteDateString(), DateTime.UtcNow.AddDays(7).ToCompleteDateString(), "test@test.com", 1, "fermatafish"));
            System.Diagnostics.Debug.Print(result);

            SessionPayload requestSessionPayload = new SessionPayload();
            string unpackagedToken = manager.UnpackageToken(result);

            requestSessionPayload.LoadJson(unpackagedToken);

            Assert.IsTrue(SessionManager.IsTokenActive(requestSessionPayload.CreatedDate.ToDateTime(), 60 * 60 * 24 * 7));
        }
    }
}