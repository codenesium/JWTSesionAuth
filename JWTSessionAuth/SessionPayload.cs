using FluentAssertions;
using Newtonsoft.Json;

namespace Codenesium.JWTSessionAuth
{
    public class SessionPayload
    {
        public string Email { get; private set; }
        public string UserId { get; private set; }
        public string CreatedDate { get; private set; }
        public string ExpirationDate { get; private set; }
        public string SiteName { get; private set; }

        public SessionPayload(
            string createdDate,
            string expirationDate,
            string email,
            string userId,
            string siteName)
        {
            createdDate.Should().NotBeNullOrEmpty();
            expirationDate.Should().NotBeNullOrEmpty();
            email.Should().NotBeNullOrEmpty();
            siteName.Should().NotBeNullOrEmpty();
            userId.Should().NotBeNullOrEmpty();

            this.Email = email;
            this.CreatedDate = createdDate;
            this.UserId = userId;
            this.ExpirationDate = expirationDate;
            this.SiteName = siteName;
        }

        public SessionPayload()
        {
        }

        public void LoadJson(string json)
        {
            var result = JsonConvert.DeserializeObject<dynamic>(json);
            this.Email = result.Email;
            this.UserId = result.UserId;
            this.CreatedDate = result.CreatedDate;
            this.ExpirationDate = result.ExpirationDate;
            this.SiteName = result.SiteName;

            this.CreatedDate.Should().NotBeNullOrEmpty();
            this.ExpirationDate.Should().NotBeNullOrEmpty();
            this.Email.Should().NotBeNullOrEmpty();
            this.SiteName.Should().NotBeNullOrEmpty();
            this.UserId.Should().NotBeNullOrEmpty();
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}